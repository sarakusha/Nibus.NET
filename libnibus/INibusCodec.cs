using System;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    public interface INibusCodec<TEncoded, TDecoded> : IDisposable
    {
        IPropagatorBlock<TDecoded, TEncoded> Encoder { get; }
        IPropagatorBlock<TEncoded, TDecoded> Decoder { get; }
    }
}
