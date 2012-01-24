//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsReset.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsReset : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsReset(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Assume(ServiceType == NmsServiceType.Reset);
        }

        public NmsReset(Address source, Address destanation, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.Reset);
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.Reset, true, id, false, new byte[0]);
        }

        #endregion //Constructors

        #region Properties

        #endregion //Properties
    }
}