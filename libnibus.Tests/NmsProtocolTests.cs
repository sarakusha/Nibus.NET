//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsProtocolTests.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
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
        private SerialTransport _serial;
        private NibusDataCodec _nibusDataCodec;
        private NmsCodec _nmsCodec;
        private List<IDisposable> _releaser;

        [TestFixtureSetUp]
        public void BuildStack()
        {
            _serial = new SerialTransport("COM7", 115200);
            _nibusDataCodec = new NibusDataCodec();
            _nmsCodec = new NmsCodec();
            _releaser = new List<IDisposable> { _nmsCodec, _nibusDataCodec, _serial };

            _nmsCodec.ConnectTo(_nibusDataCodec);
            _nibusDataCodec.ConnectTo(_serial);
            _serial.Run();
        }

        [TestFixtureTearDown]
        public void ReleaseStack()
        {
            foreach (var disposable in _releaser)
            {
                disposable.Dispose();
            }
        }

        private void ReadValueAsync_ThrowTimeout()
        {
            var nmsController = _nmsCodec.Protocol;
            try
            {
                var version = nmsController.ReadValueAsync(Address.CreateMac(0x1), 2).Result;
            }
            catch (AggregateException e)
            {
                throw e.Flatten().InnerException;
            }
        }

        [Test]
        public void NmsController_ReadValueAsync_ThrowTimeout()
        {
            Assert.Throws<TimeoutException>(ReadValueAsync_ThrowTimeout);
        }

        [Test]
        public void NmsController_ReadValueAsync_Version()
        {
            var nmsController = _nmsCodec.Protocol;
            var version = nmsController.ReadValueAsync(Address.CreateMac(0x20, 0x44), 2).Result;
            Assert.That(version, Is.EqualTo(0x00070200));
        }

        [Test]
        public void RawRead()
        {
            var readVersion = new NmsRead(Address.CreateMac(0x20, 0x44), 2);
            _nmsCodec.Encoder.Post(readVersion);
            var response = _nmsCodec.Decoder.Receive(TimeSpan.FromSeconds(1));
            Assert.That(response.Id == 2);
            Assert.That(response.IsResponse);
            Assert.That(response.ServiceType == NmsServiceType.Read);
        }

        [Test]
        public void ExcTest()
        {
            IList<int> list;
            var tb = new TransformBlock<int, int>(i => 1000 / (i-5));
            var bb = new BufferBlock<int>();
            tb.LinkTo(bb);
            try
            {
                for (int i = 10; i > 0; i--)
                {
                    tb.Post(i);
                }
                if (bb.OutputAvailableAsync().Result)
                {
                    bb.TryReceiveAll(out list);
                    var c = list.Count;
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}