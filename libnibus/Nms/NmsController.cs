//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsController.cs
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
    public sealed class NmsController : INibusEndpoint<NmsMessage>
    {
        #region Member Variables

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly CancellationTokenSource _cts;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsController(IReceivableSourceBlock<NmsMessage> incoming, ITargetBlock<NmsMessage> outgoing)
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

        public event EventHandler<NmsInformationReportEventArgs> InformationReportReceived;

        #region Properties

        public TimeSpan Timeout { get; set; }
        public IReceivableSourceBlock<NmsMessage> IncomingMessages { get; private set; }
        public ITargetBlock<NmsMessage> OutgoingMessages { get; private set; }
        public Action ResetIncoming;

        #endregion //Properties

        #region Methods

        public async Task<object> ReadValueAsync(Address destanation, int id)
        {
            var query = new NmsRead(destanation, id);
            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            ResetIncoming();
            using (IncomingMessages.LinkTo(wob, m => m.IsResponce && m.ServiceType == NmsServiceType.Read && m.Id == id))
            {
                OutgoingMessages.Post(query);
                var responce = (NmsRead)await wob.ReceiveAsync(Timeout, _cts.Token);
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