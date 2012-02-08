//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// InfoMessage.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Nms.Services;

#endregion

namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// Информационное сообщение. <see cref="GameReports.ShowMessage"/>.
    /// </summary>
    public class InfoMessage
    {
        #region Member Variables

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        /// <param name="id">Идентификатор информационного сообщения.</param>
        /// <param name="lines">Строки информационного сообщения.</param>
        public InfoMessage(byte id, params string[] lines)
        {
            Id = id;
            Lines = lines;
        }

        internal InfoMessage(IList<byte> data)
        {
            Contract.Requires(data != null);
            Contract.Requires(data.Count > 1);
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

        /// <summary>
        /// Возвращает идентификатор сообщения.
        /// </summary>
        public byte Id { get; private set; }

        /// <summary>
        /// Возвращает строки текстового сообщения.
        /// </summary>
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

    /// <summary>
    /// Расширение для конвертации информационных сообщений.
    /// </summary>
    internal static class InfoMessageExtensions
    {
        /// <summary>
        /// Создает <see cref="NmsInformationReport"/> из информационного сообщения.
        /// </summary>
        /// <param name="infoMessage">Информационное сообщение.</param>
        /// <param name="source">Адрес источника или <c>null</c>.</param>
        /// <returns></returns>
        public static NmsInformationReport CreateInformationReport(this InfoMessage infoMessage, Address source = null)
        {
            return new NmsInformationReport(
                source ?? Address.Empty,
                (int)GameReports.ShowMessage,
                NmsValueType.UInt8Array,
                infoMessage.GetData());
        }

        /// <summary>
        /// Извлекает информационное сообщение из <see cref="NmsInformationReport"/>.
        /// </summary>
        /// <param name="report">Исходное сообщение.</param>
        /// <returns></returns>
        public static InfoMessage GetInfoMessage(this NmsInformationReport report)
        {
            Contract.Requires(report.Id == (byte)GameReports.ShowMessage);
            return new InfoMessage(report.Datagram.Data.Skip(NmsMessage.NmsHeaderLength).ToArray());
        }
    }
}