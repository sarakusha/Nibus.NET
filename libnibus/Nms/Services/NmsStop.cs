//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsStop.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    /// <summary>
    /// Класс-обертка для сообщений о прекращении выполнения подпрограммы.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsStop"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="destanation">Адрес приемника.</param>
        /// <param name="id">Идентификатор подпрограммы.</param>
        /// <param name="waitResponse"><c>true</c> - если требуется ожидание подтверждения.</param>
        public NmsStop(Address source, Address destanation, int id, bool waitResponse = true)
        {
            Contract.Ensures(ServiceType == NmsServiceType.Stop);
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.Stop, waitResponse, id, false, new byte[0]);
        }

        #endregion //Constructors

        #region Properties

        #endregion //Properties
    }
}