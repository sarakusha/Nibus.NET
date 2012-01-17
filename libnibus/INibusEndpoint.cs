using System;
using System.Threading.Tasks.Dataflow;
using NataInfo.Nibus.Nms;

namespace NataInfo.Nibus
{
    public interface INibusEndpoint<TData>
    {
        IReceivableSourceBlock<TData> IncomingMessages { get; }
        ITargetBlock<TData> OutgoingMessages { get; }
    }
}
