//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsAckEventNotification.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsAckEventNotification : NmsMessage
    {
        public NmsAckEventNotification(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.AckEventNotification);
        }

        public NmsAckEventNotification(Address source, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.AckEventNotification);
            Initialize(
                source,
                Address.Broadcast,
                PriorityType.Normal,
                NmsServiceType.AckEventNotification,
                false,
                id,
                false,
                new byte[0]);
        }
    }
}