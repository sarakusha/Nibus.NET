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
    /// Сообщение сервиса <see cref="NmsServiceType.Read"/> - "прочитать значение переменной".
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
        /// Конструктор создания NMS-сообщения сервиса <see cref="NmsServiceType.Read"/>
        /// из низлежащего сообщения <see cref="NibusDatagram"/>.
        /// </summary>
        /// <param name="datagram">Датаграмма.</param>
        /// <remarks>
        /// Минимальный размер длины данных <paramref name="datagram"/> должен быть не меньше размера
        /// заголовка <see cref="NmsMessage.NmsHeaderLength"/> плюс размер NMS-данных.
        /// </remarks>
        /// <seealso cref="NmsMessage.CreateFrom"/>
        /// <exception cref="InvalidNibusDatagram"></exception>
        internal NmsRead(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Ensures(ServiceType == NmsServiceType.Read);
            Contract.Assume(ServiceType == NmsServiceType.Read);
            if (!IsResponse) return;

            if (datagram.Data.Count < NmsHeaderLength + 3)
            {
                throw new InvalidNibusDatagram("Invalid NMS message length");
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
                Contract.Requires(IsResponse);
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
                Contract.Requires(IsResponse);
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
                Contract.Requires(IsResponse);
                return _value;
            }
        }

        #endregion //Properties
    }
}