//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// InfoMessage.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Nms.Services;

#endregion

namespace NataInfo.Nibus.Sport
{
    public class InfoMessage
    {
        #region Member Variables

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public InfoMessage(byte id, params string[] lines)
        {
            Id = id;
            Lines = lines;
        }

        internal InfoMessage(IList<byte> data)
        {
            Id = data[0];
            var i = 0;
            var splits = from b in data.Skip(1)
                         group b by b == 0 ? i++ : i
                             into part
                             select Encoding.Default.GetString(part.TakeWhile(b => b != 0).ToArray());
            Lines = splits.ToArray();
        }
        #endregion //Constructors

        #region Properties

        public byte Id { get; private set; }
        public string[] Lines { get; private set; }

        #endregion //Properties

        #region Methods

        internal byte[] GetData()
        {
            var data = new List<byte>();
            data.Add(Id);
            foreach (var line in Lines)
            {
                data.AddRange(Encoding.Default.GetBytes(line));
                data.Add(0);
            }
            return data.Take(NmsMessage.NmsMaxDataLength).ToArray();
        }

        #endregion //Methods
    }

    public static class InfoMessageExtensions
    {
        public static NmsInformationReport Create(Address source, InfoMessage infoMessage)
        {
            return new NmsInformationReport(
                source,
                (int)GameReports.ShowMessage,
                NmsValueType.UInt8Array,
                infoMessage.GetData());
        }

        public static InfoMessage GetInfoMessage(this NmsInformationReport report)
        {
            Contract.Requires(report.Id == (byte)GameReports.ShowMessage);
            return new InfoMessage(report.Datagram.Data.Skip(NmsMessage.NmsHeaderLength).ToArray());
        }
    }
}
