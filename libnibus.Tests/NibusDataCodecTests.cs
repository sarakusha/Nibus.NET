using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus.Tests
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class NibusDataCodecTests
    {
        [Test]
        public void EncodeDecode_Equal(
            [ValueSource("Addresses")] Address destanation,
            [ValueSource("Addresses")] Address source,
            [Values(PriorityType.Realtime, PriorityType.AboveNormal, PriorityType.Normal, PriorityType.BelowNormal)]
            PriorityType priority,
            [Values(ProtocolType.Nms, ProtocolType.Sarp)] ProtocolType protocol)
        {
            var codec = new NibusDataCodec();
            var datagramOrig = new NibusDatagram(codec, source, destanation, priority,
                                                 protocol, new byte[] {1, 2, 3, 4, 6, 7, 8, 9});
            codec.Encoder.Post(datagramOrig);
            var data = codec.Encoder.Receive();
            codec.Decoder.Post(data);
            var datagramCopy = codec.Decoder.Receive();
            Assert.That(datagramOrig.Destanation == datagramCopy.Destanation);
            Assert.That(datagramOrig.Source == datagramCopy.Source);
            Assert.That(datagramOrig.Priority == datagramCopy.Priority);
            Assert.That(datagramOrig.ProtocolType == datagramCopy.ProtocolType);
            Assert.That(datagramOrig.Data.SequenceEqual(datagramCopy.Data));
        }

        public static IEnumerable<Address> Addresses()
        {
            return new[]
            {
                new Address(255, 255, 1),
                new Address(1, 2),
                new Address(new byte[] {0x10, 0x20, 0x30}),
                Address.Empty,
                Address.Broadcast
            };
        }
    }
}
