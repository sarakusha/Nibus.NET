using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NataInfo.Nibus.Nms.Services
{
    /// <summary>
    /// Абстрактный базовый класс для всех NMS-сообщений.
    /// </summary>
    /// <remarks>
    /// Неизменяемый объект (англ. Immutable object).
    /// Единственный метод, который изменяет состояние объекта <see cref="Initialize"/>, должен вызыватся из конструктора.
    /// </remarks>
    public abstract class NmsMessage
    {
        /// <summary>
        /// Длина заголовка NMS-сообщения.
        /// </summary>
        internal const int NmsHeaderLength = 3;

        /// <summary>
        /// Максимальный размер данных в NMS-сообщении. Размер 6 бит.
        /// </summary>
        internal const int NmsMaxDataLength = 63;

        protected NmsMessage()
        {
        }

        /// <summary>
        /// Конструктор создания NMS-сообщения из низлежащего сообщения <see cref="NibusDatagram"/>.
        /// </summary>
        /// <param name="datagram">Датаграмма.</param>
        /// <remarks>
        /// Минимальный размер длины данных <paramref name="datagram"/> должен быть не меньше размера
        /// заголовка <see cref="NmsHeaderLength"/> плюс размер NMS-данных.
        /// Конструктор потомка !должен! проверить на валидность датаграмму
        /// и в случае ошибки сгенерировать исключение <see cref="InvalidNibusDatagram"/>
        /// </remarks>
        /// <seealso cref="CreateFrom"/>
        /// <exception cref="InvalidNibusDatagram">Ошибка при декодировании датаграммы.</exception>
        protected NmsMessage(NibusDatagram datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            if (datagram.Data.Count < NmsHeaderLength
                || datagram.Data.Count < (datagram.Data[2] & 0x3F) + NmsHeaderLength)
            {
                throw new InvalidNibusDatagram("Invalid NMS message length");
            }

            Contract.Ensures(Datagram != null);

            Datagram = datagram;
        }

        /// <summary>
        /// Возвращает датаграмму низлежащего уровня.
        /// </summary>
        public NibusDatagram Datagram { get; protected set; }

        /// <summary>
        /// Возвращает тип сервиса NMS.
        /// </summary>
        public NmsServiceType ServiceType
        {
            get { return (NmsServiceType) (Datagram.Data[0] >> 3); }
        }

        /// <summary>
        /// Возвращает индикатор, что сообщение является ответом на запрос.
        /// </summary>
        public bool IsResponse
        {
            get { return (Datagram.Data[0] & 4) != 0; }
        }

        /// <summary>
        /// Возвращает идентификатор NMS-сообщения. Размер 10 бит.
        /// </summary>
        public int Id
        {
            get { return ((Datagram.Data[0] & 3) << 8) | Datagram.Data[1]; }
        }

        /// <summary>
        /// Возвращает индикатор, что сообщение требует ответа/подтверждения.
        /// </summary>
        public bool IsResponsible
        {
            get { return (Datagram.Data[2] & 0x80) == 0; }
        }

        /// <summary>
        /// Возвращает размер данных NMS-сообщения (6 бит).
        /// </summary>
        public int Length
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() <= NmsMaxDataLength);
                return Datagram.Data[2] & 0x3F;
            }
        }

        /// <summary>
        /// Возвращает код завершения в ответном сообщении.
        /// </summary>
        public int ErrorCode
        {
            get
            {
                Contract.Requires(IsResponse);
                return (sbyte)Datagram.Data[NmsHeaderLength + 0];
            }
        }

        /// <summary>
        /// Фабричный метод создания NMS-сообщений на основе датаграммы.
        /// </summary>
        /// <param name="datagram">Датаграмма.</param>
        /// <returns>Экземпляр NMS-сообщения одного из производных типов от <see cref="NmsMessage"/></returns>
        /// <exception cref="InvalidNibusDatagram"></exception>
        internal static NmsMessage CreateFrom(NibusDatagram datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            if (datagram.Data.Count < NmsHeaderLength)
            {
                throw new InvalidNibusDatagram("Invalid NMS message length");
            }
            Contract.Ensures(Contract.Result<NmsMessage>() != null);
            var serviceType = (NmsServiceType) (datagram.Data[0] >> 3);
            switch (serviceType)
            {
                case NmsServiceType.Read:
                    return new NmsRead(datagram);
                case NmsServiceType.Write:
                    return new NmsWrite(datagram);
                case NmsServiceType.InformationReport:
                    return new NmsInformationReport(datagram);
                case NmsServiceType.AckEventNotification:
                    return new NmsAckEventNotification(datagram);
                case NmsServiceType.AlterEventConditionMonitoring:
                    return new NmsAlterEventConditionMonitoring(datagram);
                case NmsServiceType.RequestDomainUpload:
                    return new NmsRequestDomainUpload(datagram);
                case NmsServiceType.InitiateUploadSequence:
                    return new NmsInitiateUploadSequence(datagram);
                case NmsServiceType.UploadSegment:
                    return new NmsUploadSegment(datagram);
                case NmsServiceType.RequestDomainDownload:
                    return new NmsRequestDomainDownload(datagram);
                case NmsServiceType.InitiateDownloadSequence:
                    return new NmsInitiateDownloadSequence(datagram);
                case NmsServiceType.DownloadSegment:
                    return new NmsDownloadSegment(datagram);
                case NmsServiceType.VerifyDomainChecksum:
                    return new NmsVerifyDomainChecksum(datagram);
                case NmsServiceType.ExecuteProgramInvocation:
                    return new NmsExecuteProgramInvocation(datagram);
                case NmsServiceType.StartProgramInvocation:
                    return new NmsStartProgramInvocation(datagram);
                case NmsServiceType.Stop:
                    return new NmsStop(datagram);
                case NmsServiceType.Resume:
                    return new NmsResume(datagram);
                case NmsServiceType.Reset:
                    return new NmsReset(datagram);
                case NmsServiceType.Shutdown:
                    return new NmsShutdown(datagram);
                default:
                    throw new InvalidNibusDatagram("Unknown NMS service");
            }
        }

        /// <summary>
        /// Возвращает размер данных указанного типа и возможно значения для массива или строки.
        /// </summary>
        /// <param name="vt">Тип данных.</param>
        /// <param name="value">Значение данных в случае массива или строки.</param>
        /// <returns>Занимаемый размер.</returns>
        public static int GetSizeOf(NmsValueType vt, object value = null)
        {
            switch (vt)
            {
                case NmsValueType.Boolean:
                case NmsValueType.Int8:
                case NmsValueType.UInt8:
                    return 1;
                case NmsValueType.Int16:
                case NmsValueType.UInt16:
                    return 2;
                case NmsValueType.Int32:
                case NmsValueType.UInt32:
                case NmsValueType.Real32:
                    return 4;
                case NmsValueType.Int64:
                case NmsValueType.UInt64:
                case NmsValueType.Real64:
                    return 8;
                case NmsValueType.DateTime:
                    return 10;
                case NmsValueType.String:
                    return Math.Min(NmsMaxDataLength - 1, (value != null ? ((string) value).Length : 0) + 1);
            }

            if (((byte) vt & (byte) NmsValueType.Array) == 0)
            {
                throw new ArgumentException("Invalid ValueType");
            }

            var arrayType = (NmsValueType) ((byte) vt & ((byte) NmsValueType.Array - 1));
            var itemSize = GetSizeOf(arrayType);
            return value != null ? ((ICollection) value).Count*itemSize : itemSize;
        }

        /// <summary>
        /// Чтение значения NMS из буфера.
        /// </summary>
        /// <param name="valueType">Тип значения.</param>
        /// <param name="buffer">Буфер.</param>
        /// <param name="startIndex">Начальный индекс в буфере, с которого хранится непосредственно значение.</param>
        /// <returns>Значение NMS.</returns>
        protected static object ReadValue(NmsValueType valueType, byte[] buffer, int startIndex)
        {
            Contract.Requires(buffer != null);
            Contract.Requires(startIndex >= 0);
            switch (valueType)
            {
                case NmsValueType.Boolean:
                    return buffer[startIndex] != 0;
                case NmsValueType.Int8:
                    return (sbyte) buffer[startIndex];
                case NmsValueType.Int16:
                    return BitConverter.ToInt16(buffer, startIndex);
                case NmsValueType.Int32:
                    return BitConverter.ToInt32(buffer, startIndex);
                case NmsValueType.Int64:
                    return BitConverter.ToInt64(buffer, startIndex);
                case NmsValueType.UInt8:
                    return buffer[startIndex];
                case NmsValueType.UInt16:
                    return BitConverter.ToUInt16(buffer, startIndex);
                case NmsValueType.UInt32:
                    return BitConverter.ToInt32(buffer, startIndex);
                case NmsValueType.UInt64:
                    return BitConverter.ToUInt64(buffer, startIndex);
                case NmsValueType.Real32:
                    return BitConverter.ToSingle(buffer, startIndex);
                case NmsValueType.Real64:
                    return BitConverter.ToDouble(buffer, startIndex);
                case NmsValueType.String:
                    return Encoding.Default.GetString(
                        buffer.Skip(startIndex).Take(NmsMaxDataLength - 1).TakeWhile(b => b != 0).ToArray());
                case NmsValueType.DateTime:
                    return GetDateTime(buffer, startIndex);
            }

            if (((byte) valueType & (byte) NmsValueType.Array) == 0)
            {
                throw new ArgumentException("Invalid ValueType");
            }

            var arrayType = (NmsValueType) ((byte) valueType & ((byte) NmsValueType.Array - 1));
            var arraySize = buffer.Length - startIndex /* - 1*/;
            var itemSize = GetSizeOf(arrayType);
            var arrayLength = arraySize/itemSize;
            var array = new object[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                array[i] = ReadValue(arrayType, buffer, startIndex);
                startIndex += itemSize;
            }

            switch (arrayType)
            {
                case NmsValueType.Boolean:
                    return array.Cast<bool>().ToArray();
                case NmsValueType.Int8:
                    return array.Cast<sbyte>().ToArray();
                case NmsValueType.Int16:
                    return array.Cast<short>().ToArray();
                case NmsValueType.Int32:
                    return array.Cast<int>().ToArray();
                case NmsValueType.Int64:
                    return array.Cast<long>().ToArray();
                case NmsValueType.UInt8:
                    return array.Cast<byte>().ToArray();
                case NmsValueType.UInt16:
                    return array.Cast<ushort>().ToArray();
                case NmsValueType.UInt32:
                    return array.Cast<uint>().ToArray();
                case NmsValueType.UInt64:
                    return array.Cast<ulong>().ToArray();
                case NmsValueType.Real32:
                    return array.Cast<float>().ToArray();
                case NmsValueType.Real64:
                    return array.Cast<double>().ToArray();
                case NmsValueType.String:
                    return array.Cast<string>().ToArray();
                case NmsValueType.DateTime:
                    return array.Cast<DateTime>().ToArray();
            }
            return array;
        }

        /// <summary>
        /// Записывает тип значения и само значение в буфер.
        /// </summary>
        /// <param name="valueType">Тип значения.</param>
        /// <param name="value">Значение.</param>
        /// <returns>Массив байт максимальной длины <see cref="NmsMaxDataLength"/> с данными.</returns>
        /// <exception cref="ArgumentException">Неверный тип значения.</exception>
        /// <exception cref="FormatException">Ошибка преобразования.</exception>
        /// <exception cref="InvalidCastException">Ошибка преобразования.</exception>
        /// <exception cref="OverflowException">Ошибка преобразования.</exception>
        protected static byte[] WriteValue(NmsValueType valueType, object value)
        {
            Contract.Requires(value != null);
            Contract.Ensures(Contract.Result<byte[]>().Length <= NmsMaxDataLength);

            var data = new List<byte>(NmsMaxDataLength) {(byte) valueType};
            switch (valueType)
            {
                case NmsValueType.Boolean:
                    data.Add(Convert.ToBoolean(value) ? (byte) 1 : (byte) 0);
                    break;
                case NmsValueType.Int8:
                    data.Add((byte) Convert.ToSByte(value));
                    break;
                case NmsValueType.Int16:
                    data.AddRange(BitConverter.GetBytes(Convert.ToInt16(value)));
                    break;
                case NmsValueType.Int32:
                    data.AddRange(BitConverter.GetBytes(Convert.ToInt32(value)));
                    break;
                case NmsValueType.Int64:
                    data.AddRange(BitConverter.GetBytes(Convert.ToInt64(value)));
                    break;
                case NmsValueType.UInt8:
                    data.Add(Convert.ToByte(value));
                    break;
                case NmsValueType.UInt16:
                    data.AddRange(BitConverter.GetBytes(Convert.ToUInt16(value)));
                    break;
                case NmsValueType.UInt32:
                    data.AddRange(BitConverter.GetBytes(Convert.ToUInt32(value)));
                    break;
                case NmsValueType.UInt64:
                    data.AddRange(BitConverter.GetBytes(Convert.ToUInt64(value)));
                    break;
                case NmsValueType.Real32:
                    data.AddRange(BitConverter.GetBytes(Convert.ToSingle(value)));
                    break;
                case NmsValueType.Real64:
                    data.AddRange(BitConverter.GetBytes(Convert.ToDouble(value)));
                    break;
                case NmsValueType.String:
                    data.AddRange(Encoding.Default.GetBytes(value.ToString()).Take(NmsMaxDataLength - 1));
                    if (value.ToString().Length < NmsMaxDataLength - 1)
                    {
                        data.Add(0);
                    }

                    break;
                case NmsValueType.DateTime:
                    var dt = Convert.ToDateTime(value);
                    data.AddRange(
                        new[]
                            {
                                PackByte(dt.Day),
                                PackByte(dt.Month),
                                PackByte(dt.Year%100),
                                PackByte(dt.Year/100),
                                PackByte(dt.Hour),
                                PackByte(dt.Minute),
                                PackByte(dt.Second),
                                (byte) (dt.Millisecond/100 & 0x0f),
                                PackByte(dt.Millisecond%100),
                                (byte) (dt.DayOfWeek + 1)
                            });
                    break;
            }

            if (((byte) valueType & (byte) NmsValueType.Array) == 0)
            {
                throw new ArgumentException("Invalid ValueType");
            }

            var arrayType = (NmsValueType) ((byte) valueType & ((byte) NmsValueType.Array - 1));
            foreach (var item in (IEnumerable) value)
            {
                data.AddRange(WriteValue(arrayType, item));
            }

            Contract.Assert(data.Count <= NmsMaxDataLength);
            return data.Take(NmsMaxDataLength).ToArray();
        }

        /// <summary>
        /// Инициализация NMS-сообщения.
        /// </summary>
        /// <param name="source">Адрес источника сообщения.</param>
        /// <param name="destanation">Адрес получателя сообщения.</param>
        /// <param name="priority">Приоритет.</param>
        /// <param name="service">Тип сервиса.</param>
        /// <param name="isResponsible">Требуется ли ответ/подтверждение на запрос.</param>
        /// <param name="id">Идентификатор NMS-сообщения.</param>
        /// <param name="r1">Разрешено ли событие для <see cref="NmsServiceType.AlterEventConditionMonitoring"/>.</param>
        /// <param name="nmsData">NMS-данные.</param>
        /// <remarks>Это единственный метод, который может изменить сообщение. Должен вызываться в конструкторе.</remarks>
        protected void Initialize(
            Address source,
            Address destanation,
            PriorityType priority,
            NmsServiceType service,
            bool isResponsible,
            int id,
            bool r1,
            byte[] nmsData)
        {
            Contract.Requires(source != null);
            Contract.Requires(destanation != null);
            Contract.Requires(nmsData != null);
            Contract.Requires(nmsData.Length <= NmsMaxDataLength);
            Contract.Ensures(!IsResponse);
            Contract.Ensures(Datagram != null);
            Contract.Ensures(ServiceType == service);
            var nmsLength = Math.Min(nmsData.Length, NmsMaxDataLength);
            var data = new byte[NmsHeaderLength + nmsLength];
            Array.Copy(nmsData, 0, data, NmsHeaderLength, nmsLength);

            data[0] = (byte) (((byte) service << 3) | ((id & 1023) >> 8));
            data[1] = (byte) (id & 0xFF);
            data[2] = (byte) ((isResponsible ? 0 : 0x80) | (r1 ? 0x40 : 0) | nmsLength & 0x3F);
            Datagram = new NibusDatagram(null, source, destanation, priority, ProtocolType.Nms, data);
            Contract.Assume(!IsResponse);
        }

        internal static byte PackByte(int b)
        {
            Contract.Assert(0 <= b && b < 100);
            b %= 100;
            return (byte) (b/10*16 + b%10);
        }

        internal static int UnpackByte(byte b)
        {
            Contract.Ensures(0 <= Contract.Result<int>());
            Contract.Ensures(Contract.Result<int>() < 100);
            Contract.Assume((b & 0x0f) < 10 && ((b >> 4) < 10));
            return (b & 0x0f) + (b >> 4)*10;
        }

        private static DateTime GetDateTime(byte[] buffer, int startIndex)
        {
            var day = UnpackByte(buffer[0 + startIndex]);
            Contract.Assume(day >= 1);
            var month = UnpackByte(buffer[1 + startIndex]);
            Contract.Assume(1 <= month && month <= 12);
            var year = UnpackByte(buffer[3 + startIndex]) + UnpackByte(buffer[2 + startIndex])*100;
            Contract.Assume(1 <= year);
            var hour = UnpackByte(buffer[4 + startIndex]);
            Contract.Assume(hour <= 24);
            var minute = UnpackByte(buffer[5 + startIndex]);
            Contract.Assume(minute <= 60);
            var second = UnpackByte(buffer[6 + startIndex]);
            Contract.Assume(second <= 60);
            var ms = UnpackByte(buffer[8 + startIndex]) + (buffer[7 + startIndex] & 0x0f)*100;
            return new DateTime(year, month, day, hour, minute, second, ms);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Datagram != null);
            Contract.Invariant(Datagram.ProtocolType == ProtocolType.Nms);
            Contract.Invariant(Datagram.Data.Count == Length + NmsHeaderLength);
            Contract.Invariant(Length >= 0);
            Contract.Invariant(Length <= NmsMaxDataLength);
        }
    }
}