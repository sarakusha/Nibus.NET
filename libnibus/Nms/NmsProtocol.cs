using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus.Nms
{
    public class NmsProtocol : INibusProtocol
    {
        private readonly CancellationTokenSource _cts;
        private readonly TransformBlock<NibusDatagram, NmsMessage> _decoder;
        private readonly TransformManyBlock<IList<NmsMessage>, NibusDatagram> _encoder;

        public NmsProtocol()
        {
            _cts = new CancellationTokenSource();
            var options = new ExecutionDataflowBlockOptions { CancellationToken = _cts.Token };
            _decoder = new TransformBlock<NibusDatagram, NmsMessage>(d => NmsMessage.CreateFrom(d), options);
            _encoder = new TransformManyBlock<IList<NmsMessage>, NibusDatagram>(m => Encode(m), options);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _cts.Cancel();
        }

        #endregion

        #region Implementation of INibusEndpoint<in NibusDatagram,out NibusDatagram>

        public ITargetBlock<NibusDatagram> IncomingMessages
        {
            get { return _decoder; }
        }

        public ISourceBlock<NibusDatagram> OutgoingMessages
        {
            get { return _encoder; }
        }

        #endregion

        #region Implementation of INibusProtocol

        public ProtocolType Protocol
        {
            get { return ProtocolType.Nms; }
        }

        #endregion

        private static IEnumerable<NibusDatagram> Encode(IList<NmsMessage> nmsMessage)
        {
            var datagrams = new List<NibusDatagram>(nmsMessage.Count);
            var reading = nmsMessage.Where(msg => msg.ServiceType == NmsServiceType.Read)
                .GroupBy(msg => new { msg.Datagram.Source, msg.Datagram.Destanation }, msg => msg.Id);

            foreach (var readingGroup in reading)
            {
                if (readingGroup.Count() == 1)
                {
                    var datagram = new NibusDatagram(
                        readingGroup.Key.Source,
                        readingGroup.Key.Destanation,
                        PriorityType.Normal,
                        ProtocolType.Nms,
                        )
                }
            }

            return datagrams;
        }
    }
}