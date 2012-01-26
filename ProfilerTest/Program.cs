using System;
using System.Collections.Generic;
using System.Linq;
using NataInfo.Nibus;
using NataInfo.Nibus.Nms;

namespace ProfilerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var stack = new NibusStack(
                new SerialTransport("COM7", 115200, true),
                new NibusDataCodec(),
                new NmsCodec()))
            {
                var nmsProtocol = stack.GetCodec<NmsCodec>().Protocol;
                var et = new List<long>(102);
                var destanation = Address.CreateMac(0x20, 0x44);
                for (int i = 0; i < 102; i++)
                {
                    var ping = nmsProtocol.Ping(destanation);
                    Console.WriteLine("Ping: {0}", ping);
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
    }
}
