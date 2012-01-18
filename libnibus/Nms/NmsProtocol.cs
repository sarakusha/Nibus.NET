using System.Diagnostics.Contracts;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus.Nms
{
    public class NmsProtocol : NibusCodec<NibusDatagram, NmsMessage>, INibusProtocol<NibusDatagram, NmsMessage>
    {
        private readonly BroadcastTransformBlock<NibusDatagram, NmsMessage> _decoder;
        private NmsController _controller;
        public NmsProtocol()
        {
            Contract.Ensures(Decoder != null);
            Contract.Ensures(Encoder != null);
            _decoder = new BroadcastTransformBlock<NibusDatagram, NmsMessage>(NmsMessage.CreateFrom);
            Decoder = _decoder;
            Encoder = new TransformBlock<NmsMessage, NibusDatagram>(m => m.Datagram);
        }

        #region Implementation of INibusProtocol

        public ProtocolType Protocol
        {
            get { return ProtocolType.Nms; }
        }

        #endregion

        public NmsController Controller
        {
            get
            {
                return _controller ??
                       (_controller =
                        new NmsController(_decoder.Source, Encoder)
                            {
                                ResetIncoming = () => _decoder.ResetSource(NmsEmptyMessage.Instance)
                            });
            }
        }

        public override System.IDisposable ConnectTo<T>(INibusCodec<T, NibusDatagram> bottomCodec)
        {
            return LinkTo(bottomCodec, datagram => datagram.Protocol == Protocol);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_controller != null)
            {
                _controller.Dispose();
                _controller = null;
            }
        }
    }
}