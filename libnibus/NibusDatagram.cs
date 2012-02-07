using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace NataInfo.Nibus
{
    // ReSharper disable ReturnTypeCanBeEnumerable.Global
    /// <summary>
    /// Интерфейс датаграммы протокола NiBUS.
    /// </summary>
    public interface INibusDatagram
    {
        /// <summary>
        /// Возвращает адрес устройства назначения.
        /// </summary>
        Address Destanation { get; }

        /// <summary>
        /// Возвращает адрес устройства отправителя.
        /// </summary>
        Address Source { get; }

        /// <summary>
        /// Возвращает приоритет сообщения.
        /// </summary>
        PriorityType Priority { get; }

        /// <summary>
        /// Возвращает тип протокола вышестоящего уровня.
        /// </summary>
        ProtocolType ProtocolType { get; }

        /// <summary>
        /// Возвращает массив данных.
        /// </summary>
        ReadOnlyCollection<byte> Data { get; }
    }

    // ReSharper restore ReturnTypeCanBeEnumerable.Global

    /// <summary>
    /// Класс-обертка для датаграмм NiBUS.
    /// </summary>
    /// <remarks>Неизменяемый объект (англ. Immutable object)</remarks>
    public class NibusDatagram : INibusDatagram
    {
        /// <summary>
        /// Максимальный размер данных в датаграмме.
        /// </summary>
        public const int MaxDataLength = 238;

        /// <summary>
        /// Initializes a new instance of the <see cref="NibusDatagram"/> class.
        /// </summary>
        /// <param name="provider">Поставщик датаграммы.</param>
        /// <param name="source">Адрес отправителя.</param>
        /// <param name="destanation">Адрес получателя.</param>
        /// <param name="priority">Приоритет.</param>
        /// <param name="protocol">Тип протокола.</param>
        /// <param name="data">Данные.</param>
        public NibusDatagram(
            ICodecInfo provider,
            Address source,
            Address destanation,
            PriorityType priority,
            ProtocolType protocol,
            IList<byte> data)
        {
            Contract.Requires(destanation != null);
            Contract.Requires(source != null);
            Contract.Requires(data != null);
            Contract.Ensures(Data != null);
            Contract.Ensures(Destanation != null);
            Contract.Ensures(Source != null);
            if (data == null || data.Count > MaxDataLength)
            {
                throw new ArgumentException("Invalid data");
            }

            Provider = provider;
            Destanation = destanation;
            Source = source;
            ProtocolType = protocol;
            Data = new ReadOnlyCollection<byte>(data);
            Priority = priority;
        }

        /// <summary>
        /// Возвращает поставщика датаграммы.
        /// </summary>
        /// <value>
        /// Если датаграмма получена извне, то здесь будет сохранен <see cref="NibusDataCodec"/> получивший кадр.
        /// Если датаграмма предназначена для передачи, то значение может быть <c>null</c>.
        /// </value>
        public ICodecInfo Provider { get; private set; }

        /// <summary>
        /// Возвращает адрес устройства назначения.
        /// </summary>
        public Address Destanation { get; private set; }

        /// <summary>
        /// Возвращает адрес устройства отправителя.
        /// </summary>
        public Address Source { get; private set; }

        /// <summary>
        /// Возвращает приоритет сообщения.
        /// </summary>
        public PriorityType Priority { get; private set; }

        /// <summary>
        /// Возвращает тип протокола вышестоящего уровня.
        /// </summary>
        public ProtocolType ProtocolType { get; private set; }

        /// <summary>
        /// Возвращает массив данных.
        /// </summary>
        public ReadOnlyCollection<byte> Data { get; private set; }
    }
}