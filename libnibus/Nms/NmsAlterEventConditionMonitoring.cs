//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsAlterEventConditionMonitoring.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    /// <summary>
    /// Запрет/разрешение сигнализации события.
    /// </summary>
    public sealed class NmsAlterEventConditionMonitoring : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsAlterEventConditionMonitoring(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            if (IsResponse && datagram.Data.Count < NmsMaxDataLength + 1)
            {
                throw new ArgumentException();
            }
            Contract.Ensures(ServiceType == NmsServiceType.AlterEventConditionMonitoring);
            Contract.Assume(ServiceType == NmsServiceType.AlterEventConditionMonitoring);
        }

        public NmsAlterEventConditionMonitoring(Address source, Address destanation, int id, bool isEventEnabled)
        {
            Contract.Requires(source != null);
            Contract.Requires(destanation != null);
            Contract.Ensures(ServiceType == NmsServiceType.AlterEventConditionMonitoring);
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.AlterEventConditionMonitoring, true, id,
                       isEventEnabled, new byte[0]);
        }

        #endregion //Constructors

        #region Properties

        public bool IsEventEnabled
        {
            get
            {
                Contract.Requires(!IsResponse);
                return (Datagram.Data[2] & 0x40) != 0;
            }
        }

        public int ErrorCode
        {
            get
            {
                Contract.Requires(IsResponse);
                return Datagram.Data[NmsHeaderLength + 0];
            }
        }

        #endregion //Properties
    }
}