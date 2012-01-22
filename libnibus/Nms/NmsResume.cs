//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsResume.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsResume : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsResume(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Assume(ServiceType == NmsServiceType.Resume);
        }

        public NmsResume(Address source, Address destanation, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.Resume);
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.Resume, true, id, false, new byte[0]);
        }

        #endregion //Constructors

        #region Properties

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