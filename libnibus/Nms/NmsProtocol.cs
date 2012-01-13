using System.Diagnostics.Contracts;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus.Nms
{
    public class NmsProtocol : NibusCodec<NibusDatagram, NmsMessage>, INibusProtocol<NibusDatagram, NmsMessage>
    {
        public NmsProtocol()
        {
            Contract.Ensures(Decoder != null);
            Contract.Ensures(Encoder != null);
            Decoder = new TransformBlock<NibusDatagram, NmsMessage>(d => NmsMessage.CreateFrom(d));
            Encoder = new TransformBlock<NmsMessage, NibusDatagram>(m => m.Datagram);
        }

        #region Implementation of INibusProtocol

        public ProtocolType Protocol
        {
            get { return ProtocolType.Nms; }
        }

        #endregion
    }
}