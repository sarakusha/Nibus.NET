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
    /// <summary>
    /// Сообщение сервиса <see cref="NmsServiceType.Read"/>.
    /// </summary>
    public sealed class NmsRead : NmsMessage
    {
        #region Member Variables

        private readonly int _errorCode;

        private readonly object _value;
        private readonly NmsValueType _valueType;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор создания NMS-сообщения из низлежащего сообщения <see cref="NibusDatagram"/>.
        /// </summary>
        /// <param name="datagram">Датаграмма.</param>
        internal NmsRead(NibusDatagram datagram)
            : base(datagram)
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

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsRead"/> class.
        /// </summary>
        /// <param name="source">Адрес источника сообщения.</param>
        /// <param name="destanation">Адрес получателя сообщения.</param>
        /// <param name="id">Идентификатор переменной.</param>
        public NmsRead(Address source, Address destanation, int id)
        {
            Initialize(source, destanation, PriorityType.Realtime, NmsServiceType.Read, true, id, false, new byte[0]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsRead"/> class.
        /// </summary>
        /// <param name="destanation">Адрес получателя сообщения.</param>
        /// <param name="id">Идентификатор переменной.</param>
        public NmsRead(Address destanation, int id)
            : this(Address.Empty, destanation, id)
        {
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Возвращает код завершения в ответном сообщении.
        /// </summary>
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

        /// <summary>
        /// Возвращает тип значения в ответном сообщении.
        /// </summary>
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

        /// <summary>
        /// Возвращенное на запрос значение переменной.
        /// </summary>
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