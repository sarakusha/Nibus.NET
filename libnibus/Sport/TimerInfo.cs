//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// TimerInfo.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NataInfo.Nibus.Nms;

#endregion

namespace NataInfo.Nibus.Sport
{
    public sealed class TimerInfo
    {
        private const int AttrOfs = 1;
        internal const int IdOfs = 2;
        private const int MinOfs = 3;
        private const int SecOfs = 4;
        private const int HundredthOfs = 5;

        private readonly byte[] _data;
        private Attributes _attrs;

        [Flags]
        private enum Attributes : byte
        {
            Active = 1,
            Dots = 2,
            Rest = 4,
            Hundredth = 8,
            Tenth = 16,
            Hidden = 32,
            Secondary = 64
        }

        public TimerInfo(int timerId)
        {
            _data = new byte[6];
            _data[0] = (byte)NmsValueType.UInt8Array;
            _data[IdOfs] = (byte)timerId;
        }

        internal TimerInfo(IEnumerable<byte> data)
        {
            _data = data.Take(6).ToArray();
        }

        public int TimerId
        {
            get { return _data[IdOfs]; }
        }

        public bool IsActive
        {
            get { return (_attrs & Attributes.Active) != 0; }
            set
            {
                if (value)
                {
                    _attrs |= Attributes.Active;
                }
                else
                {
                    _attrs &= ~Attributes.Active;
                }
            }
        }

        public bool HasDots
        {
            get { return (_attrs & Attributes.Dots) != 0; }
            set
            {
                if (value)
                {
                    _attrs |= Attributes.Dots;
                }
                else
                {
                    _attrs &= ~Attributes.Dots;
                }
            }
        }

        public bool IsRest
        {
            get { return (_attrs & Attributes.Rest) != 0; }
            set
            {
                if (value)
                {
                    _attrs |= Attributes.Rest;
                }
                else
                {
                    _attrs &= ~Attributes.Rest;
                }
            }
        }

        public bool HasHundredth
        {
            get { return (_attrs & Attributes.Hundredth) != 0; }
            set
            {
                if (value)
                {
                    _attrs |= Attributes.Hundredth;
                }
                else
                {
                    _attrs &= ~Attributes.Hundredth;
                }
            }
        }

        public bool IsOnlyTenth
        {
            get { return (_attrs & Attributes.Tenth) != 0; }
            set
            {
                if (value)
                {
                    _attrs |= Attributes.Tenth;
                }
                else
                {
                    _attrs &= ~Attributes.Tenth;
                }
            }
        }

        public bool IsHidden
        {
            get { return (_attrs & Attributes.Hidden) != 0; }
            set
            {
                if (value)
                {
                    _attrs |= Attributes.Hidden;
                }
                else
                {
                    _attrs &= ~Attributes.Hidden;
                }
            }
        }

        public bool IsSecondary
        {
            get { return (_attrs & Attributes.Secondary) != 0; }
            set
            {
                if (value)
                {
                    _attrs |= Attributes.Secondary;
                }
                else
                {
                    _attrs &= ~Attributes.Secondary;
                }
            }
        }

        public int Minutes
        {
            get { return NmsMessage.UnpackByte(_data[MinOfs]); }
            set { _data[MinOfs] = NmsMessage.PackByte(value); }
        }

        public int Seconds
        {
            get { return NmsMessage.UnpackByte(_data[SecOfs]); }
            set { _data[SecOfs] = NmsMessage.PackByte(value); }
        }

        public int Hundredth
        {
            get { return NmsMessage.UnpackByte(_data[HundredthOfs]); }
            set { _data[HundredthOfs] = NmsMessage.PackByte(value); }
        }

        public byte[] GetData()
        {
            _data[AttrOfs] = (byte)_attrs;
            return (byte[])_data.Clone();
        }
    }

    public static class NmsTimerExtentions
    {
        public static NmsInformationReport Create(Address source, TimerInfo timerInfo)
        {
            return new NmsInformationReport(
                source,
                (int)GameReports.Timer,
                NmsValueType.UInt8Array,
                timerInfo.GetData(),
                PriorityType.Realtime);
        }

        public static TimerInfo GetTimerInfo(this NmsInformationReport informationReport)
        {
            Contract.Requires(informationReport.Id == (byte)GameReports.Timer);
            return new TimerInfo(informationReport.Datagram.Data.Skip(NmsMessage.NmsHeaderLength));
        }

        public static int GetTimerId(this NmsInformationReport informationReport)
        {
            Contract.Requires(informationReport.Id == (byte)GameReports.Timer);
            return informationReport.Datagram.Data[NmsMessage.NmsHeaderLength + TimerInfo.IdOfs];
        }

        public static bool IsTimerReport(this NmsInformationReport informationReport)
        {
            return informationReport.Id == (byte)GameReports.Timer;
        }
    }
}
