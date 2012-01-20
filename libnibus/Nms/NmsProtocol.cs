//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsProtocol.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics;
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
        
        /// <summary>
        /// Сбрасывает входящий канал от последнего сохраненного сообщения (особенность <see cref="BroadcastBlock{T}"/>).
        /// </summary>
        public Action ResetIncoming;

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Асинхронное чтение переменной.
        /// </summary>
        /// <param name="destanation">Адрес устройства, с которого необходимо получить значение переменной.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <returns><see cref="Task{TResult}"/> представляющий асинхронную операцию чтения.</returns>
        /// <exception cref="AggregateException">Призошла ошибка в асинхронном коде. См. <see cref="Exception.InnerException"/></exception>
        /// <exception cref="TimeoutException">Ошибка по таймауту.</exception>
        /// <exception cref="TaskCanceledException">Операция прервана пользователем.</exception>
        /// <exception cref="NibusException">Ошибка NiBUS.</exception>
        public async Task<object> ReadValueAsync(Address destanation, int id)
        {
            var query = new NmsRead(destanation, id);
            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            ResetIncoming();
            using (IncomingMessages.LinkTo(wob, m => m.IsResponce && m.ServiceType == NmsServiceType.Read && m.Id == id))
            {
                OutgoingMessages.Post(query);
                var responce = (NmsRead)await wob.ReceiveAsync(Timeout, _cts.Token);
                if (responce.ErrorCode != 0)
                {
                    throw new NibusException(responce.ErrorCode);
                }

                return responce.Value;
            }
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

        #endregion //Implementations

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
            }
        }

        #endregion

    }
}