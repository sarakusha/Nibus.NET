//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// TennisReceiverTests.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Sport;

#endregion

namespace NataInfo.Nibus.Tests
{
    [TestFixture]
    public class TennisReceiverTests
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

        [Test]
        public void TenisStat()
        {
            var tennis = new TennisReceiver(_nmsCodec.Protocol);
            bool s = false;
            var h = new AutoResetEvent(false);
            tennis.TennisStatChanged += (o, e) =>
                                            {
                                                s = true;
                                                h.Set();
                                            };
            h.WaitOne(TimeSpan.FromMinutes(1));
            Assert.That(s);
        }
    }
}
