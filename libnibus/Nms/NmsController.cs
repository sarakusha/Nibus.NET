//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsController.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

#endregion

namespace NataInfo.Nibus.Nms
{
    public class NmsController
    {
        #region Member Variables

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsController()
        {
        }

        #endregion //Constructors

        #region Properties

        public ISourceBlock<NmsMessage> IncomingMessages { get; set; }
        public ITargetBlock<NmsMessage> OutgoingMessages { get; set; }

        public TimeSpan Timeout { get; set; }

        #endregion //Properties

        #region Methods

        public async Task<object> ReadValue(Address destanation, int id)
        {
            var query = new NmsRead(destanation, id);
            OutgoingMessages.Post(query);
            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            //IncomingMessages.LinkTo()
            return null;
        }

        #endregion //Methods

        #region Implementations

        #endregion //Implementations
    }
}
