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
        /// <summary>
        /// Минмальное время ожидания отклика от устройства для операций загрузки/выгрузки массивов данных.
        /// </summary>
        public static readonly TimeSpan MinUploadDounloadTimeout = TimeSpan.FromSeconds(10);
        private readonly NibusOptions _defaultOptions;

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
            _defaultOptions = new NibusOptions { Token = _cts.Token };

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
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <returns>
        /// Возвращает время в миллисекундах, затрачиваемое на отправку запроса
        /// и получение соответствующего сообщения ответа.
        /// </returns>
        /// <value>
        /// <c>-1</c> - устройство не ответило в течение заданного интервала времени <see cref="NibusOptions.Timeout"/>
        /// либо выполнение было прервано пользователем или статус ответа содержал ошибку.
        /// </value>
        public long Ping(Address target, NibusOptions options = null)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            return PingAsync(target, options).Result;
        }

        /// <summary>
        /// Асинхронная проверка доступности устройства.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <returns>
        /// Возвращает время в миллисекундах, затрачиваемое на отправку запроса
        /// и получение соответствующего сообщения ответа.
        /// </returns>
        /// <value>
        ///   <c>-1</c> - устройство не ответило в течение заданного интервала времени <see cref="NibusOptions.Timeout"/>
        /// либо выполнение было прервано пользователем или статус ответа содержал ошибку.
        ///   </value>
        public async Task<long> PingAsync(Address target, NibusOptions options = null)
        {
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(target != null);
            //Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var queryVersion = new NmsRead(target, (int)StdNms.SoftwareVersion);
            var sw = new Stopwatch();
            try
            {
                sw.Start();
                await WaitForNmsResponseAsync(queryVersion, options).ConfigureAwait(false);
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
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <returns><see cref="Task{TResult}"/> представляющий асинхронную операцию чтения.</returns>
        /// <seealso cref="WaitForNmsResponseAsync{TMessage}"/>
        public async Task<object> ReadValueAsync(Address target, int id, NibusOptions options = null)
        {
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(target != null);
            //Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var query = new NmsRead(target, id);
            var response = await WaitForNmsResponseAsync(query, options);
            return response.Value;
        }

        /// <summary>
        /// Асинхронное пакетное чтение множества преременных.
        /// </summary>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <param name="target">Адрес устойства.</param>
        /// <param name="ids">Список идентификаторов переменных.</param>
        /// <returns>
        /// Аснихронная задача
        /// </returns>
        /// <seealso cref="ReadProgressInfo"/>
        public async Task ReadManyValuesAsync(NibusOptions options, Address target, params int[] ids)
        {
            //Contract.Requires(options != null);
            //Contract.Requires(options.Progress != null);
            //Contract.Requires(target != null);
            //Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            foreach (var splitIds in ids.Split(NmsMessage.NmsMaxDataLength/3).Select(splitIds => splitIds.ToArray()))
            {
                NmsMessage lastMessage;
                IncomingMessages.TryReceive(null, out lastMessage);
                await OutgoingMessages.SendAsync(new NmsReadMany(target, splitIds));
                var tasks = from id in splitIds
                            select GetNmsReadResponseAsync(lastMessage, target, id, options)
                                .ContinueWith(
                                    task =>
                                        {
                                            var rp = new ReadProgressInfo(target, id, task);
                                            if (Logger.IsTraceEnabled)
                                                Logger.Trace("Received id={0}, Exc = {1}", rp.Id, rp.Exception);
                                            options.Progress.Report(rp);
                                        });
                try
                {
                    await Task.WhenAll(tasks.ToArray());
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Read failed");
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
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(target != null);
            //Contract.Requires(value != null);
            var write = new NmsWrite(target, id, valueType, value, false);
            await OutgoingMessages.SendAsync(write);
        }

        /// <summary>
        /// Асинхронно изменить значение переменной с подтверждением успеха внесения изменений.
        /// </summary>
        /// <param name="target">Адрес устройства, на котором требуется изменить значение переменной.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="valueType">Тип значения.</param>
        /// <param name="value">Записываемое значение.</param>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <returns>
        ///  <see cref="Task"/> асинхронная операция.
        /// </returns>
        public async Task WriteValueConfirmedAsync(
            Address target,
            int id,
            NmsValueType valueType,
            object value,
            NibusOptions options = null)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            Contract.Requires(value != null);
            var write = new NmsWrite(target, id, valueType, value);
            await WaitForNmsResponseAsync(write, options);
        }

        #endregion

        #region Сервис выполнения подпрограммы

        /// <summary>
        /// Выполнить подпрограмму на устройстве без подтверждения успеха.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="id">Идентификатор подпрограммы.</param>
        /// <param name="args">Аргументы, передаваемые в подпрограмму в виде массива пар,
        /// где первый элемент пары содержит тип параметра, второй - значение.</param>
        public void ExecuteProgram(Address target, int id, params Tuple<NmsValueType, object>[] args)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type != AddressType.Empty);
            var execute = new NmsExecuteProgramInvocation(Address.Empty, target, id, false, args);
            OutgoingMessages.Post(execute);
        }

        /// <summary>
        /// Асинхронно выполнить подпрограмму на устройстве с подтверждением успеха операции.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="id">Идентификатор подпрограммы.</param>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <param name="args">Аргументы, передаваемые в подпрограмму в виде массива пар,
        /// где первый элемент пары содержит тип параметра, второй - значение.</param>
        /// <returns><see cref="Task"/> - асинхронная операция.</returns>
        public async Task ExecuteProgramConfirmedAsync(
            Address target,
            int id,
            NibusOptions options = null,
            params Tuple<NmsValueType, object>[] args)
        {
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(target != null);
            //Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var execute = new NmsExecuteProgramInvocation(Address.Empty, target, id, true, args);
            await WaitForNmsResponseAsync(execute, options);
        }

        /// <summary>
        /// Начать выполнение подпрограммы на устройстве без подтверждения успеха.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="id">Идентификатор подпрограммы.</param>
        /// <param name="args">Аргументы, передаваемые в подпрограмму в виде массива пар,
        /// где первый элемент пары содержит тип параметра, второй - значение.</param>
        public void StartProgram(Address target, int id, params Tuple<NmsValueType, object>[] args)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type != AddressType.Empty);
            var start = new NmsStartProgramInvocation(Address.Empty, target, id, false, args);
            OutgoingMessages.Post(start);
        }

        /// <summary>
        /// Асинхронно начать выполнять подпрограмму на устройстве с подтверждением успеха операции.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="id">Идентификатор подпрограммы.</param>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <param name="args">Аргументы, передаваемые в подпрограмму в виде массива пар,
        /// где первый элемент пары содержит тип параметра, второй - значение.</param>
        /// <returns><see cref="Task"/> - асинхронная операция.</returns>
        public async Task StartProgramConfirmedAsync(
            Address target,
            int id,
            NibusOptions options = null,
            params Tuple<NmsValueType, object>[] args)
        {
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(target != null);
            //Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var start = new NmsStartProgramInvocation(Address.Empty, target, id, true, args);
            await WaitForNmsResponseAsync(start, options);
        }

        /// <summary>
        /// Прекратить выполнение подпрограммы на устройстве без подтверждения успеха.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="id">Идентификатор подпрограммы.</param>
        public void StopProgram(Address target, int id)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type != AddressType.Empty);
            Contract.Requires(id > 0); 
            var stop = new NmsStop(Address.Empty, target, id, false);
            OutgoingMessages.Post(stop);
        }

        /// <summary>
        /// Асинхронно прекратить выполнять подпрограмму на устройстве с подтверждением успеха операции.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="id">Идентификатор подпрограммы.</param>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <returns><see cref="Task"/> - асинхронная операция.</returns>
        public async Task StopProgramComfirmedAsync(Address target, int id, NibusOptions options = null)
        {
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(target != null);
            //Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var stop = new NmsStop(Address.Empty, target, id);
            await WaitForNmsResponseAsync(stop, options);
        }

        /// <summary>
        /// Продолжить выполнение подпрограммы на устройстве без подтверждения успеха.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="id">Идентификатор подпрограммы.</param>
        public void ResumeProgram(Address target, int id)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type != AddressType.Empty);
            Contract.Requires(id > 0); 
            var resume = new NmsResume(Address.Empty, target, id, false);
            OutgoingMessages.Post(resume);
        }

        /// <summary>
        /// Асинхронно продолжить выполнять подпрограмму на устройстве с подтверждением успеха операции.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="id">Идентификатор подпрограммы.</param>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <returns><see cref="Task"/> - асинхронная операция.</returns>
        public async Task ResumeProgramComfirmedAsync(Address target, int id, NibusOptions options = null)
        {
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(target != null);
            //Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var resume = new NmsResume(Address.Empty, target, id);
            await WaitForNmsResponseAsync(resume, options);
        }

        /// <summary>
        /// Перегрузить устройство без подтверждения успеха.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        public void ResetDevice(Address target)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type != AddressType.Empty);
            var reset = new NmsReset(Address.Empty, target, false);
            OutgoingMessages.Post(reset);
        }

        /// <summary>
        /// Асинхронно перегрузить устройство с подтверждением успеха.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <returns><see cref="Task"/> - асинхронная операция.</returns>
        public async Task ResetDeviceComfirmedAsync(Address target, NibusOptions options = null)
        {
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(target != null);
            //Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var reset = new NmsReset(Address.Empty, target);
            await WaitForNmsResponseAsync(reset, options);
        }

        /// <summary>
        /// Остановить устройство без подтверждения успеха.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        public void ShutdownDevice(Address target)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type != AddressType.Empty);
            var shutdown = new NmsShutdown(Address.Empty, target, false);
            OutgoingMessages.Post(shutdown);
        }

        /// <summary>
        /// Асинхронно остановить устройство с подтверждением успеха.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <returns><see cref="Task"/> - асинхронная операция.</returns>
        public async Task ShutdownDeviceComfirmedAsync(Address target, NibusOptions options = null)
        {
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(target != null);
            //Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var shutdown = new NmsShutdown(Address.Empty, target);
            await WaitForNmsResponseAsync(shutdown, options);
        }

        #endregion

        #region Сигнализация событий

        /// <summary>
        /// Сигнализировать о событии.
        /// </summary>
        /// <param name="id">Идентификатор события.</param>
        /// <seealso cref="Events"/>
        public void FireEventNotification(int id)
        {
            Contract.Requires(!IsDisposed);
            var @event = new NmsEventNotification(Address.Empty, id);
            OutgoingMessages.Post(@event);
        }

        /// <summary>
        /// ?Сигнализировать о подтверждении события?.
        /// </summary>
        /// <param name="id">Идентификатор события.</param>
        /// <seealso cref="Events"/>
        public void FireAckEventNotification(int id)
        {
            Contract.Requires(!IsDisposed);
            var @event = new NmsAckEventNotification(Address.Empty, id);
            OutgoingMessages.Post(@event);
        }

        /// <summary>
        /// Разрешить сигнализацию о событии без подтверждения о приеме.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="id">Идентификатор события.</param>
        /// <param name="isEventEnabled"><c>true</c> - разрешить сигнализацию о событии, иначе - <c>false</c>.</param>
        public void EnableEventMonitoring(Address target, int id, bool isEventEnabled)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var msg = new NmsAlterEventConditionMonitoring(Address.Empty, target, id, isEventEnabled, false);
            OutgoingMessages.Post(msg);
        }

        /// <summary>
        /// Разрешить сигнализацию о событии с подтверждением о приеме.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="id">Идентификатор события.</param>
        /// <param name="isEventEnabled"><c>true</c> - разрешить сигнализацию о событии, иначе - <c>false</c>.</param>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <returns><see cref="Task"/> - асинхронная операция.</returns>
        public async Task EnableEventMonitoringConfirmedAsync(
            Address target,
            int id,
            bool isEventEnabled,
            NibusOptions options = null)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(target != null);
            Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            var msg = new NmsAlterEventConditionMonitoring(Address.Empty, target, id, isEventEnabled);
            await WaitForNmsResponseAsync(msg, options);
        }

        #endregion

        #region Upload/Download сервис

        /// <summary>
        /// Асинхронная выгрузка массива данных с устройства.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="domain">Домен.</param>
        /// <param name="offset">Смещение в домене.</param>
        /// <param name="cbSize">Количество байт.</param>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <returns>
        ///   <see cref="Task"/> - асинхронная операция с данными домена.
        /// </returns>
        public async Task<byte[]> UploadDomainAsync(
            Address target,
            string domain,
            uint offset = 0,
            uint cbSize = 0,
            NibusOptions options = null)
        {
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(target != null);
            //Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            //Contract.Requires(domain != null);
            //Contract.Requires(0 < domain.Length && domain.Length <= 8);

            options = GetDefaultOrClone(options);
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

            if (options != null && options.Progress != null)
            {
                options.Progress.Report((int)cbSize);
            }

            var uploadOptions = new NibusOptions(options);
            uploadOptions.Attempts = Math.Max(uploadOptions.Attempts, 3);
            if (uploadOptions.Timeout < MinUploadDounloadTimeout)
            {
                uploadOptions.Timeout = MinUploadDounloadTimeout;
            }

            var initiation = new NmsInitiateUploadSequence(Address.Empty, target, response.Id);
            await WaitForNmsResponseAsync(initiation, uploadOptions);

            var uploaded = new List<byte>((int)cbSize);
            while (uploaded.Count < cbSize)
            {
                uploadOptions.Token.ThrowIfCancellationRequested();
                var rest = cbSize - uploaded.Count;
                var upload = new NmsUploadSegment(
                    Address.Empty, target, response.Id, offset + (uint)uploaded.Count, rest > 58 ? (byte)58 : (byte)rest);
                var respUpload = await WaitForNmsResponseAsync(upload, uploadOptions);
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
                if (uploadOptions.Progress != null)
                {
                    uploadOptions.Progress.Report(uploaded.Count);
                }
            }

            return uploaded.ToArray();
        }

        /// <summary>
        /// Асинхронная загрузка массива данных в устройство.
        /// </summary>
        /// <param name="target">Адрес устройства.</param>
        /// <param name="domain">Домен.</param>
        /// <param name="data">Данные.</param>
        /// <param name="offset">Смещение в домене.</param>
        /// <param name="options">Параметры NiBUS-операции.</param>
        /// <returns>
        ///   <see cref="Task"/> - асинхронная операция.
        /// </returns>
        public async Task DownloadDomainAsync(
            Address target,
            string domain,
            byte[] data,
            uint offset,
            NibusOptions options = null)
        {
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(target != null);
            //Contract.Requires(target.Type == AddressType.Hardware || target.Type == AddressType.Net);
            //Contract.Requires(domain != null);
            //Contract.Requires(0 < domain.Length && domain.Length <= 8);

            var query = new NmsRequestDomainDownload(Address.Empty, target, domain);
            var response = await WaitForNmsResponseAsync(query, options);
            if (data.Length + offset > response.DomainSize)
            {
                throw new ArgumentOutOfRangeException(
                    "data",
                    data.Length + offset,
                    String.Format("Address space overflow detected. Domain size is {0} bytes.", response.DomainSize));
            }

            var downloadOptions = new NibusOptions(options);
            downloadOptions.Attempts = Math.Max(downloadOptions.Attempts, 3);
            if (downloadOptions.Timeout < MinUploadDounloadTimeout)
            {
                downloadOptions.Timeout = MinUploadDounloadTimeout;
            }
            var initiation = new NmsInitiateDownloadSequence(Address.Empty, target, response.Id);
            await WaitForNmsResponseAsync(initiation, downloadOptions);
            foreach (var split in data.Split(16).Select(s => s.ToArray()))
            {
                downloadOptions.Token.ThrowIfCancellationRequested();
                var download = new NmsDownloadSegment(
                    Address.Empty, target, response.Id, offset, split, !response.IsFastDownload);
                if (response.IsFastDownload)
                {
                    await OutgoingMessages.SendAsync(download);
                }
                else
                {
                    await WaitForNmsResponseAsync(download, downloadOptions);
                }

                offset += (uint)split.Length;
            }
        }

        #endregion

        /// <summary>
        /// Отправляет NMS-сообщение <paramref name="query"/> и ожидает ответа.
        /// Асинхронная операция.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения, потомок NmsMessage.</typeparam>
        /// <param name="query">Сообщение-запрос.</param>
        /// <param name="options">Параметры асинхронной операции.</param>
        /// <returns>
        ///  <see cref="Task{TResult}"/> асинхронная операция - ответное сообщение.
        /// </returns>
        /// <exception cref="AggregateException">Призошла ошибка в асинхронном коде. См. <see cref="Exception.InnerException"/></exception>
        /// <exception cref="TimeoutException">Ошибка по таймауту.</exception>
        /// <exception cref="TaskCanceledException">Операция прервана пользователем.</exception>
        /// <exception cref="NibusResponseException">Ошибка NiBUS.</exception>
        public async Task<TMessage> WaitForNmsResponseAsync<TMessage>(TMessage query, NibusOptions options = null)
            where TMessage : NmsMessage
        {
            //Contract.Requires(!IsDisposed);
            //Contract.Requires(
            //    query.Datagram.Destanation.Type == AddressType.Hardware
            //    || query.Datagram.Destanation.Type == AddressType.Net);

            options = GetDefaultOrClone(options);

            // Последнее сообщение в BroadcastBlock всегда остается! Незабываем его фильтровать.
            NmsMessage lastMessage;
            IncomingMessages.TryReceive(null, out lastMessage);

            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            using (IncomingMessages.LinkTo(
                wob,
                m => !ReferenceEquals(lastMessage, m) && m.IsResponse && m.ServiceType == query.ServiceType
                     && (query.Id == 0 || m.Id == query.Id) && (query.Datagram.Destanation == Address.Empty || m.Datagram.Source == query.Datagram.Destanation)))
            {
                for (var i = 0; i < options.Attempts; i++)
                {
                    await OutgoingMessages.SendAsync(query);
                    try
                    {
                        var response =
                            (TMessage)await wob.ReceiveAsync(options.Timeout, options.Token).ConfigureAwait(false);
                        if (response.ErrorCode != 0)
                        {
                            throw new NibusResponseException(response.ErrorCode);
                        }

                        return response;
                    }
                    catch (TimeoutException)
                    {
                        Logger.Debug("Timeout {0}", i + 1);
                        if (i < options.Attempts - 1) continue;
                        throw;
                    }
                }
            }

            // Эта точка недостижима при attempts > 0!
            throw new InvalidOperationException();
        }

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
                        Logger.Warn("Unhandled NMS Information Report: id={0}", report.Id);
                    }
                }
                catch (Exception exception)
                {
                    Logger.Error(exception, "NMS Information Report error");
                    Debug.Fail("NMS Information Report error");
                }
            }
        }

        private async Task<NmsRead> GetNmsReadResponseAsync(
            NmsMessage lastMessage, Address target, int id, NibusOptions options)
        {
            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            using (IncomingMessages.LinkTo(
                wob,
                m => !ReferenceEquals(lastMessage, m) && m.IsResponse && m.ServiceType == NmsServiceType.Read
                     && m.Id == id && (target == Address.Empty || m.Datagram.Source == target)))
            {
                var response = (NmsRead)await wob.ReceiveAsync(options.Timeout, options.Token).ConfigureAwait(false);
                if (response.ErrorCode != 0)
                {
                    throw new NibusResponseException(response.ErrorCode);
                }

                return response;
            }
        }

        private NibusOptions GetDefaultOrClone(NibusOptions options)
        {
            return options == null
                       ? _defaultOptions
                       : new NibusOptions
                             {
                                 Attempts = options.Attempts,
                                 Progress = options.Progress,
                                 Timeout = options.Timeout,
                                 Token = options.Token
                             };
        }

        #endregion //Implementations

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
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

        /// <summary>
        /// Возвращает описание кодека.
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}