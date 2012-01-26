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
    /// <summary>
    /// Класс-обертка для сообщений сервиса <see cref="NmsServiceType.Write"/>
    /// "Изменить значение переменной".
    /// </summary>
    public sealed class NmsWrite : NmsMessage
    {
        #region Member Variables

        private object _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор создания NMS-сообщения сервиса <see cref="NmsServiceType.Write"/>
        /// из низлежащего сообщения <see cref="NibusDatagram"/>.
        /// </summary>
        /// <param name="datagram">Датаграмма.</param>
        /// <remarks>
        /// Минимальный размер длины данных <paramref name="datagram"/> должен быть не меньше размера
        /// заголовка <see cref="NmsMessage.NmsHeaderLength"/> плюс размер NMS-данных.
        /// </remarks>
        /// <seealso cref="NmsMessage.CreateFrom"/>
        /// <exception cref="InvalidNibusDatagram"></exception>
        internal NmsWrite(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Ensures(ServiceType == NmsServiceType.Write);
            if (datagram.Data.Count < NmsHeaderLength + 1
                || !IsResponse && datagram.Data.Count < NmsHeaderLength + 1 + GetSizeOf(ValueType))
            {
                throw new InvalidNibusDatagram("Invalid NMS message length");
            }
            Contract.Assume(ServiceType == NmsServiceType.Write);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsWrite"/> class.
        /// </summary>
        /// <param name="source">Адрес источника сообщения.</param>
        /// <param name="destanation">Адрес получателя сообщения.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="valueType">Тип значения.</param>
        /// <param name="value">Записываемое значение.</param>
        /// <param name="waitResponse"><c>true</c> - если требуется подтверждение записи.</param>
        /// <param name="priority">Приоритет.</param>
        public NmsWrite(Address source, Address destanation, int id, NmsValueType valueType, object value,
                        bool waitResponse = true, PriorityType priority = PriorityType.Normal)
        {
            Contract.Requires(source != null);
            Contract.Requires(destanation != null);
            Contract.Requires(value != null);
            Initialize(
                source,
                destanation,
                priority,
                NmsServiceType.Write,
                waitResponse,
                id,
                false,
                WriteValue(valueType, value));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsWrite"/> class.
        /// </summary>
        /// <param name="destanation">Адрес получателя сообщения.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="valueType">Тип значения.</param>
        /// <param name="value">Записываемое значение.</param>
        /// <param name="waitResponse"><c>true</c> - если требуется подтверждение записи.</param>
        /// <param name="priority">Приоритет.</param>
        public NmsWrite(
            Address destanation,
            int id,
            NmsValueType valueType,
            object value,
            bool waitResponse = true,
            PriorityType priority = PriorityType.Normal)
            : this(Address.Empty, destanation, id, valueType, value, waitResponse, priority)
        {
            Contract.Requires(destanation != null);
            Contract.Requires(value != null);
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Возвращает тип значения для записи.
        /// </summary>
        public NmsValueType ValueType
        {
            get
            {
                Contract.Requires(!IsResponse);
                return (NmsValueType) Datagram.Data[NmsHeaderLength + 0];
            }
        }

        /// <summary>
        /// Возвращает значение для записи.
        /// </summary>
        public object Value
        {
            get
            {
                Contract.Requires(!IsResponse);
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