//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsShutdown.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    /// <summary>
    /// Класс-обертка для сообщиений о выключении устройства.
    /// </summary>
    public sealed class NmsShutdown : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsShutdown(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Assume(ServiceType == NmsServiceType.Shutdown);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsShutdown"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="destanation">Адрес приемника.</param>
        /// <param name="waitResponse"><c>true</c> - если требуется ожидание подтверждения.</param>
        public NmsShutdown(Address source, Address destanation, bool waitResponse = true)
        {
            Contract.Ensures(ServiceType == NmsServiceType.Shutdown);
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.Shutdown, waitResponse, 0, false, new byte[0]);
        }

        #endregion //Constructors

        #region Properties

        #endregion //Properties
    }
}