//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsReset.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    /// <summary>
    /// Класс-обертка для сообщений о перезагрузке устройства.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsReset"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="destanation">Адрес приемника.</param>
        /// <param name="waitResponse"><c>true</c> - если требуется ожидание подтверждения.</param>
        public NmsReset(Address source, Address destanation, bool waitResponse = true)
        {
            Contract.Ensures(ServiceType == NmsServiceType.Reset);
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.Reset, waitResponse, 0, false, new byte[0]);
        }

        #endregion //Constructors
    }
}