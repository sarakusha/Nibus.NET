using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using NUnit.Framework;

namespace NataInfo.Nibus.Tests
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class SerialTransportTests
    {
        [Test, Ignore]
        public void SerialCodec_RunAsync_ReceiveNotNull()
        {
            using (var serial = new SerialTransport("COM4", 9600))
            {
                serial.RunAsync();
                var data = serial.IncomingMessages.Receive();
                Assert.That(data, Is.Not.Null);
            }
        }

        [Test, Ignore]
        public void SerialCodec_Send()
        {
            using (var serial = new SerialTransport("COM4", 9600))
            {
                serial.RunAsync();
                var b = serial.OutgoingMessages.Post(Encoding.Default.GetBytes("Привет! Hello!"));
                Assert.That(b);
            }
        }
    }
}
