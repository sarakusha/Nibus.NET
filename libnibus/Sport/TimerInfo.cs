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
    /// <summary>
    /// Данные таймера.
    /// </summary>
    public sealed class TimerInfo
    {
        private const int AttrOfs = 0;
        internal const int IdOfs = 1;
        private const int MinOfs = 2;
        private const int SecOfs = 3;
        private const int HundredthOfs = 4;
        private const int Length = 5;

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
            _attrs = (Attributes)_data[AttrOfs];
        }

        /// <summary>
        /// Возвращает идентификатор таймера.
        /// </summary>
        public int TimerId
        {
            get { return _data[IdOfs]; }
        }

        /// <summary>
        /// Индикатор активности таймера.
        /// </summary>
        /// <value>
        ///   <c>true</c>таймер активизирован; иначе - <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Включить точки между часами и минутами.
        /// </summary>
        /// <remarks>используется в основном для показа реального времени</remarks>
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

        /// <summary>
        /// Tаймер отсчитывает время перерыва.
        /// </summary>
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

        /// <summary>
        /// Нужно показывать доли секунды.
        /// </summary>
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

        /// <summary>
        /// Показывать только десятые доли секунды (разрешение таймера 1/10 сек).
        /// </summary>
        /// <remarks>Должен быть установлен также <see cref="HasFraction"/></remarks>
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

        /// <summary>
        /// Скрыть таймер.
        /// </summary>
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

        /// <summary>
        /// Не отображать данный таймер на знакоместах основного времени.
        /// </summary>
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

        /// <summary>
        /// Минуты.
        /// </summary>
        public int Minutes
        {
            get { return NmsMessage.UnpackByte(_data[MinOfs]); }
            set { _data[MinOfs] = NmsMessage.PackByte(value); }
        }

        /// <summary>
        /// Секунды.
        /// </summary>
        public int Seconds
        {
            get { return NmsMessage.UnpackByte(_data[SecOfs]); }
            set { _data[SecOfs] = NmsMessage.PackByte(value); }
        }

        /// <summary>
        /// Доли секунд.
        /// </summary>
        /// <value>Сотые доли, если <see cref="IsTenthOnly"/> = <c>False</c>, иначе десятые доли.</value>
        public int Fractions
        {
            get
            {
                return NmsMessage.UnpackByte(_data[HundredthOfs]);
            }
            set { _data[HundredthOfs] = NmsMessage.PackByte(IsTenthOnly ? value%10 : value); }
        }

        public string ToString(TimerAttributes attributes)
        {
            if (attributes == null)
            {
                return ToString();
            }

            if (attributes.IsSecondaryTimer && Minutes == 0 && Seconds == 0 && Fractions == 0)
            {
                return String.Empty;
            }

            if (attributes.IsLongTimeFormat)
            {
                if (!IsFinishedOrStarted(attributes) && 
                    (attributes.HasFractionAlways
                    || attributes.HasFractionOnLastMinute && IsLastMinute(attributes)
                    || attributes.HasFractionWhenPaused && !IsActive))
                {
                    var format = IsTenthOnly ? "{0:D2}:{1:D2}.{2}" : "{0:D2}:{1:D2}.{2:D2}";
                    return String.Format(format, Minutes, Seconds, Fractions);
                }

                return String.Format("{0:D2}:{1:D2}", Minutes, Seconds);
            }

            if (!attributes.Increase && attributes.HasFractionOnLastMinute && IsLastMinute(attributes))
            {
                return String.Format(IsTenthOnly ? "{0:D2}.{1}" : "{0:D2}.{1:D2}", Seconds, Fractions);
            }

            return ToString();
        }

        public override string ToString()
        {
            if (HasFraction)
            {
                var format = IsTenthOnly ? "{0:D2}:{1:D2}.{2}" : "{0:D2}:{1:D2}.{2:D2}";
                return String.Format(format, Minutes, Seconds, Fractions);
            }
            
            return String.Format("{0:D2}:{1:D2}", Minutes, Seconds);
        }

        internal byte[] GetData()
        {
            _data[AttrOfs] = (byte)_attrs;
            return (byte[])_data.Clone();
        }

        private bool IsLastMinute(TimerAttributes attributes)
        {
            if (attributes.Increase && attributes.Duration == 0)
            {
                return false;
            }

            return attributes.Increase && (attributes.Duration - (Minutes * 60 + Seconds)) < 60
                   || !attributes.Increase && Minutes == 0 && Seconds < 60;
        }

        private bool IsFinishedOrStarted(TimerAttributes attributes)
        {
            if (Minutes == 0 && Seconds == 0 && Fractions == 0)
            {
                return true;
            }

            if (attributes.Increase && attributes.Duration == 0)
            {
                return false;
            }

            return attributes.Increase && (attributes.Duration - (Minutes * 60 + Seconds)) == 0 && Fractions == 0
                   || !attributes.Increase && (Minutes*60 + Seconds) == attributes.Duration && Fractions == 0;
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
            return new TimerInfo((byte[])informationReport.Value);
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
