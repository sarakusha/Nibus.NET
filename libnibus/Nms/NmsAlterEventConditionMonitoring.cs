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
    /// Класс-обертка для сообщений сервиса <see cref="NmsServiceType.AckEventNotification"/>
    /// "Запрет/разрешение сигнализации события".
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
            Contract.Ensures(ServiceType == NmsServiceType.AlterEventConditionMonitoring);
            if (IsResponse && datagram.Data.Count < NmsMaxDataLength + 1)
            {
                throw new InvalidNibusDatagram("Invalid NMS message length");
            }

            Contract.Assume(ServiceType == NmsServiceType.AlterEventConditionMonitoring);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsAlterEventConditionMonitoring"/> class.
        /// </summary>
        /// <param name="source">Адрес источника сообщения.</param>
        /// <param name="destanation">Адрес получателя сообщения.</param>
        /// <param name="id">Идентификатор события.</param>
        /// <param name="isEventEnabled">Если <c>true</c> событие разрешено.</param>
        public NmsAlterEventConditionMonitoring(Address source, Address destanation, int id, bool isEventEnabled)
        {
            Contract.Requires(source != null);
            Contract.Requires(destanation != null);
            Contract.Ensures(!IsResponse);
            Contract.Ensures(ServiceType == NmsServiceType.AlterEventConditionMonitoring);
            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.AlterEventConditionMonitoring,
                true,
                id,
                isEventEnabled,
                new byte[0]);
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
        #endregion //Properties
    }
}