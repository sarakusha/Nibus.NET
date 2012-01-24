//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsStop.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsStop : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsStop(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Assume(ServiceType == NmsServiceType.Stop);
        }

        public NmsStop(Address source, Address destanation, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.Stop);
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.Stop, true, id, false, new byte[0]);
        }

        #endregion //Constructors

        #region Properties

        #endregion //Properties
    }
}