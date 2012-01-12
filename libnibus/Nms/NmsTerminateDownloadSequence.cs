//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsTerminateDownloadSequence.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsTerminateDownloadSequence : NmsMessage
    {
        #region Constructors

        public NmsTerminateDownloadSequence(NibusDatagram datagram) : base(datagram)
        {
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