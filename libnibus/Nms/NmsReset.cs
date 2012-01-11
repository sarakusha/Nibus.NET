//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsReset.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    internal sealed class NmsReset : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsReset(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.Reset);
        }

        public NmsReset(Address source, Address destanation, int id)
        {
            Contract.Ensures(ServiceType == NmsServiceType.Reset);
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.Reset, true, id, false, new byte[0]);
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