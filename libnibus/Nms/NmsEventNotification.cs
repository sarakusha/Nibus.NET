//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsEventNotification.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    internal sealed class NmsEventNotification : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsEventNotification(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.EventNotification);
        }

        public NmsEventNotification(Address source, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.EventNotification);
            Initialize(
                source,
                Address.Broadcast,
                PriorityType.Normal,
                NmsServiceType.EventNotification,
                false,
                id,
                false,
                new byte[0]);
        }

        #endregion //Constructors
    }
}