//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsRead.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion

namespace NataInfo.Nibus.Nms
{
    internal sealed class NmsRead : NmsMessage
    {
        #region Member Variables

        private readonly int _errorCode;

        private readonly object _value;
        private readonly NmsValueType _valueType;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsRead(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.Read);
            if (!IsResponce) return;

            if (datagram.Data.Count < NmsHeaderLength + 3)
            {
                throw new ArgumentException("Invalid NMS Read Responce");
            }

            _errorCode = datagram.Data[NmsHeaderLength + 0];
            _valueType = (NmsValueType)datagram.Data[NmsHeaderLength + 1];
            _value = ReadValue(_valueType, datagram.Data.ToArray(), NmsHeaderLength + 2);
        }

        public NmsRead(Address source, Address destanation, int id)
        {
            Initialize(source, destanation, PriorityType.Normal, NmsServiceType.Read, true, id, false, new byte[0]);
        }

        #endregion //Constructors

        #region Properties

        public int ErrorCode
        {
            get
            {
                Contract.Requires(IsResponce);
                if (!IsResponce)
                {
                    throw new InvalidOperationException();
                }

                return _errorCode;
            }
        }

        public NmsValueType ValueType
        {
            get
            {
                Contract.Requires(IsResponce);
                if (!IsResponce)
                {
                    throw new InvalidOperationException();
                }

                return _valueType;
            }
        }

        public object Value
        {
            get
            {
                Contract.Requires(IsResponce);
                if (!IsResponce)
                {
                    throw new InvalidOperationException();
                }

                return _value;
            }
        }

        #endregion //Properties
    }
}