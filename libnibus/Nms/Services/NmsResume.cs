//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsResume.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    /// <summary>
    /// Класс-обертка для сообщений о возобновлении выполнения подпрограммы.
    /// </summary>
    public sealed class NmsResume : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsResume(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Assume(ServiceType == NmsServiceType.Resume);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsResume"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="destanation">Адрес приемника.</param>
        /// <param name="id">Идентификатор подпрограммы.</param>
        /// <param name="waitResponse"><c>true</c> - если требуется ожидание подтверждения.</param>
        public NmsResume(Address source, Address destanation, int id, bool waitResponse = true)
        {
            Contract.Ensures(ServiceType == NmsServiceType.Resume);
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.Resume, waitResponse, id, false, new byte[0]);
        }

        #endregion //Constructors

        #region Properties

        #endregion //Properties
    }
}