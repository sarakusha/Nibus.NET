//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsTerminateDownloadSequence.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    public sealed class NmsTerminateDownloadSequence : NmsMessage
    {
        #region Constructors

        internal NmsTerminateDownloadSequence(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Assume(ServiceType == NmsServiceType.TerminateDownloadSequence);
        }

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsTerminateDownloadSequence(Address source, Address destanation, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.TerminateDownloadSequence);
            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.TerminateDownloadSequence,
                false,
                id,
                false,
                new byte[0]);
        }

        #endregion //Constructors
    }
}