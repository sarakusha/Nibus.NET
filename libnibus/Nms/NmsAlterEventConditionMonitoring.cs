//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsAlterEventConditionMonitoring.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;

#endregion

namespace NataInfo.Nibus.Nms
{
    internal sealed class NmsAlterEventConditionMonitoring : NmsMessage
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsAlterEventConditionMonitoring(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.AlterEventConditionMonitoring);
        }

        #endregion //Constructors

        #region Properties

        public bool IsEventEnabled
        {
            get
            {
                Contract.Requires(!IsResponce);
                return (Datagram.Data[2] & 0x40) != 0;
            }
        }

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