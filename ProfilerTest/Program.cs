using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using NataInfo.Nibus;
using NataInfo.Nibus.Nms;

namespace ProfilerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var ids = GetMibIds(@"Z:\mibs\vms50.mib.xsd");
            Upload();
        }

        static void Upload()
        {
            using (var stack = new NibusStack(
                new SerialTransport("COM7", 115200, true),
                new NibusDataCodec(),
                new NmsCodec()))
            {
                var nmsProtocol = stack.GetCodec<NmsCodec>().Protocol;
                try
                {
                    var sw = new Stopwatch();
                    var total = 0;
                    var progress = new Progress<int>(cb =>
                                                         {
                                                             if (total == 0)
                                                             {
                                                                 total = cb;
                                                                 Console.WriteLine("Total: {0}", total);
                                                             }
                                                             else
                                                             {
                                                                 Console.Write("{0}\t{1}\r", cb, cb*100/total);
                                                             }
                                                         });
                    sw.Start();
                    var result = nmsProtocol.UploadDomainAsync(progress, Address.CreateMac(0x6c, 0xea), "NVRAM").Result;
                    sw.Stop();
                    Console.WriteLine("Duration: {0}", sw.Elapsed);
                    using (var file = File.Create(@"c:\temp\nvram.hex"))
                    {
                        file.Write(result, 0, result.Length);
                    }
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.Flatten().InnerException);
                }
            }
        }
        
        static void Ping()
        {
            using (var stack = new NibusStack(
                new SerialTransport("COM7", 115200, true),
                new NibusDataCodec(),
                new NmsCodec()))
            {
                var nmsProtocol = stack.GetCodec<NmsCodec>().Protocol;
                var et = new List<long>(102);
                var destanation = Address.CreateMac(0x20, 0x44);
                var sw = new Stopwatch();
                for (int i = 0; i < 102; i++)
                {
                    var ping = nmsProtocol.Ping(destanation);
                    sw.Restart();
                    nmsProtocol.ReadValueAsync(destanation, 3).Wait();
                    sw.Stop();
                    Console.WriteLine("Ping: {0}, Read: {1}", ping, sw.ElapsedMilliseconds);
                    et.Add(ping);
                }
                var first = et[0];
                et.RemoveAt(0);
                et.Sort();
                Console.WriteLine("Min: {0}, Max: {1}, average: {2}, mediana: {3}, first: {4}", et.First(), et.Last(), et.Average(), et[50], first);
                //et = new List<long>(102);
                //for (int i = 0; i < 102; i++)
                //{
                //    var ping = nmsProtocol.PingAsync(destanation).Result;
                //    //Console.WriteLine("Ping: {0}", ping);
                //    et.Add(ping);
                //}
                //first = et[0];
                //et.RemoveAt(0);
                //et.Sort();
                //Console.WriteLine("Async version Min: {0}, Max: {1}, average: {2}, mediana: {3}, first: {4}", et.First(), et.Last(), et.Average(), et[50], first);
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
            foreach (XmlElement e in x)
            {
                Console.WriteLine(e.InnerText);
                result.Add(Convert.ToInt32(e.InnerText, e.InnerText.StartsWith("0x") ? 16 : 10));
            }
            return result.ToArray();
        }
    }
}
