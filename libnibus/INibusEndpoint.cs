using System;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    public interface INibusEndpoint<TData> : IDisposable
    {
        IReceivableSourceBlock<TData> IncomingMessages { get; }
        ITargetBlock<TData> OutgoingMessages { get; }
    }
}
