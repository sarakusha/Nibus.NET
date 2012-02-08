//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsRequestDomainDownload.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    /// <summary>
    /// Класс-обертка для сообщений о запросе загрузки массива данных в устройство.
    /// </summary>
    public sealed class NmsRequestDomainDownload : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsRequestDomainDownload(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Assume(ServiceType == NmsServiceType.RequestDomainDownload);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsRequestDomainDownload"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="destanation">Адрес приемника.</param>
        /// <param name="domain">Домен.</param>
        public NmsRequestDomainDownload(Address source, Address destanation, string domain)
        {
            Contract.Requires(domain != null);
            Contract.Requires(domain.Length <= 8);
            Contract.Ensures(ServiceType == NmsServiceType.RequestDomainDownload);
            var data = new byte[8];
            Encoding.Default.GetBytes(domain).CopyTo(data, 0);
            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.RequestDomainDownload,
                true,
                0,
                false,
                data);
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Возвращает запрашиваемый домен для загрузки.
        /// </summary>
        public string Domain
        {
            get
            {
                Contract.Requires(!IsResponse);
                return
                    Encoding.Default.GetString(
                        Datagram.Data.Skip(NmsHeaderLength).Take(8).TakeWhile(b => b != 0).ToArray());
            }
        }

        /// <summary>
        /// Возвращает размер запрашиваемого домена.
        /// </summary>
        public uint DomainSize
        {
            get
            {
                Contract.Requires(IsResponse);
                return BitConverter.ToUInt32(Datagram.Data.ToArray(), NmsHeaderLength + 1);
            }
        }

        /// <summary>
        /// Возвращает признак, что загрузка может происходить без подверждения успешного приема.
        /// </summary>
        public bool IsFastDownload
        {
            get
            {
                Contract.Requires(IsResponse);
                return Datagram.Data.Count > NmsHeaderLength + 5 && (Datagram.Data[NmsHeaderLength + 5] & 1) != 0;
            }
        }

        #endregion //Properties
    }
}