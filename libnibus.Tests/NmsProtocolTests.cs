//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsProtocolTests.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using NUnit.Framework;
using NataInfo.Nibus.Nms;

#endregion

namespace NataInfo.Nibus.Tests
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class NmsProtocolTests
    {
        private static readonly Address Destanation = Address.CreateMac(0x20, 0x44);
        private NibusStack _stack;

        [TestFixtureSetUp]
        public void BuildStack()
        {
            _stack = new NibusStack(
                new SerialTransport("COM7", 115200, true),
                new NibusDataCodec(),
                new NmsCodec());
        }

        [TestFixtureTearDown]
        public void ReleaseStack()
        {
            using (_stack)
            {
                _stack = null;
            }
        }

        private void ReadValueAsync_ThrowTimeout()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            try
            {
                nmsProtocol.ReadValueAsync(Address.CreateMac(1,2,3,4,5,6), 2, 5).Wait();
            }
            catch (AggregateException e)
            {
                throw e.Flatten().InnerException;
            }
        }

        [Test]
        public void NmsProtocol_ReadValueAsync_ThrowTimeout()
        {
            var sw = new Stopwatch();
            sw.Start();
            Assert.Throws<TimeoutException>(ReadValueAsync_ThrowTimeout);
            sw.Stop();
            Assert.That(sw.ElapsedMilliseconds, Is.InRange(5000, 5000+900));
        }

        [Test]
        public void NmsProtocol_Ping_MinusOne()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            Assert.That(nmsProtocol.Ping(Address.CreateMac(1,2,3,4,5,6)), Is.EqualTo(-1));
        }

        [Test]
        public void NmsProtocol_ReadValueAsync_Version()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            var version = nmsProtocol.ReadValueAsync(Destanation, 2).Result;
            Assert.That(version, Is.EqualTo(0x00070200));
        }

        [Test]
        public void RawRead()
        {
            var readVersion = new NmsRead(Destanation, 2);
            var nmsCodec = _stack.GetCodec<NmsCodec>();
            nmsCodec.Encoder.Post(readVersion);
            var response = nmsCodec.Decoder.Receive(TimeSpan.FromSeconds(1));
            Assert.That(response.Id == 2);
            Assert.That(response.IsResponse);
            Assert.That(response.ServiceType == NmsServiceType.Read);
        }

        // Отключить логи!
        [Test]
        public void SpeedTest([Values(21)] int attempts)
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            var pings = new List<long>(attempts);
            for (int i = 0; i < attempts; i++)
            {
                pings.Add(nmsProtocol.Ping(Destanation));
            }
            pings.Sort();
            var count = pings.Count;
            var index = count/2;
            var mediana = count%2 == 0 ? ((int)(pings[index] + pings[index - 1] + 0.5))/2 : pings[index];
            Assert.That(mediana, Is.LessThanOrEqualTo(16));
        }

        [Test]
        [Repeat(3)]
        public void NmsProtocol_LastMessageDublicate_ThrowTimeout()
        {
            Assert.Throws<TimeoutException>(LastMessageDublicate_ThrowTimeout);
        }

        [Test]
        public void NmsProtocol_ReadMany()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            var test = new List<ReadProgressInfo>(2);
            var progress = new Progress<ReadProgressInfo>(test.Add);
            nmsProtocol.ReadManyValuesAsync(progress, Destanation, 2, 3, 0x7f, 0x100, 0x101, 0x102, 0x103,
                0x104, 0x105, 0x106, 0x107, 0x108, 0x109, 0x10a, 0x10c, 0x10b, 0x10d).Wait();
            Assert.That(test.All(info => !info.IsFaulted));

        }

        private void LastMessageDublicate_ThrowTimeout()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            NmsMessage lastMessage;
            nmsProtocol.IncomingMessages.TryReceive(null, out lastMessage);
            var query = new NmsRead(Destanation, 2);
            nmsProtocol.OutgoingMessages.Post(query);
            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            NmsMessage response, response2;
            using (nmsProtocol.IncomingMessages.LinkTo(
                wob, m => !ReferenceEquals(m, lastMessage) && m.IsResponse && m.ServiceType == query.ServiceType && m.Id == query.Id))
            {
                response = wob.Receive(nmsProtocol.Timeout);
            }
            nmsProtocol.IncomingMessages.TryReceive(null, out lastMessage);
            Assert.That(lastMessage, Is.EqualTo(response));
            var wob2 = new WriteOnceBlock<NmsMessage>(m => m);
            using (nmsProtocol.IncomingMessages.LinkTo(
                wob2, m => !ReferenceEquals(m, lastMessage) && m.IsResponse && m.ServiceType == query.ServiceType && m.Id == query.Id))
            {
                response2 = wob2.Receive(nmsProtocol.Timeout);
            }
        }
    }
}