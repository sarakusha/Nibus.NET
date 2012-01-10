using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NataInfo.Nibus
{
    public interface INibusProtocol : INibusEndpoint<NibusDatagram, NibusDatagram>
    {
        ProtocolType Protocol { get; }
    }
}
