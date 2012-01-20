//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsInformationReport.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;
using System.Linq;
using NataInfo.Nibus.Sport;

#endregion

namespace NataInfo.Nibus.Nms
{
    /// <summary>
    /// Класс-обертка для информационных NMS-сообщений.
    /// </summary>
    public sealed class NmsInformationReport : NmsMessage
    {
        #region Member Variables

        private object _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор создания NMS-сообщения из низлежащего сообщения <see cref="NibusDatagram"/>.
        /// </summary>
        /// <param name="datagram">Датаграмма.</param>
        /// <remarks>
        /// Требуется, чтобы <see cref="NmsMessage.ServiceType"/> созданного из датаграммы NMS-сообщения
        /// был равен <see cref="NmsServiceType.InformationReport"/>
        /// </remarks>
        internal NmsInformationReport(NibusDatagram datagram) : base(datagram)
        {
            Contract.Ensures(!IsResponce);
            Contract.Assume(ServiceType == NmsServiceType.InformationReport);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsInformationReport"/> class.
        /// </summary>
        /// <param name="source">Адрес источника сообщения.</param>
        /// <param name="id">Идентификатор информационного сообщения. <seealso cref="GameReports"/></param>
        /// <param name="valueType">Тип значения информационного сообщения.</param>
        /// <param name="value">Значение информационного сообщения.</param>
        /// <param name="priority">Приоритет.</param>
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

        /// <summary>
        /// Возвращает тип для сохраненного значения информационного сообщения.
        /// </summary>
        public NmsValueType ValueType
        {
            get { return (NmsValueType)Datagram.Data[NmsHeaderLength + 0]; }
        }

        /// <summary>
        /// Возвращает сохраненное значение информационного сообщения.
        /// </summary>
        public object Value
        {
            get { return _value ?? (_value = ReadValue(ValueType, Datagram.Data.ToArray(), NmsHeaderLength + 1)); }
        }

        #endregion //Properties
    }
}