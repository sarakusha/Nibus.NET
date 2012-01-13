using System.Diagnostics.Contracts;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    public class BroadcastCodec<TData> : NibusCodec<TData, TData>, INibusEndpoint<TData>
    {
        public BroadcastCodec()
        {
            Contract.Ensures(Decoder != null);
            Contract.Ensures(Encoder != null);
            Decoder = new BroadcastBlock<TData>(d => d);
            Encoder = new BufferBlock<TData>();
        }
    }
}
