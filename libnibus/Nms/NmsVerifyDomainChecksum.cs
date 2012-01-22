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

namespace NataInfo.Nibus.Nms
{
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

        public uint Offset
        {
            get
            {
                Contract.Requires(!IsResponse);
                return BitConverter.ToUInt32(Datagram.Data.ToArray(), NmsHeaderLength + 0);
            }
        }

        public uint Size
        {
            get
            {
                Contract.Requires(!IsResponse);
                return BitConverter.ToUInt32(Datagram.Data.ToArray(), NmsHeaderLength + 4);
            }
        }

        public ushort Checksum
        {
            get
            {
                Contract.Requires(!IsResponse);
                return BitConverter.ToUInt16(Datagram.Data.ToArray(), 8);
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