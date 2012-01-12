using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NataInfo.Nibus
{
    public interface INibusProtocol<TEncoded, TDecoded> : INibusCodec<TEncoded, TDecoded>
    {
        ProtocolType Protocol { get; }
    }
}
