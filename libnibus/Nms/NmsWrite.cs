//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsWrite.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsWrite : NmsMessage
    {
        #region Member Variables

        private object _value;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsWrite(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.Write);
            if (datagram.Data.Count < NmsHeaderLength + 1)
            {
                throw new ArgumentException();
            }
        }

        public NmsWrite(Address source, Address destanation, int id, NmsValueType valueType, object value, bool isResponsible = true, PriorityType priority = PriorityType.Normal)
        {
            Initialize(
                source,
                destanation,
                priority,
                NmsServiceType.Write,
                isResponsible,
                id,
                false,
                WriteValue(valueType, value));
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

        public NmsValueType ValueType
        {
            get
            {
                Contract.Requires(!IsResponce);
                return (NmsValueType)Datagram.Data[NmsHeaderLength + 0];
            }
        }

        public object Value
        {
            get
            {
                Contract.Requires(!IsResponce);
                return _value ?? (_value = ReadValue(ValueType, Datagram.Data.ToArray(), NmsHeaderLength + 1));
            }
        }

        #endregion //Properties

        #region Methods

        #endregion //Methods

        #region Implementations

        #endregion //Implementations
    }
}