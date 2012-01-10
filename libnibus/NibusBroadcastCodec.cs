using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    public class NibusBroadcastCodec : INibusCodec<NibusDatagram, NibusDatagram>
    {
        private readonly CancellationTokenSource _cts;
        private readonly BroadcastBlock<NibusDatagram> _decoder;
        private readonly BufferBlock<NibusDatagram> _encoder;

        public NibusBroadcastCodec()
        {
            _cts = new CancellationTokenSource();
            var options = new DataflowBlockOptions {CancellationToken = _cts.Token};
            _decoder = new BroadcastBlock<NibusDatagram>(d => d, options);
            _encoder = new BufferBlock<NibusDatagram>(options);
        }
        #region Implementation of IDisposable

        public void Dispose()
        {
            _cts.Cancel();
        }

        #endregion

        #region Implementation of INibusCodec<NibusDatagram,NibusDatagram>

        public IPropagatorBlock<NibusDatagram, NibusDatagram> Encoder
        {
            get { return _encoder; }
        }

        public IPropagatorBlock<NibusDatagram, NibusDatagram> Decoder
        {
            get { return _decoder; }
        }

        #endregion
    }
}
