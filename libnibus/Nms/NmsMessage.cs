using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NataInfo.Nibus.Nms
{
    internal enum NmsServiceType
    {
        None = 0,
        Read = 1,
        Write = 2,
        InformationReport = 3,
        EventNotification = 4,
        AckEventNotification = 5,
        AlterEventConditionMonitoring = 6,
        RequestDomainUpload = 7,
        InitiateUploadSequence = 8,
        UploadSegment = 9,
        RequestDomainDownload = 10,
        InitiateDownloadSequence = 11,
        DownloadSegment = 12,
        TerminateDownloadSequence = 13,
        VerifyDomainChecksum = 14,
        ExecuteProgramInvocation = 15,
        StartProgramInvocation = 16,
        Stop = 17,
        Resume = 18,
        Reset = 19,
        Shutdown = 20
    }

    internal abstract class NmsMessage 
    {
        #region NmsValueType enum

        public enum NmsValueType
        {
            Boolean = 11, // 8 бит Значение TRUE = 1/FALSE = 0
            Int8 = 16, // 8 бит Знаковый байт
            Int16 = 2, // 16 бит Знаковое короткое целое
            Int32 = 3, // 32 бита Знаковое целое
            Int64 = 20, // 64 бита Знаковое длинное целое
            UInt8 = 17, // 8 бит Байт
            UInt16 = 18, // 16 бит Короткое целое
            UInt32 = 19, // 32 бита Целое
            UInt64 = 21, // 64 бита Длинное целое
            Real32 = 4, // 32 бита Значение с плавающей точкой
            Real64 = 5, // 64 бита Значение с плавающей точкой удвоенной точности
            String = 30, // Строка символов с терминирующим нулем
            DateTime = 7, // 80 бит Дата/время в формате BCD
            // DD-MM-YYYY HH:MM:SS.0mmmbW
            // DD – дата
            // MM – месяц
            // YYYY – год
            // HH – час (0..23)
            // MM – минуты
            // SS – секунды
            // mmm – миллисекунды
            // W – день недели (1..7,
            // 1 – вс,
            // 2 – пн,
            // … 7 – сб)
            // b – зарезервировано
            Array = 0x80
        }

        #endregion

        public const int NmsHeaderLength = 3;
        public const int NmsMaxDataLength = 63;

        protected NmsMessage()
        {
        }

        protected NmsMessage(NibusDatagram datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.Protocol == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Ensures(Datagram != null);

            if (datagram.Data.Count < Length + NmsHeaderLength)
            {
                throw new ArgumentException("Invalid data");
            }

            Datagram = datagram;
        }

        public NibusDatagram Datagram { get; private set; }

        public NmsServiceType ServiceType
        {
            get
            {
                Contract.Requires(Datagram != null);
                return (NmsServiceType)(Datagram.Data[0] >> 3);
            }
        }

        public bool IsResponce
        {
            get
            {
                Contract.Requires(Datagram != null);
                return (Datagram.Data[0] & 4) != 0;
            }
        }

        public int Id
        {
            get
            {
                Contract.Requires(Datagram != null);
                return ((Datagram.Data[0] & 3) << 8) | Datagram.Data[1];
            }
        }

        public bool IsResponsible
        {
            get
            {
                Contract.Requires(Datagram != null);
                return (Datagram.Data[2] & 0x80) == 0;
            }
        }

        public int Length
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() <= NmsMaxDataLength);
                return Datagram.Data[2] & 0x3F;
            }
        }

        public static NmsMessage CreateFrom(NibusDatagram datagram)
        {
            if (datagram.Protocol != ProtocolType.Nms)
            {
                throw new InvalidOperationException("nms protocol required");
            }

            var serviceType = (NmsServiceType)(datagram.Data[0] >> 3);
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
                    throw new ArgumentException("Unknown NMS service");
            }
        }

        protected static object ReadValue(NmsValueType valueType, byte[] buffer, int offset)
        {
            Contract.Requires(buffer != null);
            switch (valueType)
            {
                case NmsValueType.Boolean:
                    return buffer[0] != 0;
                case NmsValueType.Int8:
                    return (sbyte)buffer[offset];
                case NmsValueType.Int16:
                    return BitConverter.ToInt16(buffer, offset);
                case NmsValueType.Int32:
                    return BitConverter.ToInt32(buffer, offset);
                case NmsValueType.Int64:
                    return BitConverter.ToInt64(buffer, offset);
                case NmsValueType.UInt8:
                    return buffer[0];
                case NmsValueType.UInt16:
                    return BitConverter.ToUInt16(buffer, offset);
                case NmsValueType.UInt32:
                    return BitConverter.ToInt32(buffer, offset);
                case NmsValueType.UInt64:
                    return BitConverter.ToUInt64(buffer, offset);
                case NmsValueType.Real32:
                    return BitConverter.ToSingle(buffer, offset);
                case NmsValueType.Real64:
                    return BitConverter.ToDouble(buffer, offset);
                case NmsValueType.String:
                    return Encoding.Default.GetString(
                        buffer.Skip(offset).Take(NmsMaxDataLength - 1).TakeWhile(b => b != 0).ToArray());
                case NmsValueType.DateTime:
                    return GetDateTime(buffer, offset);
                case NmsValueType.Array:
                    int length = Math.Min((int)buffer[offset], NmsMaxDataLength - 1);
                    return buffer.Skip(offset + 1).Take(length).ToArray();
                default:
                    throw new ArgumentException("Invalid ValueType");
            }
        }

        protected static byte[] WriteValue(NmsValueType valueType, object value)
        {
            Contract.Requires(value != null);
            var data = new List<byte>(NmsMaxDataLength) { (byte)valueType };
            switch (valueType)
            {
                case NmsValueType.Boolean:
                    data.Add((bool)value ? (byte)1 : (byte)0);
                    break;
                case NmsValueType.Int8:
                    data.Add((byte)(sbyte)value);
                    break;
                case NmsValueType.Int16:
                    data.AddRange(BitConverter.GetBytes((short)value));
                    break;
                case NmsValueType.Int32:
                    data.AddRange(BitConverter.GetBytes((int)value));
                    break;
                case NmsValueType.Int64:
                    data.AddRange(BitConverter.GetBytes((long)value));
                    break;
                case NmsValueType.UInt8:
                    data.Add((byte)value);
                    break;
                case NmsValueType.UInt16:
                    data.AddRange(BitConverter.GetBytes((ushort)value));
                    break;
                case NmsValueType.UInt32:
                    data.AddRange(BitConverter.GetBytes((uint)value));
                    break;
                case NmsValueType.UInt64:
                    data.AddRange(BitConverter.GetBytes((ulong)value));
                    break;
                case NmsValueType.Real32:
                    data.AddRange(BitConverter.GetBytes((float)value));
                    break;
                case NmsValueType.Real64:
                    data.AddRange(BitConverter.GetBytes((double)value));
                    break;
                case NmsValueType.String:
                    data.AddRange(Encoding.Default.GetBytes((string)value).Take(NmsMaxDataLength - 1));
                    if (((string)value).Length < NmsMaxDataLength - 1)
                    {
                        data.Add(0);
                    }

                    break;
                case NmsValueType.DateTime:
                    var dt = (DateTime)value;
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
                                (byte)(dt.Millisecond/100 & 0x0f),
                                PackByte(dt.Millisecond%100),
                                (byte)(dt.DayOfWeek + 1)
                            });
                    break;
                case NmsValueType.Array:
                    var nmsValue = (byte[])value;
                    if (nmsValue.Length > NmsMaxDataLength - 1)
                    {
                        throw new ArgumentException("Invalid array length");
                    }

                    data.Add((byte)nmsValue.Length);
                    data.AddRange(nmsValue);
                    break;
                default:
                    throw new ArgumentException("Invalid ValueType");
            }

            return data.ToArray();
        }

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
            Contract.Requires(nmsData != null);
            Contract.Requires(nmsData.Length <= NmsMaxDataLength);
            Contract.Ensures(IsResponce == false);
            Contract.Ensures(Datagram != null);
            int nmsLength = Math.Min(nmsData.Length, NmsMaxDataLength);
            var data = new byte[NmsHeaderLength + nmsLength];
            Array.Copy(nmsData, data, nmsLength);

            data[0] = (byte)(((byte)service << 3) | ((id & 1023) >> 8));
            data[1] = (byte)(id & 0xFF);
            data[2] = (byte)((isResponsible ? 0x80 : 0) | (r1 ? 0x40 : 0) | nmsLength & 0x3F);

            Datagram = new NibusDatagram(source, destanation, priority, ProtocolType.Nms, data);
        }

        private static byte PackByte(int b)
        {
            b %= 100;
            return (byte)(b/10*16 + b%10);
        }

        private static int UnpackByte(byte b)
        {
            return (b & 0x0f) + (b >> 4)*10;
        }

        private static DateTime GetDateTime(byte[] buffer, int startIndex)
        {
            int day = UnpackByte(buffer[0 + startIndex]);
            int month = UnpackByte(buffer[1 + startIndex]);
            int year = UnpackByte(buffer[3 + startIndex]) + UnpackByte(buffer[2 + startIndex])*100;
            int hour = UnpackByte(buffer[4 + startIndex]);
            int minute = UnpackByte(buffer[5 + startIndex]);
            int second = UnpackByte(buffer[6 + startIndex]);
            int ms = UnpackByte(buffer[8 + startIndex]) + (buffer[7 + startIndex] & 0x0f)*100;
            return new DateTime(year, month, day, hour, minute, second, ms);
        }

        public static int GetSizeOf(NmsValueType vt, object value)
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
                    return Math.Min(NmsMaxDataLength - 1, ((string)value).Length + 1);
                case NmsValueType.Array:
                    return Math.Min(NmsMaxDataLength - 1, ((byte[])value).Length);
                default:
                    throw new ArgumentException("Invalid value type");
            }
        }
    }
}