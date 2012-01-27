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
using NataInfo.Nibus.Nms.Services;

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
        private const int Length = 6;

        private readonly byte[] _data;
        private Attributes _attrs;

        [Flags]
        private enum Attributes : byte
        {
            Active = 1,
            Dots = 2,
            Rest = 4,
            Fraction = 8,
            TenthOnly = 16,
            Hidden = 32,
            Secondary = 64
        }

        public TimerInfo(int timerId)
        {
            _data = new byte[Length];
            _data[0] = (byte)NmsValueType.UInt8Array;
            _data[IdOfs] = (byte)timerId;
        }

        internal TimerInfo(IEnumerable<byte> data)
        {
            Contract.Requires(data != null);
            _data = data.Take(Length).ToArray();
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

        public bool HasFraction
        {
            get { return (_attrs & Attributes.Fraction) != 0; }
            set
            {
                if (value)
                {
                    _attrs |= Attributes.Fraction;
                }
                else
                {
                    _attrs &= ~Attributes.Fraction;
                }
            }
        }

        public bool IsTenthOnly
        {
            get { return (_attrs & Attributes.TenthOnly) != 0; }
            set
            {
                if (value)
                {
                    _attrs |= Attributes.TenthOnly;
                }
                else
                {
                    _attrs &= ~Attributes.TenthOnly;
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
            return new TimerInfo((byte)informationReport.Value);
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
