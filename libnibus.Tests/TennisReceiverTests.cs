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
        private NmsProtocol _nmsProtocol;
        private List<IDisposable> _releaser;

        [TestFixtureSetUp]
        public void BuildStack()
        {
            _serial = new SerialTransport("COM7", 115200);
            _nibusDataCodec = new NibusDataCodec();
            _nmsProtocol = new NmsProtocol();
            _releaser = new List<IDisposable> { _nmsProtocol, _nibusDataCodec, _serial };

            _nmsProtocol.ConnectTo(_nibusDataCodec);
            _nibusDataCodec.ConnectTo(_serial);
            _serial.RunAsync();
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
            var tennis = new TennisReceiver(_nmsProtocol.Controller);
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
