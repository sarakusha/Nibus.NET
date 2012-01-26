//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsProtocol.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

#endregion

namespace NataInfo.Nibus.Nms
{
    /// <summary>
    /// Front-end класс для взаимодействия с NMS-протоколом.
    /// NMS – NiBUS Message Specification
    /// Сервис NMS позволяет пользовательским приложениям посылать сообщения другим
    /// абонентам, используя стандартный формат сообщения.
    /// </summary>
    /// <seealso cref="NmsCodec.Protocol"/>
    public sealed class NmsProtocol : INibusEndpoint<NmsMessage>
    {
        #region Member Variables

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly CancellationTokenSource _cts;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        /// <param name="incoming">Источник входящих NMS-сообщений.</param>
        /// <param name="outgoing">Канал для исходящих NMS-сообщений.</param>
        internal NmsProtocol(IReceivableSourceBlock<NmsMessage> incoming, ITargetBlock<NmsMessage> outgoing)
        {
            IsDisposed = false;
            _cts = new CancellationTokenSource();

            Timeout = TimeSpan.FromSeconds(1);
            IncomingMessages = incoming;
            OutgoingMessages = outgoing;
            
            if (SynchronizationContext.Current == null)
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }

            var ui = TaskScheduler.FromCurrentSynchronizationContext();
            var infoHandlers = new ActionBlock<NmsMessage>(
                message => OnInformationReport((NmsInformationReport)message),
                new ExecutionDataflowBlockOptions
                    {
                        //CancellationToken = _cts.Token,
                        TaskScheduler = ui
                    });

            IncomingMessages.LinkTo(
                infoHandlers,
                message => message.ServiceType == NmsServiceType.InformationReport);
        }

        #endregion //Constructors

        /// <summary>
        /// Происходит при получении информационного сообщения.
        /// </summary>
        /// <remarks>Вызывается в контексте используемом при вызове контструктора.</remarks>
        public event EventHandler<NmsInformationReportEventArgs> InformationReportReceived;

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Задает величину таймаута ожидания ответа на запросы.
        /// </summary>
        public TimeSpan Timeout { get; set; }
        
        /// <summary>
        /// Возвращает источник входящих NMS-сообщений.
        /// </summary>
        /// <remarks>Относится к типу <see cref="BroadcastBlock{T}"/> в Dataflow TPL.</remarks>
        public IReceivableSourceBlock<NmsMessage> IncomingMessages { get; private set; }
        
        /// <summary>
        /// Возвращает канал для передачи исходящих NMS-сообщений.
        /// </summary>
        public ITargetBlock<NmsMessage> OutgoingMessages { get; private set; }

        #endregion //Properties

        #region Methods
        // ReSharper disable MemberCanBePrivate.Global

        #region Сервис NmsServiceType.Read

        /// <summary>
        /// Проверка доступности устройства.
        /// </summary>
        /// <param name="destanation">Адрес устройства.</param>
        /// <returns>
        /// Возвращает время в миллисекундах, затрачиваемое на отправку запроса
        /// и получение соответствующего сообщения ответа.
        /// </returns>
        /// <value>
        /// <c>-1</c> - устройство не ответило в течение заданного интервала времени <see cref="Timeout"/>
        /// либо выполнение было прервано пользователем или статус ответа содержал ошибку.
        /// </value>
        public long Ping(Address destanation)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(destanation != null);
            return PingAsync(destanation).Result;
        }

        /// <summary>
        /// Асинхронная проверка доступности устройства.
        /// </summary>
        /// <param name="destanation">Адрес устройства.</param>
        /// <returns>
        /// Возвращает время в миллисекундах, затрачиваемое на отправку запроса
        /// и получение соответствующего сообщения ответа.
        /// </returns>
        /// <value>
        /// <c>-1</c> - устройство не ответило в течение заданного интервала времени <see cref="Timeout"/>
        /// либо выполнение было прервано пользователем или статус ответа содержал ошибку.
        /// </value>
        public async Task<long> PingAsync(Address destanation)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(destanation != null);
            var queryVersion = new NmsRead(destanation, 2);
            var sw = new Stopwatch();
            try
            {
                sw.Start();
                await WaitForNmsResponseAsync(queryVersion, 1).ConfigureAwait(false);
                sw.Stop();
            }
            catch (TimeoutException)
            {
                return -1;
            }
            catch (TaskCanceledException)
            {
                return -1;
            }
            catch (NibusResponseException)
            {
                return -1;
            }

            return sw.ElapsedMilliseconds;
        }

        /// <summary>
        /// Асинхронное чтение переменной.
        /// </summary>
        /// <param name="destanation">Адрес устройства, с которого необходимо получить значение переменной.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="attempts">Количество попыток.</param>
        /// <returns><see cref="Task{TResult}"/> представляющий асинхронную операцию чтения.</returns>
        /// <seealso cref="WaitForNmsResponseAsync{TMessage}"/>
        public async Task<object> ReadValueAsync(Address destanation, int id, int attempts = 1)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(destanation != null);
            Contract.Requires(attempts > 0);
            var query = new NmsRead(destanation, id);
            var response = await WaitForNmsResponseAsync(query, attempts);
            return response.Value;
        }

        public async Task ReadManyValuesAsync(IProgress<ReadProgressInfo> progress, Address destanation, params int[] ids)
        {
            Contract.Requires(progress != null);
            Contract.Requires(destanation != null);
            Contract.Requires(ids.Length > 0);
            Contract.Requires(ids.Length <= NmsMessage.MaxReadVariables);
            NmsMessage lastMessage;
            IncomingMessages.TryReceive(null, out lastMessage);
            await OutgoingMessages.SendAsync(new NmsReadMany(destanation, ids));
            var tasks = (from id in ids
                         select ReadResponseAsync(lastMessage, destanation, id)
                             .ContinueWith(t => progress.Report(new ReadProgressInfo(destanation, id, t)))).ToArray();
            try
            {
                await TaskEx.WhenAll(tasks);
            }
            catch (Exception e)
            {
                Logger.ErrorException("Read failed", e);
            }
        }

        #endregion

        #region Сервис NmsServiceType.Write

        /// <summary>
        /// Изменить значение переменной без подтверждения успеха внесения изменений.
        /// </summary>
        /// <param name="destanation">Адрес устройства, на котором требуется изменить значение переменной.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="valueType">Тип значения.</param>
        /// <param name="value">Записываемое значение.</param>
        public void WriteValue(Address destanation, int id, NmsValueType valueType, object value)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(destanation != null);
            Contract.Requires(value != null);
            var write = new NmsWrite(destanation, id, valueType, value, false);
            OutgoingMessages.Post(write);
        }

        /// <summary>
        /// Асинхронно изменить значение переменной без подтверждения успеха внесения изменений.
        /// </summary>
        /// <param name="destanation">Адрес устройства, на котором требуется изменить значение переменной.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="valueType">Тип значения.</param>
        /// <param name="value">Записываемое значение.</param>
        public async Task WriteValueAsync(Address destanation, int id, NmsValueType valueType, object value)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(destanation != null);
            Contract.Requires(value != null);
            var write = new NmsWrite(destanation, id, valueType, value, false);
            await OutgoingMessages.SendAsync(write);
        }

        /// <summary>
        /// Асинхронно изменить значение переменной с подтверждением успеха
        /// внесения изменений за <paramref name="attempts"/> попыток.
        /// </summary>
        /// <param name="destanation">Адрес устройства, на котором требуется изменить значение переменной.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="valueType">Тип значения.</param>
        /// <param name="value">Записываемое значение.</param>
        /// <param name="attempts">Количество попыток.</param>
        /// <returns>
        ///  <see cref="Task"/> асинхронная операция.
        /// </returns>
        public async Task WriteValueConfirmedAsync(
            Address destanation, int id, NmsValueType valueType, object value, int attempts = 1)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(destanation != null);
            Contract.Requires(value != null);
            Contract.Requires(attempts > 0);
            var write = new NmsWrite(destanation, id, valueType, value);
            await WaitForNmsResponseAsync(write, attempts);
        }

        #endregion

        /// <summary>
        /// Отправляет NMS-сообщение <paramref name="query"/>
        /// и ожидает ответа в течении интервала <see cref="Timeout"/>.
        /// В случае отсутствия ответа повторяет попытки <paramref name="attempts"/> раз.
        /// Асинхронная операция.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения, потомок NmsMessage.</typeparam>
        /// <param name="query">Сообщение-запрос.</param>
        /// <param name="attempts">Количество попыток.</param>
        /// <returns>
        ///  <see cref="Task{TResult}"/> асинхронная операция - ответное сообщение.
        /// </returns>
        /// <exception cref="AggregateException">Призошла ошибка в асинхронном коде. См. <see cref="Exception.InnerException"/></exception>
        /// <exception cref="TimeoutException">Ошибка по таймауту.</exception>
        /// <exception cref="TaskCanceledException">Операция прервана пользователем.</exception>
        /// <exception cref="NibusResponseException">Ошибка NiBUS.</exception>
        public async Task<TMessage> WaitForNmsResponseAsync<TMessage>(TMessage query, int attempts) where TMessage : NmsMessage
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(attempts > 0);
            
            // Последнее сообщение в BroadcastBlock всегда остается! Незабываем его фильтровать.
            NmsMessage lastMessage;
            IncomingMessages.TryReceive(null, out lastMessage);

            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            using (IncomingMessages.LinkTo(
                wob, m => !ReferenceEquals(lastMessage, m) && m.IsResponse && m.ServiceType == query.ServiceType
                    && m.Id == query.Id && m.Datagram.Source == query.Datagram.Destanation))
            {
                for (var i = 0; i < attempts; i++)
                {
                    await OutgoingMessages.SendAsync(query);
                    try
                    {
                        var response = (TMessage)await wob.ReceiveAsync(Timeout, _cts.Token).ConfigureAwait(false);
                        if (response.ErrorCode != 0)
                        {
                            throw new NibusResponseException(response.ErrorCode);
                        }

                        return response;
                    }
                    catch (TimeoutException)
                    {
                        if (i < attempts - 1) continue;
                        throw;
                    }
                }
            }

            // Эта точка недостижима при attempts > 0!
            throw new InvalidOperationException();
        }

        // ReSharper restore MemberCanBePrivate.Global
        #endregion //Methods

        #region Implementations

        private void OnInformationReport(NmsInformationReport report)
        {
            var handler = InformationReportReceived;
            if (handler != null)
            {
                var e = new NmsInformationReportEventArgs(report);
                try
                {
                    handler(this, e);
                    if (!e.Identified)
                    {
                        Logger.Debug("Unhandled NMS Information Report: id={0}", report.Id);
                    }
                }
                catch (Exception exception)
                {
                    Logger.ErrorException("NMS Information Report error", exception);
                    Debug.Fail("NMS Information Report error");
                }
            }
        }

        private async Task<NmsRead> ReadResponseAsync(NmsMessage lastMessage, Address source, int id)
        {
            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            using (IncomingMessages.LinkTo(
                wob, m => !ReferenceEquals(lastMessage, m) && m.IsResponse && m.ServiceType == NmsServiceType.Read
                          && m.Id == id && m.Datagram.Source == source))
            {
                var response =
                    (NmsRead)await wob.ReceiveAsync(Timeout, _cts.Token)
                                       .ConfigureAwait(false);
                if (response.ErrorCode != 0)
                {
                    throw new NibusResponseException(response.ErrorCode);
                }
                return response;
            }
        }

        #endregion //Implementations

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (!IsDisposed)
            {
                _cts.Cancel();
                _cts.Dispose();
                IsDisposed = true;
            }
        }

        #endregion

        #region Implementation of ICodecInfo

        public string Description { get; set; }

        #endregion
    }
}