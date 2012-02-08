//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsVerifyDomainChecksum.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    /// <summary>
    /// Класс-обертка о проверке контрольной суммы домена.
    /// </summary>
    public sealed class NmsVerifyDomainChecksum : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsVerifyDomainChecksum(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Assume(ServiceType == NmsServiceType.VerifyDomainChecksum);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsVerifyDomainChecksum"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="destanation">Адрес приемника.</param>
        /// <param name="id">Идентификатор домена.</param>
        /// <param name="offset">Смещение в домене.</param>
        /// <param name="size">Размер.</param>
        /// <param name="checksum">Контрольная сумма.</param>
        public NmsVerifyDomainChecksum(
            Address source, Address destanation, int id, uint offset, uint size, ushort checksum)
        {
            Contract.Ensures(ServiceType == NmsServiceType.VerifyDomainChecksum);
            var nmsData = new byte[10];
            BitConverter.GetBytes(offset).CopyTo(nmsData, 0);
            BitConverter.GetBytes(size).CopyTo(nmsData, 4);
            BitConverter.GetBytes(checksum).CopyTo(nmsData, 8);
            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.VerifyDomainChecksum,
                true,
                id,
                false,
                nmsData);
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Возвращает смещение в домене.
        /// </summary>
        public uint Offset
        {
            get
            {
                Contract.Requires(!IsResponse);
                return BitConverter.ToUInt32(Datagram.Data.ToArray(), NmsHeaderLength + 0);
            }
        }

        /// <summary>
        /// Размер проверяемых данных.
        /// </summary>
        public uint Size
        {
            get
            {
                Contract.Requires(!IsResponse);
                return BitConverter.ToUInt32(Datagram.Data.ToArray(), NmsHeaderLength + 4);
            }
        }

        /// <summary>
        /// Возвращает контрольную сумму.
        /// </summary>
        public ushort Checksum
        {
            get
            {
                Contract.Requires(!IsResponse);
                return BitConverter.ToUInt16(Datagram.Data.ToArray(), 8);
            }
        }

        #endregion //Properties
    }
}