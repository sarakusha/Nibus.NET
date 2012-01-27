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
    public class TimerAttributes
    {
        private const int IdOfs = 0;
        private const int AttrsOfs = 1;
        private const int DurationOfs = 2;
        internal const int Length = 6;

        private readonly byte[] _data;
        private Attributes _attrs;

        [Flags]
        private enum Attributes : byte
        {
            FractionOnLastMinute = 1,
            FractionAlways = 2,
            Increase = 128
        }

        public TimerAttributes(byte id, UInt32 duration, bool increase, bool hasFractionOnLastMinute, bool hasFractionAlways)
        {
            _data = new byte[Length];
            Id = id;
            Duration = duration;
            Increase = increase;
            HasFractionOnLastMinute = hasFractionOnLastMinute;
            HasFractionAlways = hasFractionAlways;
        }

        public TimerAttributes(byte[] data, int startIndex)
        {
            Contract.Requires(data != null);
            Contract.Requires(data.Length == Length);
            _data = new byte[Length];
            Array.Copy(data, startIndex, _data, 0, Length);
            _attrs = (Attributes)_data[AttrsOfs];
        }

        public byte Id
        {
            get { return _data[IdOfs]; }
            private set { _data[IdOfs] = value; }
        }

        public uint Duration
        {
            get { return BitConverter.ToUInt32(_data, DurationOfs); }
            private set { BitConverter.GetBytes(value).CopyTo(_data, DurationOfs); }
        }

        public bool Increase
        {
            get { return (_attrs & Attributes.Increase) != 0; }
            private set
            {
                if (value)
                {
                    _attrs |= Attributes.Increase;
                }
                else
                {
                    _attrs &= ~Attributes.Increase;
                }
            }
        }

        public bool HasFractionOnLastMinute
        {
            get { return (_attrs & Attributes.FractionOnLastMinute) != 0; }
            private set
            {
                if (value)
                {
                    _attrs |= Attributes.FractionOnLastMinute;
                }
                else
                {
                    _attrs &= ~Attributes.FractionOnLastMinute;
                }
            }
        }

        public bool HasFractionAlways
        {
            get { return (_attrs & Attributes.FractionAlways) != 0; }
            private set
            {
                if (value)
                {
                    _attrs |= Attributes.FractionAlways;
                }
                else
                {
                    _attrs &= ~Attributes.FractionAlways;
                }
            }
        }

        internal byte[] GetData()
        {
            _data[AttrsOfs] = (byte)_attrs;
            return (byte[])_data.Clone();
        }
    }

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
            _timers = timers;
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

        public ushort Id { get; private set; }

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
