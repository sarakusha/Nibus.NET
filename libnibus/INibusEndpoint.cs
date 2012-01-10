using System;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    public interface INibusEndpoint<in TIncoming, out TOutcoming> : IDisposable where TOutcoming : TIncoming
    {
        ITargetBlock<TIncoming> IncomingMessages { get; }
        ISourceBlock<TOutcoming> OutgoingMessages { get; }
    }
}
