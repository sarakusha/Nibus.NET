//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsController.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

#endregion

namespace NataInfo.Nibus.Nms
{
    // TODO: INibusEndpoint
    public class NmsController : INibusEndpoint<NmsMessage>
    {
        #region Member Variables

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsController()
        {
            Timeout = TimeSpan.FromSeconds(1);
        }

        #endregion //Constructors

        #region Properties

        public TimeSpan Timeout { get; set; }
        public IReceivableSourceBlock<NmsMessage> IncomingMessages { get; set; }
        public ITargetBlock<NmsMessage> OutgoingMessages { get; set; }

        #endregion //Properties

        #region Methods

        public async Task<object> ReadValueAsync(Address destanation, int id)
        {
            var query = new NmsRead(destanation, id);
            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            using (IncomingMessages.LinkTo(wob, m => m.IsResponce && m.ServiceType == NmsServiceType.Read && m.Id == id))
            {
                OutgoingMessages.Post(query);
                var responce = (NmsRead)await wob.ReceiveAsync(Timeout);
                return responce.Value;
            }
        }

        #endregion //Methods

        #region Implementations

        #endregion //Implementations
    }
}