//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsProtocol.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using NataInfo.Nibus.Nms.Services;
using NataInfo.Nibus.Nms.Variables;

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

            Timeout = TimeSpan.FromSeconds(2);
            UploadDownloadTimeout = TimeSpan.FromSeconds(10);
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
        /// Задает величину таймаута ожидания ответа на запросы.
        /// </summary>
        public TimeSpan UploadDownloadTimeout { get; set; }

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
        /// <param name="target">Адрес устройства.</param>
        /// <returns>
        /// Возвращает время в миллисекундах, затрачиваемое на отправку запроса
        /// и получение соответствующего сообщения ответа.
        /// </returns>
        /// <value>
        /// <c>-1</c> - устройство не ответило в течение заданного интервала времени <see cref="Timeout"/>
        /// либо выполнение было прервано пользователем или статус ответа содержал ошибку.
        /// </value>
        public long Ping(Address target)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            return PingAsync(target).Result;
        }

        /// <summary>
        /// Асинхронная проверка доступности устройства.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <returns>
        /// Возвращает время в миллисекундах, затрачиваемое на отправку запроса
        /// и получение соответствующего сообщения ответа.
        /// </returns>
        /// <value>
        /// <c>-1</c> - устройство не ответило в течение заданного интервала времени <see cref="Timeout"/>
        /// либо выполнение было прервано пользователем или статус ответа содержал ошибку.
        /// </value>
        public async Task<long> PingAsync(Address target)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var queryVersion = new NmsRead(target, (int)StdNms.SoftwareVersion);
            var sw = new Stopwatch();
            try
            {
                sw.Start();
                await WaitForNmsResponseAsync(queryVersion, Timeout).ConfigureAwait(false);
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
        /// <param name="target">Адрес устройства, с которого необходимо получить значение переменной.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="attempts">Количество попыток.</param>
        /// <returns><see cref="Task{TResult}"/> представляющий асинхронную операцию чтения.</returns>
        /// <seealso cref="WaitForNmsResponseAsync{TMessage}"/>
        public async Task<object> ReadValueAsync(Address target, int id, int attempts = 1)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            Contract.Requires(attempts > 0);
            var query = new NmsRead(target, id);
            var response = await WaitForNmsResponseAsync(query, Timeout, attempts);
            return response.Value;
        }

        /// <summary>
        /// Асинхронное блочное чтение множества преременных.
        /// </summary>
        /// <param name="progress">Предоставляет интерфейс для сообщений о получении значения переменной.</param>
        /// <param name="target">Адрес устойства.</param>
        /// <param name="ids">Список идентификаторов переменных.</param>
        /// <returns>Аснихронная задача</returns>
        /// <seealso cref="ReadProgressInfo"/>
        public async Task ReadManyValuesAsync(IProgress<ReadProgressInfo> progress, Address target, params int[] ids)
        {
            Contract.Requires(progress != null);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            foreach (var splitIds in ids.Split(NibusDatagram.MaxDataLength/3).Select(splitIds => splitIds.ToArray()))
            {
                NmsMessage lastMessage;
                IncomingMessages.TryReceive(null, out lastMessage);
                await OutgoingMessages.SendAsync(new NmsReadMany(target, splitIds));
                var tasks = from id in splitIds
                            select GetNmsReadResponseAsync(lastMessage, target, id)
                                .ContinueWith(task => progress.Report(new ReadProgressInfo(target, id, task)));
                try
                {
                    await TaskEx.WhenAll(tasks.ToArray());
                }
                catch (Exception e)
                {
                    Logger.ErrorException("Read failed", e);
                }
            }
        }

        #endregion

        #region Сервис NmsServiceType.Write

        /// <summary>
        /// Изменить значение переменной без подтверждения успеха внесения изменений.
        /// </summary>
        /// <param name="target">Адрес устройства, на котором требуется изменить значение переменной.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="valueType">Тип значения.</param>
        /// <param name="value">Записываемое значение.</param>
        public void WriteValue(Address target, int id, NmsValueType valueType, object value)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(value != null);
            var write = new NmsWrite(target, id, valueType, value, false);
            OutgoingMessages.Post(write);
        }

        /// <summary>
        /// Асинхронно изменить значение переменной без подтверждения успеха внесения изменений.
        /// </summary>
        /// <param name="target">Адрес устройства, на котором требуется изменить значение переменной.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="valueType">Тип значения.</param>
        /// <param name="value">Записываемое значение.</param>
        public async Task WriteValueAsync(Address target, int id, NmsValueType valueType, object value)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(value != null);
            var write = new NmsWrite(target, id, valueType, value, false);
            await OutgoingMessages.SendAsync(write);
        }

        /// <summary>
        /// Асинхронно изменить значение переменной с подтверждением успеха
        /// внесения изменений за <paramref name="attempts"/> попыток.
        /// </summary>
        /// <param name="target">Адрес устройства, на котором требуется изменить значение переменной.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="valueType">Тип значения.</param>
        /// <param name="value">Записываемое значение.</param>
        /// <param name="attempts">Количество попыток.</param>
        /// <returns>
        ///  <see cref="Task"/> асинхронная операция.
        /// </returns>
        public async Task WriteValueConfirmedAsync(
            Address target, int id, NmsValueType valueType, object value, int attempts = 1)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            Contract.Requires(value != null);
            Contract.Requires(attempts > 0);
            var write = new NmsWrite(target, id, valueType, value);
            await WaitForNmsResponseAsync(write, Timeout, attempts);
        }

        #endregion

        #region Сервис NmsServiceType.Reset

        public void ResetDevice(Address target)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type != AddressType.Empty);
            var reset = new NmsReset(Address.Empty, target, 0, false);
            OutgoingMessages.Post(reset);
        }

        public async Task ResetDeviceComfirmed(Address target)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var reset = new NmsReset(Address.Empty, target, 0);
            await WaitForNmsResponseAsync(reset, Timeout);
        }

        #endregion

        #region NmsServiceType.ExecuteProgramInvocation

        public void ExecuteProgram(Address target, int id, params Tuple<NmsValueType, object>[] args)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type != AddressType.Empty);
            var execute = new NmsExecuteProgramInvocation(Address.Empty, target, id, false, args);
            OutgoingMessages.Post(execute);
        }

        public async Task ExecuteProgramConfirmedAsync(
            Address target, int id, params Tuple<NmsValueType, object>[] args)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var execute = new NmsExecuteProgramInvocation(Address.Empty, target, id, true, args);
            await WaitForNmsResponseAsync(execute, Timeout);
        }

        #endregion

        #region Сигнализация событий

        public void FireEventNotification(int id)
        {
            Contract.Requires(!IsDisposed);
            var @event = new NmsEventNotification(Address.Empty, id);
            OutgoingMessages.Post(@event);
        }

        public void FireAckEventNotification(int id)
        {
            Contract.Requires(!IsDisposed);
            var @event = new NmsAckEventNotification(Address.Empty, id);
            OutgoingMessages.Post(@event);
        }

        public void EnableEventMonitoring(Address target, int id, bool isEventEnabled)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var msg = new NmsAlterEventConditionMonitoring(Address.Empty, target, id, isEventEnabled, false);
            OutgoingMessages.Post(msg);
        }

        public async Task EnableEventMonitoringConfirmedAsync(Address target, int id, bool isEventEnabled)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var msg = new NmsAlterEventConditionMonitoring(Address.Empty, target, id, isEventEnabled);
            await WaitForNmsResponseAsync(msg, Timeout);
        }

        #endregion

        #region Upload/Download сервис

        public async Task<byte[]> UploadDomainAsync(
            Address target,
            string domain,
            uint offset = 0,
            uint cbSize = 0,
            NibusOptions options = null)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            Contract.Requires(domain != null);
            Contract.Requires(0 < domain.Length && domain.Length <= 8);

            if (options == null)
            {
                options = new NibusOptions { Attempts = 3 };
            }

            var query = new NmsRequestDomainUpload(Address.Empty, target, domain);
            var response = await WaitForNmsResponseAsync(query, options);
            if (cbSize > response.DomainSize - offset)
            {
                throw new ArgumentOutOfRangeException(
                    "cbSize",
                    cbSize,
                    String.Format("Address space overflow detected. Domain size is {0} bytes.", response.DomainSize));
            }

            if (cbSize == 0)
            {
                cbSize = response.DomainSize - offset;
            }

            if (options.Progress != null)
            {
                options.Progress.Report((int)cbSize);
            }

            var initiation = new NmsInitiateUploadSequence(Address.Empty, target, response.Id);
            await WaitForNmsResponseAsync(initiation, UploadDownloadTimeout, attempts);

            var uploaded = new List<byte>((int)cbSize);
            while (uploaded.Count < cbSize)
            {
                var rest = cbSize - uploaded.Count;
                var upload = new NmsUploadSegment(
                    Address.Empty, target, response.Id, offset + (uint)uploaded.Count, rest > 58 ? (byte)58 : (byte)rest);
                var respUpload = await WaitForNmsResponseAsync(upload, UploadDownloadTimeout, attempts);
                if (respUpload.Offset != uploaded.Count + offset)
                {
                    if (Logger.IsDebugEnabled)
                    {
                        Logger.Debug(
                            "Invalid offset, resp.ofs = {0}, but = {1}", respUpload.Offset, uploaded.Count + offset);
                    }
                    continue;
                }

                uploaded.AddRange(respUpload.Segment);
                if (progress != null)
                {
                    progress.Report(uploaded.Count);
                }
            }

            return uploaded.ToArray();
        }

        public async Task DownloadDomainAsync(Address target, string domain, byte[] data, uint offset, int attempts = 3, CancellationToken token = null)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            Contract.Requires(domain != null);
            Contract.Requires(0 < domain.Length && domain.Length <= 8);

            var query = new NmsRequestDomainDownload(Address.Empty, target, domain);
            var response = await WaitForNmsResponseAsync(query, Timeout, attempts);
            if (data.Length + offset > response.DomainSize)
            {
                throw new ArgumentOutOfRangeException(
                    "data",
                    data.Length + offset,
                    String.Format("Address space overflow detected. Domain size is {0} bytes.", response.DomainSize));
            }

            var initiation = new NmsInitiateDownloadSequence(Address.Empty, target, response.Id);
            await WaitForNmsResponseAsync(initiation, UploadDownloadTimeout, attempts);
            foreach (var split in data.Split(16).Select(s => s.ToArray()))
            {
                var download = new NmsDownloadSegment(
                    Address.Empty, target, response.Id, offset, split, !response.IsFastDownload);
                if (response.IsFastDownload)
                {
                    await OutgoingMessages.SendAsync(download);
                }
                else
                {
                    await WaitForNmsResponseAsync(download, UploadDownloadTimeout, attempts);
                }

                offset += (uint)split.Length;
            }
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
        /// <param name="timeout">Таймаут</param>
        /// <param name="attempts">Количество попыток.</param>
        /// <returns>
        ///  <see cref="Task{TResult}"/> асинхронная операция - ответное сообщение.
        /// </returns>
        /// <exception cref="AggregateException">Призошла ошибка в асинхронном коде. См. <see cref="Exception.InnerException"/></exception>
        /// <exception cref="TimeoutException">Ошибка по таймауту.</exception>
        /// <exception cref="TaskCanceledException">Операция прервана пользователем.</exception>
        /// <exception cref="NibusResponseException">Ошибка NiBUS.</exception>
        public async Task<TMessage> WaitForNmsResponseAsync<TMessage>(
            TMessage query, TimeSpan timeout, int attempts = 1) where TMessage : NmsMessage
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(attempts > 0);
            Contract.Requires(
                query.Datagram.Destanation.Type == AddressType.Hardware
                || query.Datagram.Destanation.Type == AddressType.Net);

            // Последнее сообщение в BroadcastBlock всегда остается! Незабываем его фильтровать.
            NmsMessage lastMessage;
            IncomingMessages.TryReceive(null, out lastMessage);

            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            using (IncomingMessages.LinkTo(
                wob,
                m => !ReferenceEquals(lastMessage, m) && m.IsResponse && m.ServiceType == query.ServiceType
                     && (query.Id == 0 || m.Id == query.Id) && m.Datagram.Source == query.Datagram.Destanation))
            {
                for (var i = 0; i < attempts; i++)
                {
                    await OutgoingMessages.SendAsync(query);
                    try
                    {
                        var response = (TMessage)await wob.ReceiveAsync(timeout, _cts.Token).ConfigureAwait(false);
                        if (response.ErrorCode != 0)
                        {
                            throw new NibusResponseException(response.ErrorCode);
                        }

                        return response;
                    }
                    catch (TimeoutException)
                    {
                        Logger.Debug("Timeout {0}", i + 1);
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

        private async Task<NmsRead> GetNmsReadResponseAsync(NmsMessage lastMessage, Address source, int id)
        {
            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            using (IncomingMessages.LinkTo(
                wob,
                m => !ReferenceEquals(lastMessage, m) && m.IsResponse && m.ServiceType == NmsServiceType.Read
                     && m.Id == id && m.Datagram.Source == source))
            {
                var response = (NmsRead)await wob.ReceiveAsync(Timeout, _cts.Token).ConfigureAwait(false);
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