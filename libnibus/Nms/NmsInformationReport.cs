//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsInformationReport.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;
using System.Linq;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsInformationReport : NmsMessage
    {
        #region Member Variables

        private object _value;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsInformationReport(NibusDatagram datagram) : base(datagram)
        {
            Contract.Ensures(!IsResponce);
            Contract.Assume(ServiceType == NmsServiceType.InformationReport);
        }

        public NmsInformationReport(Address source, int id, NmsValueType valueType, object value, PriorityType priority = PriorityType.Normal)
        {
            Contract.Ensures(!IsResponce);
            Contract.Ensures(ServiceType == NmsServiceType.InformationReport);
            Initialize(
                source,
                Address.Broadcast,
                priority,
                NmsServiceType.InformationReport,
                false,
                id,
                false,
                WriteValue(valueType, value));
        }

        #endregion //Constructors

        #region Properties

        public NmsValueType ValueType
        {
            get { return (NmsValueType)Datagram.Data[0]; }
        }

        public object Value
        {
            get { return _value ?? (_value = ReadValue(ValueType, Datagram.Data.ToArray(), 1)); }
        }

        #endregion //Properties
    }
}