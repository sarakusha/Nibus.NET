//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsStop.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsStop : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsStop(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.Stop);
        }

        public NmsStop(Address source, Address destanation, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.Stop);
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.Stop, true, id, false, new byte[0]);
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