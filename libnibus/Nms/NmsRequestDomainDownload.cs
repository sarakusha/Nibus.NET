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

namespace NataInfo.Nibus.Nms
{
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

        public NmsRequestDomainDownload(Address source, Address destanation, string domain)
        {
            Contract.Requires(domain != null);
            Contract.Requires(domain.Length <= 8);
            Contract.Ensures(ServiceType == NmsServiceType.RequestDomainDownload);
            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.RequestDomainDownload,
                true,
                0,
                false,
                Encoding.Default.GetBytes(domain));
        }

        #endregion //Constructors

        #region Properties

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

        public uint DomainSize
        {
            get
            {
                Contract.Requires(IsResponse);
                return BitConverter.ToUInt32(Datagram.Data.ToArray(), NmsHeaderLength + 1);
            }
        }

        #endregion //Properties
    }
}