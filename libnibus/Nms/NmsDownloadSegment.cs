//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsDownloadSegment.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsDownloadSegment : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsDownloadSegment(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.DownloadSegment);
        }

        public NmsDownloadSegment(Address source, Address destanation, int id, uint offset, byte[] segment)
        {
            Contract.Requires(segment.Length < NmsMaxDataLength - 4);
            Contract.Ensures(ServiceType == NmsServiceType.DownloadSegment);
            int segmentLength = Math.Min(segment.Length, NmsMaxDataLength - 4);
            var nmsData = new byte[4 + segmentLength];
            BitConverter.GetBytes(offset).CopyTo(nmsData, 0);
            Array.Copy(segment, 0, nmsData, 4, segmentLength);

            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.DownloadSegment,
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
                Contract.Requires(!IsResponce);
                return BitConverter.ToUInt32(Datagram.Data.ToArray(), NmsHeaderLength + 0);
            }
        }

        public int ErrorCode
        {
            get
            {
                Contract.Requires(IsResponce);
                return Datagram.Data[NmsHeaderLength + 0];
            }
        }

        public byte[] Segment
        {
            get
            {
                Contract.Requires(!IsResponce);
                return Datagram.Data.Skip(NmsHeaderLength + 4).Take(Length - 4).ToArray();
            }
        }

        #endregion //Properties
    }
}