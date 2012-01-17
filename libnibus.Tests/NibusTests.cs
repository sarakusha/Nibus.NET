//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NibusTests.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;
using NUnit.Framework;
using NataInfo.Nibus.Nms;

#endregion

namespace NataInfo.Nibus.Tests
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class NibusTests
    {
        [Test]
        public void BuildSerialStack()
        {

            using (var serial = new SerialTransport("COM7", 115200))
            {
                var nibusDataCodec = new NibusDataCodec();
                nibusDataCodec.LinkTo(serial);
                var nmsProtocol = new NmsProtocol();
                nmsProtocol.LinkTo(nibusDataCodec, datagram => datagram.Protocol == nmsProtocol.Protocol, true);

                serial.RunAsync();

                var readVersion = new NmsRead(Address.CreateMac(0x20, 0x44), 2);
                nmsProtocol.Encoder.Post(readVersion);
                var resp = nmsProtocol.Decoder.Receive(TimeSpan.FromSeconds(3));
                Assert.That(resp.Id == 2);
                Assert.That(resp.IsResponce);
                Assert.That(resp.ServiceType == NmsServiceType.Read);
            }
        }

        [Test]
        public void NmsController_ReadValueAsync_Version()
        {
            using (var serial = new SerialTransport("COM7", 115200))
            {
                var nibusDataCodec = new NibusDataCodec();
                nibusDataCodec.LinkTo(serial);
                var nmsProtocol = new NmsProtocol();
                nmsProtocol.LinkTo(nibusDataCodec, datagram => datagram.Protocol == nmsProtocol.Protocol, true);

                serial.RunAsync();

                var nmsController = nmsProtocol.Controller;
                var version = nmsController.ReadValueAsync(Address.CreateMac(0x20, 0x44), 2).Result;
                Assert.That(version, Is.EqualTo(0x00070200));
            }
        }

        [Test]
        public void NmsController_ReadValueAsync_ThrowTimeout()
        {
            Assert.Throws<TimeoutException>(ReadValueAsync_ThrowTimeout);
        }

        private void ReadValueAsync_ThrowTimeout()
        {
            using (var serial = new SerialTransport("COM7", 115200))
            {
                var nibusDataCodec = new NibusDataCodec();
                nibusDataCodec.LinkTo(serial);
                var nmsProtocol = new NmsProtocol();
                nmsProtocol.LinkTo(nibusDataCodec, datagram => datagram.Protocol == nmsProtocol.Protocol, true);

                serial.RunAsync();

                var nmsController = nmsProtocol.Controller;
                try
                {
                    var version = nmsController.ReadValueAsync(Address.CreateMac(0x1), 2).Result;
                }
                catch (AggregateException e)
                {
                    throw e.Flatten().InnerException;
                }
            }
        }
    }
}
