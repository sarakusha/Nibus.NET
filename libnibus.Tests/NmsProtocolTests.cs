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
using System.Threading.Tasks.Dataflow;
using System.Xml;
using NUnit.Framework;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Nms.Services;
using NataInfo.Nibus.Nms.Variables;

#endregion

namespace NataInfo.Nibus.Tests
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class NmsProtocolTests
    {
        private static readonly Address Destanation = Address.CreateMac(0x20, 0x44);
        private NibusStack _stack;
        private static readonly Address Vms50Address = Address.CreateMac(0x6c, 0xea);

        [TestFixtureSetUp]
        public void BuildStack()
        {
            Contract.ContractFailed += (sender, e) =>
            {
                e.SetHandled();
                e.SetUnwind(); //cause code to abort after event
                Assert.Fail(e.FailureKind.ToString() + ":" + e.Message);
            };

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
            var options = new NibusOptions { Attempts = 5, Timeout = TimeSpan.FromSeconds(1) };
            try
            {
                nmsProtocol.ReadValueAsync(Address.CreateMac(1, 2, 3, 4, 5, 6), 2, options).Wait();
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
            Assert.That(sw.ElapsedMilliseconds, Is.InRange(5000, 5300));
        }

        [Test]
        public void NmsProtocol_Ping_MinusOne()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            var sw = new Stopwatch();
            var options = new NibusOptions {Timeout = TimeSpan.FromSeconds(1)};
            sw.Start();
            Assert.That(nmsProtocol.Ping(Address.CreateMac(1,2,3,4,5,6), options), Is.EqualTo(-1));
            sw.Stop();
            Assert.That(sw.ElapsedMilliseconds, Is.InRange(options.Timeout.TotalMilliseconds, options.Timeout.TotalMilliseconds + 150));
        }

        [Test]
        public void NmsProtocol_Ping()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            //Для раскачки. Первый запрос длительный.
            nmsProtocol.ReadValueAsync(Destanation, 3).Wait();
            var ping = nmsProtocol.Ping(Destanation);
            Assert.That(ping, Is.InRange(10, 25));
        }

        [Test]
        public void NmsProtocol_ReadValueAsync_Version()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            var version = nmsProtocol.ReadValueAsync(Destanation, 2).Result;
            Assert.That(version, Is.EqualTo(0x00070200));
        }

        [Test, Ignore]
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
        [Ignore]
        public void NmsProtocol_LastMessageDublicate_ThrowTimeout()
        {
            Assert.Throws<TimeoutException>(LastMessageDublicate_ThrowTimeout);
        }

        [Test]
        public void NmsProtocol_ReadMany()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            var ids = GetMibIds(@"Z:\mibs\siolynx.mib.xsd");
            Assert.That(ids.Length, Is.GreaterThanOrEqualTo(17));
            var test = new List<ReadProgressInfo>(ids.Length);
            var options = new NibusOptions { Progress = new Progress<object>(pi => test.Add((ReadProgressInfo)pi)) };
            nmsProtocol.ReadManyValuesAsync(options, Destanation, ids).Wait();
            Assert.That(test.Select(pi => pi.IsFaulted), Is.All.EqualTo(false));
            Assert.That(test.Count, Is.EqualTo(ids.Length));
        }

        [Test]
        [Category("VMS50")]
        public void NmsProtocol_vms50_ReadMany()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            var ids = GetMibIds(@"Z:\mibs\vms50.mib.xsd");
            Assert.That(ids.Length, Is.GreaterThanOrEqualTo(66));
            Assert.That(ids.Distinct().Count(), Is.EqualTo(ids.Length));
            var test = new List<ReadProgressInfo>(ids.Length);
            var options = new NibusOptions { Progress = new Progress<object>(pi => test.Add((ReadProgressInfo)pi)) };
            nmsProtocol.ReadManyValuesAsync(options, Vms50Address, ids).Wait();
            Assert.That(test.Select(pi => pi.IsFaulted), Is.All.EqualTo(false));
            Assert.That(test.Count, Is.EqualTo(ids.Length));
            Assert.That(test.Select(pi => pi.Id).Distinct().Count(), Is.EqualTo(ids.Length));
        }

        [Test]
        [Category("VMS50")]
        public void NmsProtocol_vms50_Reset()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            nmsProtocol.ResetDeviceComfirmedAsync(Vms50Address).Wait();
        }

        [Test]
        [Category("VMS50")]
        public void NmsProtocol_vms50_ExecuteProgram_ResetStats()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            var rs485_rx_datagrams1 =
                Convert.ToUInt32(
                    nmsProtocol.ReadValueAsync(Vms50Address, (int)StdKernel.RS485RxDatagrams).Result);
            nmsProtocol.ExecuteProgramConfirmedAsync(Vms50Address, 4).Wait();
            Thread.Sleep(500);
            var rs485_rx_datagrams2 =
                Convert.ToUInt32(
                    nmsProtocol.ReadValueAsync(Vms50Address, (int)StdKernel.RS485RxDatagrams).Result);
            Assert.That(rs485_rx_datagrams2, Is.LessThan(rs485_rx_datagrams1));
        }
 
        private void LastMessageDublicate_ThrowTimeout()
        {
            var nmsProtocol = _stack.GetCodec<NmsCodec>().Protocol;
            var options = new NibusOptions();
            NmsMessage lastMessage;
            nmsProtocol.IncomingMessages.TryReceive(null, out lastMessage);
            var query = new NmsRead(Destanation, 2);
            nmsProtocol.OutgoingMessages.Post(query);
            var wob = new WriteOnceBlock<NmsMessage>(m => m);
            NmsMessage response, response2;
            using (nmsProtocol.IncomingMessages.LinkTo(
                wob, m => !ReferenceEquals(m, lastMessage) && m.IsResponse && m.ServiceType == query.ServiceType && m.Id == query.Id))
            {
                response = wob.Receive(options.Timeout);
            }
            nmsProtocol.IncomingMessages.TryReceive(null, out lastMessage);
            Assert.That(lastMessage, Is.EqualTo(response));
            var wob2 = new WriteOnceBlock<NmsMessage>(m => m);
            using (nmsProtocol.IncomingMessages.LinkTo(
                wob2, m => !ReferenceEquals(m, lastMessage) && m.IsResponse && m.ServiceType == query.ServiceType && m.Id == query.Id))
            {
                response2 = wob2.Receive(options.Timeout);
            }
        }

        static int[] GetMibIds(string filename)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            var mgr = new XmlNamespaceManager(xmlDoc.NameTable);
            mgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            var x = xmlDoc.SelectNodes("/xs:schema/xs:complexType/xs:attribute/xs:annotation/xs:appinfo/nms_id[../access='r']", mgr);
            if (x == null) return null;
            var result = new List<int>(x.Count);
            result.AddRange(from XmlElement e in x select Convert.ToInt32(e.InnerText, e.InnerText.StartsWith("0x") ? 16 : 10));
            return result.ToArray();
        }
    }
}