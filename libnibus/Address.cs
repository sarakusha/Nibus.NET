using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NataInfo.Nibus
{
    /// <summary>
    /// Вид адреса хранящегося в переменной типа <see cref="Address"/>
    /// </summary>
    /// <seealso cref="Address.Type"/>
    public enum AddressType
    {
        /// <summary>
        /// "Пустой" адрес <c>::0</c>.
        /// </summary>
        Empty = 0,
        /// <summary>
        /// Аппаратный адрес (MAC), ID устройства.
        /// </summary>
        /// <example>FF::12:34</example>
        Hardware,
        /// <summary>
        /// Сетевой адрес (Домен.Подсеть.Устройство).
        /// </summary>
        /// <example>255.255.1</example>
        Net,
        /// <summary>
        /// Групповой адрес (Домен.Группа).
        /// </summary>
        /// <example>1.2</example>
        Group,
        /// <summary>
        /// Широковещательный адрес. <c>FF:FF:FF:FF:FF:FF</c>
        /// </summary>
        Broadcast
    };


    /// <summary>
    /// Неизменяемый (immutable) класс представляющий адрес в сети NiBUS.
    /// </summary>
    public sealed class Address
    {
        /// <summary>
        /// Пустой адрес.
        /// </summary>
        public static readonly Address Empty = new Address();
        /// <summary>
        /// Широковещательный адрес.
        /// </summary>
        public static readonly Address Broadcast = new Address(new byte[] {255, 255, 255, 255, 255, 255});

        /// <summary>
        /// Количество байт в MAC-адресе.
        /// </summary>
        public const int MACLength = 6;

        private readonly byte[] _mac;
        private readonly ushort _domain;
        private readonly byte _subnet;
        private readonly byte _group;
        private readonly ushort _device;
        private string _value;

        /// <summary>
        /// Считывает адрес из буфера NiBUS.
        /// </summary>
        /// <param name="buffer">Буфер NiBUS хранящий адрес.</param>
        /// <param name="offset">Смещение в буфере.</param>
        /// <returns>Экземпляр класса <see cref="Address"/> инициализированный данными из буфера.</returns>
        /// <remarks>Размер буфера должен быть не меньше <see cref="Length"/> + <see cref="offset"/>.</remarks>
        public static Address ReadFrom(byte[] buffer, int offset)
        {
            Contract.Requires(buffer.Length - offset >= MACLength + 1);

            switch(buffer[offset])
            {
                case 0:
                    var mac = new byte[MACLength];
                    Array.Copy(buffer, offset + 1, mac, 0, MACLength);
                    return new Address(mac);
                case 1:
                    var netDomain = BitConverter.ToUInt16(buffer, offset + 1);
                    var subnet = buffer[offset + 3];
                    var device = BitConverter.ToUInt16(buffer, offset + 4);
                    return new Address(netDomain, subnet, device);
                case 2:
                    var groupDomain = BitConverter.ToUInt16(buffer, offset + 1);
                    var group = buffer[offset + 3];
                    return new Address(groupDomain, group);
                default:
                    throw new ArgumentException("Invalid Address.Type");
            }
        }

        public static Address CreateMac(params byte[] mac)
        {
            return new Address(mac);
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="Address"/> содержащий пустой адрес.
        /// </summary>
        /// <seealso cref="Address.Empty"/>
        public Address()
        {
            Type = AddressType.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class.
        /// </summary>
        /// <param name="address">Строковое представление адреса.</param>
        /// <exception cref="ArgumentException">Неверный формат строки с адресом.</exception>
        public Address(string address)
        {
            if (String.IsNullOrWhiteSpace(address))
            {
                Type = AddressType.Empty;
                return;
            }

            try
            {
                if (address.IndexOf('.') != -1)
                {
                    char[] delimiter = { '.' };
                    var split = address.Split(delimiter, 4);
                    char[] hex = { 'x', 'a', 'b', 'c', 'd', 'e', 'f', 'X', 'A', 'B', 'C', 'D', 'E', 'F' };

                    var fromBase = address.IndexOfAny(hex) != -1 ? 16 : 10;
                    switch (split.Length)
                    {
                        case 2:
                            Type = AddressType.Group;
                            _domain = Convert.ToUInt16(split[0], fromBase);
                            _group = Convert.ToByte(split[1], fromBase);
                            break;
                        case 3:
                            Type = AddressType.Net;
                            _domain = Convert.ToUInt16(split[0], fromBase);
                            _subnet = Convert.ToByte(split[1], fromBase);
                            _device = Convert.ToUInt16(split[2], fromBase);
                            break;
                        default:
                            throw new ArgumentException();
                    }
                }
                else
                {
                    var mac = new byte[MACLength];
                    char[] delimiter = { ':' };
                    var sep = address.IndexOf("::", StringComparison.Ordinal);
                    if (sep == -1)
                    {
                        var split = address.Split(delimiter, MACLength + 1);
                        if (split.Length != MACLength)
                        {
                            throw new ArgumentException();
                        }

                        for (var i = 0; i < MACLength; i++)
                            mac[i] = Convert.ToByte(split[i], 16);
                    }
                    else
                    {
                        if (sep + 2 < address.Length && address.IndexOf("::", sep + 2, StringComparison.Ordinal) != -1)
                        {
                            throw new ArgumentException();
                        }

                        var left = address.Substring(0, sep).Split(delimiter);
                        var right = address.Substring(sep + 2).Split(delimiter);
                        if (left.Length + right.Length > MACLength)
                        {
                            throw new ArgumentException();
                        }

                        for (var i = 0; i < left.Length; i++)
                        {
                            mac[i] = left[i].Length != 0 ? Convert.ToByte(left[i], 16) : (byte)0;
                        }

                        var j = 0;
                        for (var i = MACLength - right.Length; i < MACLength; j++, i++)
                        {
                            mac[i] = right[j].Length != 0 ? Convert.ToByte(right[j], 16) : (byte)0;
                        }
                    }

                    if (mac.All(b => b == 0))
                    {
                        Type = AddressType.Empty;
                    }
                    else if (mac.All(b => b == 255))
                    {
                        Type = AddressType.Broadcast;
                    }
                    else
                    {
                        Type = AddressType.Hardware;
                        _mac = mac;
                    }
                }
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException("Invalid value specified", e);
            }
            catch (FormatException e)
            {
                throw new ArgumentException("Invalid value specified", e);
            }
            catch (OverflowException e)
            {
                throw new ArgumentException("Out-of-bounds", e);
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Address"/> содержащий "групповой" адрес.
        /// </summary>
        /// <param name="domain">Домен.</param>
        /// <param name="group">Группа.</param>
        public Address(ushort domain, byte group)
        {
            _domain = domain;
            _group = group;
            Type = AddressType.Group;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Address"/> содержащий "сетевой" адрес.
        /// </summary>
        /// <param name="domain">Домен.</param>
        /// <param name="subnet">Подсеть.</param>
        /// <param name="device">Устройство.</param>
        public Address(ushort domain, byte subnet, ushort device)
        {
            _domain = domain;
            _subnet = subnet;
            _device = device;
            Type = AddressType.Net;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Address"/> содержащий MAC-адрес,
        /// "пустой" адрес при всех значениях равных 0 или пустом массиве,
        /// "широковещательный" адрес при всех значениях равных 255.
        /// </summary>
        /// <param name="mac">
        /// Массив байт длиной не более <see cref="Address.MACLength"/>.
        /// Если длина меньше, в начало добавляются 0 байты.
        /// </param>
        public Address(byte[] mac)
        {
            if (mac.Length > MACLength)
            {
                throw new ArgumentException();
            }

            if (mac.Length == MACLength && mac.All(b => b == 255))
            {
                Type = AddressType.Broadcast;
                return;
            }

            if (mac.Length == 0 || mac.All(b => b == 0))
            {
                Type = AddressType.Empty;
                return;
            }

            _mac = new byte[MACLength];
            var delta = MACLength - mac.Length;
            Array.Copy(mac, 0, _mac, delta, MACLength - delta);
            Type = AddressType.Hardware;
        }

        /// <summary>
        /// Тип адреса.
        /// </summary>
        /// <seealso cref="AddressType"/>
        public AddressType Type { get; private set; }

        /// <summary>
        /// Возвращает строковое представление адреса.
        /// </summary>
        public string Value
        {
            get { return _value ?? (_value = ToString()); }
        }

        /// <summary>
        /// Возвращает значение байта по заданному индексу для MAC-адреса.
        /// </summary>
        /// <seealso cref="AddressType.Hardware"/>
        /// <exception cref="InvalidOperationException">Тип адреса <see cref="Type"/> не является <see cref="AddressType.Hardware"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">Индекс вне диапазона 0..<see cref="Address.MACLength"/></exception>
        public byte this[int index]
        {
            get
            {
                if (_mac == null)
                {
                    throw new InvalidOperationException("Class must be Address.Class.Hardware");
                }
                
                if (index < 0 || MACLength <= index)
                {
                    throw new ArgumentOutOfRangeException("index", "Индекс должен находится в диапазоне 0..5");
                }

                return _mac[index];
            }
        }

        /// <summary>
        /// Возвращает домен.
        /// </summary>
        /// <exception cref="InvalidOperationException">Тип адреса <see cref="Type"/> не является
        /// <see cref="AddressType.Net"/> или <see cref="AddressType.Group"/></exception>
        public ushort Domain
        {
            get
            {
                if (Type != AddressType.Net && Type != AddressType.Group)
                {
                    throw new InvalidOperationException();
                }

                return _domain;
            }
        }

        /// <summary>
        /// Возвращает группу.
        /// </summary>
        /// <exception cref="InvalidOperationException">Тип адреса <see cref="Type"/> не является <see cref="AddressType.Group"/></exception>
        public byte Group
        {
            get
            {
                if (Type != AddressType.Group)
                {
                    throw new InvalidOperationException();
                }

                return _group;
            }
        }

        /// <summary>
        /// Возвращает подсеть.
        /// </summary>
        /// <exception cref="InvalidOperationException">Тип адреса <see cref="Type"/> не является <see cref="AddressType.Net"/></exception>
        public byte Subnet
        {
            get
            {
                if (Type != AddressType.Net)
                {
                    throw new InvalidOperationException();
                }

                return _subnet;
            }
        }

        /// <summary>
        /// Возвращает номер устройства в "сетевом" адресе.
        /// </summary>
        /// <exception cref="InvalidOperationException">Тип адреса <see cref="Type"/> не является <see cref="AddressType.Net"/></exception>
        public ushort Device
        {
            get
            {
                if (Type != AddressType.Net)
                {
                    throw new InvalidOperationException();
                }

                return _device;
            }
        }

        /// <summary>
        /// Возвращает размер буфера для хранения адреса.
        /// </summary>
        public static int Length
        {
            get
            {
                return MACLength + 1;
            }
        }

        /// <summary>
        /// Сохраняет себя в буфере NiBUS.
        /// </summary>
        /// <param name="buffer">Буфер.</param>
        /// <param name="offset">Смещение.</param>
        /// <returns>Количество записанных байт. Всегда равно <see cref="Length"/></returns>
        public int Write(byte[] buffer, int offset)
        {
            switch (Type)
            {
                case AddressType.Net:
                    var netData = BitConverter.GetBytes(Domain).Concat(new[] {Subnet})
                        .Concat(BitConverter.GetBytes(Device)).ToArray();
                    WriteData(buffer, offset, 1, netData);
                    break;
                case AddressType.Group:
                    var groupData = BitConverter.GetBytes(Domain).Concat(new[] {Group}).ToArray();
                    WriteData(buffer, offset, 2, groupData);
                    break;
                case AddressType.Empty:
                    WriteData(buffer, offset, 0, new byte[0]);
                    break;
                case AddressType.Broadcast:
                    WriteData(buffer, offset, 0, new byte[] {255, 255, 255, 255, 255, 255});
                    break;
                case AddressType.Hardware:
                    WriteData(buffer, offset, 0, _mac);
                    break;
                default:
                    throw new ArgumentException("Invalid Address.Type");
            }

            return Length;
        }

        private void WriteData(byte[] buffer, int offset, byte type, byte[] data)
        {
            Contract.Assume(buffer.Length - offset > data.Length);
            var i = 0;
            buffer[offset + i++] = type;
            Array.Copy(data, 0, buffer, offset + i, data.Length);
            i += data.Length;
            Array.Clear(buffer, offset + i, Length - i);
        }

        public override int GetHashCode()
        {
            switch (Type)
            {
                case AddressType.Group:
                    return ((byte) Type << 24) + (Domain << 8) + Group;
                case AddressType.Net:
                    return ((byte) Type << 24) + ((Domain << 8) + Subnet) ^ (Device << 8);
                case AddressType.Hardware:
                    return ((byte) Type << 24) +
                           ((((_mac[5] << 8) + _mac[4]) << 8) + _mac[3]) ^ ((((_mac[2] << 8) + _mac[1]) << 8) + _mac[0]);
                case AddressType.Empty:
                    return 0;
                case AddressType.Broadcast:
                    return -1;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static bool operator ==(Address a1, Address a2)
        {
            if (ReferenceEquals(a1, a2))
            {
                return true;
            }
            
            if (ReferenceEquals(a1, null) || ReferenceEquals(a2, null))
            {
                return false;
            }

            if (a1.Type != a2.Type)
            {
                return false;
            }

            switch (a1.Type)
            {
                case AddressType.Group:
                    return a1.Domain == a2.Domain && a1.Group == a2.Group;
                case AddressType.Net:
                    return a1.Domain == a2.Domain && a1.Subnet == a2.Subnet && a1.Device == a2.Device;
                case AddressType.Hardware:
                    for (var i = 0; i < MACLength; i++)
                    {
                        if (a1[i] != a2[i])
                        {
                            return false;
                        }
                    }

                    return true;
            }
            
            return true;
        }
        
        public static bool operator !=(Address a1, Address a2)
        {
            return !(a1 == a2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!(obj is Address))
            {
                return false;
            }

            return this == (Address)obj;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case AddressType.Empty:
                    return "::0";
                case AddressType.Broadcast:
                    return "FF:FF:FF:FF:FF:FF";
                case AddressType.Group:
                    return String.Format("{0}.{1}", Domain, Group);
                case AddressType.Net:
                    return String.Format("{0}.{1}.{2}", Domain, Subnet, Device);
                case AddressType.Hardware:
                    Contract.Assume(_mac.Any(b => b != 0));
                    var str = String.Join(":", _mac.SkipWhile(b => b == 0).Select(b => b.ToString("X2")));
                    return _mac[0] == 0 ? "::" + str : str;
                default:
                    throw new InvalidOperationException();
            }
        }

    }
}
