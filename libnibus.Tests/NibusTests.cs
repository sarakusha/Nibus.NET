//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NibusTests.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;
using NUnit.Framework;
using NataInfo.Nibus.Nms;

#endregion

namespace NataInfo.Nibus.Tests
{
    [TestFixture]
    public class NibusTests
    {
        [Test]
        public void BuildSerialStack()
        {

            using (var serial = new SerialTransport("COM7", 115200))
            {
                serial.RunAsync();
                var stack = Nibus.BuildNibusStack(serial);
                var nmsProtocol = new NmsProtocol();
                stack.LinkTo(nmsProtocol, datagram => datagram.Protocol == nmsProtocol.Protocol);

                var readVersion = new NmsRead(Address.Empty, new Address(new byte[] { 0x20, 0x44 }), 2);
                nmsProtocol.Encoder.Post(readVersion);
                var resp = nmsProtocol.Decoder.Receive(TimeSpan.FromSeconds(3));
                Assert.That(resp.Id == 2);
            }
        }


    }
}
