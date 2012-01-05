using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NataInfo.Nibus
{
    public enum ProtocolType
    {
        None = 0,
        Nms = 1,
        Sarp = 2
    }

    public interface INibusDatagram
    {
        Address Destanation { get; }
        Address Source { get; }
        int Priority { get; set; }
        ProtocolType Protocol { get; }
        ReadOnlyCollection<byte> Data { get; }
    }

    public class NibusDatagram : INibusDatagram
    {
        public const int MaxDataLength = 238;

        public NibusDatagram(Address destanation, Address source, int priority, ProtocolType protocol, IList<byte> data)
        {
            if (data == null || data.Count > MaxDataLength)
            {
                throw new ArgumentException("Invalid data");
            }

            Destanation = destanation;
            Source = source;
            Protocol = protocol;
            Data = new ReadOnlyCollection<byte>(data);
            Priority = priority;
        }

        #region Implementation of INibusDatagram

        public Address Destanation { get; private set; }
        public Address Source { get; private set; }
        public int Priority { get; set; }
        public ProtocolType Protocol { get; private set; }
        public ReadOnlyCollection<byte> Data { get; private set; }

        #endregion
    }
}
