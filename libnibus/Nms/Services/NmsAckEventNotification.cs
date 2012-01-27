//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsAckEventNotification.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    /// <summary>
    /// Класс-обертка для сообщений сервиса <see cref="NmsServiceType.AckEventNotification"/>
    /// "Подтверждение события".
    /// </summary>
    public sealed class NmsAckEventNotification : NmsMessage
    {
        /// <summary>
        /// Конструктор создания NMS-сообщения из низлежащего сообщения <see cref="NibusDatagram"/>.
        /// </summary>
        /// <param name="datagram">Датаграмма.</param>
        /// <remarks>
        /// Минимальный размер длины данных <paramref name="datagram"/> должен быть не меньше размера
        /// заголовка <see cref="NmsMessage.NmsHeaderLength"/>.
        /// </remarks>
        /// <seealso cref="NmsMessage.CreateFrom"/>
        internal NmsAckEventNotification(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Ensures(ServiceType == NmsServiceType.AckEventNotification);
            Contract.Assume(ServiceType == NmsServiceType.AckEventNotification);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsAckEventNotification"/> class.
        /// </summary>
        /// <param name="source">Адрес источника сообщения.</param>
        /// <param name="id">Идентификатор события.</param>
        public NmsAckEventNotification(Address source, int id)
        {
            Contract.Requires(source != null);
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