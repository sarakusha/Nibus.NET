//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsUploadSegment.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    public sealed class NmsUploadSegment : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsUploadSegment(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Assume(ServiceType == NmsServiceType.UploadSegment);
        }

        public NmsUploadSegment(Address source, Address destanation, int id, uint offset, byte size)
        {
            Contract.Requires(size < NmsMaxDataLength - 5);
            Contract.Ensures(ServiceType == NmsServiceType.UploadSegment);
            var nmsData = new byte[5];
            BitConverter.GetBytes(offset).CopyTo(nmsData, 0);
            nmsData[4] = size;

            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.UploadSegment,
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
            get { return BitConverter.ToUInt32(Datagram.Data.ToArray(), NmsHeaderLength + (IsResponse ? 1 : 0)); }
        }

        /// <summary>
        /// Возвращает количество байт в сегменте.
        /// </summary>
        public int Size
        {
            get
            {
                Contract.Requires(!IsResponse);
                return Datagram.Data[NmsHeaderLength + 4];
            }
        }

        public byte[] Segment
        {
            get
            {
                Contract.Requires(IsResponse);
                return Datagram.Data.Skip(NmsHeaderLength).Take(Length - 5).ToArray();
            }
        }

        #endregion //Properties
    }
}