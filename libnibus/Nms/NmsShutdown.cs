//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsShutdown.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    internal sealed class NmsShutdown : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsShutdown(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.Shutdown);
        }

        public NmsShutdown(Address source, Address destanation, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.Shutdown);
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.Shutdown, true, id, false, new byte[0]);
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