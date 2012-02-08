//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsInitiateUploadSequence.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    /// <summary>
    /// Класс-обертка для сообщений о инициации выгрузки массива данных с устройства.
    /// </summary>
    public sealed class NmsInitiateUploadSequence : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsInitiateUploadSequence(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Assume(ServiceType == NmsServiceType.InitiateUploadSequence);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsInitiateUploadSequence"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="destanation">Адрес приемника.</param>
        /// <param name="id">Идентификатор домена.</param>
        public NmsInitiateUploadSequence(Address source, Address destanation, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.InitiateUploadSequence);
            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.InitiateUploadSequence,
                true,
                id,
                false,
                new byte[0]);
        }

        #endregion //Constructors

        #region Properties

        #endregion //Properties
    }
}