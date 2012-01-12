//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsInitiateDownloadSequence.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsInitiateDownloadSequence : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsInitiateDownloadSequence(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.InitiateDownloadSequence);
        }

        public NmsInitiateDownloadSequence(Address source, Address destanation, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.InitiateDownloadSequence);
            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.InitiateDownloadSequence,
                true,
                id,
                false,
                new byte[0]);
        }

        #endregion //Constructors

        #region Properties

        public int ErrorCode
        {
            get
            {
                Contract.Requires(IsResponce);
                return Datagram.Data[NmsHeaderLength + 0];
            }
        }

        #endregion //Properties
    }
}