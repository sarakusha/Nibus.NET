//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// ProviderInfo.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Nms.Services;

#endregion

namespace NataInfo.Nibus.Sport
{
    public class ProviderInfo
    {
        #region Member Variables

        private const int IdOfs = 0;
        private const int TimerCountOfs = 3;
        private const int TimersOfs = 4;

        private readonly TimerAttributes[] _timers;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public ProviderInfo(ushort id, params TimerAttributes[] timers)
        {
            Id = id;
            _timers = (TimerAttributes[])timers.Clone();
        }

        public ProviderInfo(ProviderInfo other) : this(other.Id, other.Timers)
        {
        }

        internal ProviderInfo(byte[] data)
        {
            Contract.Requires(data != null);
            Contract.Requires(data.Length >= TimersOfs);
            Id = BitConverter.ToUInt16(data, IdOfs);
            var timerCount = data[TimerCountOfs];
            var timers = new List<TimerAttributes>(timerCount);
            for (var i = TimersOfs; i < data.Length; i += TimerAttributes.Length)
            {
                var timer = new TimerAttributes(data, i);
                timers.Add(timer);
            }

            _timers = timers.ToArray();
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Возвращает идентификатор провайдера.
        /// </summary>
        /// <seealso cref="Providers"/>
        public ushort Id { get; private set; }

        /// <summary>
        /// Возвращает таймеры для провайдера.
        /// </summary>
        public TimerAttributes[] Timers { get { return _timers; } }

        #endregion //Properties

        #region Methods

        internal byte[] GetData()
        {
            var data = new List<byte>(2 + _timers.Length*TimerAttributes.Length);
            data.AddRange(BitConverter.GetBytes(Id));
            data.Add((byte)_timers.Length);
            foreach (var timer in _timers)
            {
                data.AddRange(timer.GetData());
            }
            return data.Take(NmsMessage.NmsMaxDataLength).ToArray();
        }

        #endregion //Methods
    }

    public static class ProviderExtensions
    {
        public static NmsInformationReport Create(Address source, ProviderInfo providerInfo)
        {
            return new NmsInformationReport(
                source,
                (int)GameReports.ChangeSport,
                NmsValueType.UInt8Array,
                providerInfo.GetData());
        }

        public static ProviderInfo GetProviderInfo(this NmsInformationReport report)
        {
            Contract.Requires(report.Id == (byte)GameReports.ChangeSport);
            return new ProviderInfo((byte)report.Value);
        }
    }
}
