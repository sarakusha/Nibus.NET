using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace NataInfo.Nibus
{
    public enum ProtocolType
    {
        None = 0,
        Nms = 1,
        Sarp = 2
    }

    public enum PriorityType
    {
		Realtime = 0,
		AboveNormal = 1,
		Normal = 2,
		BelowNormal = 3
	}

    public interface INibusDatagram
    {
        Address Destanation { get; }
        Address Source { get; }
        PriorityType Priority { get; }
        ProtocolType Protocol { get; }
        ReadOnlyCollection<byte> Data { get; }
    }

    [Pure]
    public class NibusDatagram : INibusDatagram
    {
        public const int MaxDataLength = 238;

        public NibusDatagram(Address source, Address destanation, PriorityType priority, ProtocolType protocol, IList<byte> data)
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

            Destanation = destanation;
            Source = source;
            Protocol = protocol;
            Data = new ReadOnlyCollection<byte>(data);
            Priority = priority;
        }

        [Pure]
        public Address Destanation { get; private set; }
        [Pure]
        public Address Source { get; private set; }
        [Pure]
        public PriorityType Priority { get; private set; }
        [Pure]
        public ProtocolType Protocol { get; private set; }
        [Pure]
        public ReadOnlyCollection<byte> Data { get; private set; }
    }
}
