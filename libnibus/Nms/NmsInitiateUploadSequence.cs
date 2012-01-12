//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsInitiateUploadSequence.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsInitiateUploadSequence : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsInitiateUploadSequence(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.InitiateUploadSequence);
        }

        public NmsInitiateUploadSequence(Address source, Address destanation, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.InitiateUploadSequence);
            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.InitiateUploadSequence,
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