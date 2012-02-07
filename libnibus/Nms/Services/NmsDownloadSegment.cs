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

namespace NataInfo.Nibus.Nms.Services
{
    /// <summary>
    /// Класс-обертка для сообщений сервиса <see cref="NmsServiceType.DownloadSegment"/>.
    /// </summary>
    public sealed class NmsDownloadSegment : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// Конструктор создания сообщения из датаграммы NiBUS.
        /// </summary>
        /// <param name="datagram">Датаграмма.</param>
        internal NmsDownloadSegment(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Ensures(ServiceType == NmsServiceType.DownloadSegment);
            if (IsResponse && datagram.Data.Count < NmsHeaderLength + 1)
            {
                throw new ArgumentException();
            }

            if (!IsResponse && datagram.Data.Count < NmsHeaderLength + sizeof(uint))
            {
                throw new ArgumentException();
            }
            Contract.Assume(ServiceType == NmsServiceType.DownloadSegment);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsDownloadSegment"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="destanation">Адрес приемника.</param>
        /// <param name="id">Идентификатор домена.</param>
        /// <param name="offset">Смещение в домене.</param>
        /// <param name="segment">Массив данных.</param>
        /// <param name="waitResponse"><c>true</c> - ожидать подтверждения, иначе - <c>false</c>.</param>
        public NmsDownloadSegment(Address source, Address destanation, int id, uint offset, byte[] segment, bool waitResponse = true)
        {
            Contract.Requires(source != null);
            Contract.Requires(destanation != null);
            Contract.Requires(segment != null);
            Contract.Requires(segment.Length <= NmsMaxDataLength - sizeof(uint));
            Contract.Ensures(ServiceType == NmsServiceType.DownloadSegment);

            var segmentLength = Math.Min(segment.Length, NmsMaxDataLength - sizeof(uint));
            var nmsData = new byte[sizeof(uint) + segmentLength];
            BitConverter.GetBytes(offset).CopyTo(nmsData, 0);
            Array.Copy(segment, 0, nmsData, sizeof(uint), segmentLength);

            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.DownloadSegment,
                waitResponse,
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
        /// Возвращает массив данных.
        /// </summary>
        public byte[] Segment
        {
            get
            {
                Contract.Requires(!IsResponse);
                return Datagram.Data.Skip(NmsHeaderLength + sizeof(uint)).Take(Length - sizeof(uint)).ToArray();
            }
        }

        #endregion //Properties
    }
}