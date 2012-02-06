using System;
using System.Diagnostics.Contracts;

namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// Атрибуты таймера в провайдере игры.
    /// </summary>
    public class TimerAttributes
    {
        private const int IdOfs = 0;
        private const int AttrsOfs = 1;
        private const int DurationOfs = 2;
        internal const int Length = 6;

        private readonly byte[] _data;
        private Attributes _attrs;

        public const uint Infinity = 0;

        [Flags]
        private enum Attributes : byte
        {
            FractionOnLastMinute = 1,
            FractionAlways = 2,
            HiResolution = 4,
            LongTimeFormat = 8,
            FractionWhenPaused = 16,
            RestTimer = 32,
            SecondaryTimer = 64,
            Increase = 128
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerAttributes"/> class.
        /// </summary>
        /// <param name="id">Идентификатор таймера.</param>
        /// <param name="duration">Продолжительность.</param>
        /// <param name="increase">Если <c>true</c> возрастание, иначе убывание.</param>
        /// <param name="hasFractionOnLastMinute">Показывать доли секунды на последней минуте.</param>
        /// <param name="hasFractionAlways">Показывать доли секунды всегда.</param>
        /// <param name="isHiResolution">Разрешение 1/100 сек. (по умолчанию 1/10).</param>
        /// <param name="isLongTimeFormat">Всегда показывать время с минутами.</param>
        /// <param name="hasFractionWhenPaused">Показывать доли секунды в паузе.</param>
        /// <param name="isRestTimer">Не игровой таймер.</param>
        /// <param name="isSecondaryTimer">Не показывать данный таймер на знакоместах основного таймера.</param>
        /// <param name="description">Описание таймера</param>
        public TimerAttributes(
            byte id,
            UInt32 duration,
            bool increase,
            bool hasFractionOnLastMinute,
            bool hasFractionAlways,
            bool isHiResolution,
            bool isLongTimeFormat,
            bool hasFractionWhenPaused,
            bool isRestTimer,
            bool isSecondaryTimer,
            string description)
        {
            _data = new byte[Length];
            Id = id;
            Duration = duration;
            Increase = increase;
            HasFractionOnLastMinute = hasFractionOnLastMinute;
            HasFractionAlways = hasFractionAlways;
            IsHiResolution = isHiResolution;
            IsLongTimeFormat = isLongTimeFormat;
            HasFractionWhenPaused = hasFractionWhenPaused;
            IsRestTimer = isRestTimer;
            IsSecondaryTimer = isSecondaryTimer;
            Description = description;
        }

        public TimerAttributes(byte[] data, int startIndex)
        {
            Contract.Requires(data != null);
            Contract.Requires(data.Length == Length);
            _data = new byte[Length];
            Array.Copy(data, startIndex, _data, 0, Length);
            _attrs = (Attributes)_data[AttrsOfs];
        }

        /// <summary>
        /// Возвращает идентификатор таймера.
        /// </summary>
        public byte Id
        {
            get { return _data[IdOfs]; }
            private set { _data[IdOfs] = value; }
        }

        /// <summary>
        /// Интервал таймера (длительность в секундах).
        /// </summary>
        /// <value><c>0</c> - бесконечный таймер.</value>
        /// <seealso cref="Infinity"/>
        public uint Duration
        {
            get { return BitConverter.ToUInt32(_data, DurationOfs); }
            set { BitConverter.GetBytes(value).CopyTo(_data, DurationOfs); }
        }

        /// <summary>
        /// Направление отсчета.
        /// </summary>
        /// <value><c>true</c> - возрастание, <c>false</c> - убывание.</value>
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

        /// <summary>
        /// Показывать доли секунды на последней минуте.
        /// </summary>
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

        /// <summary>
        /// Показывать доли секунды всегда.
        /// </summary>
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

        /// <summary>
        /// Разрешение 1/100 сек. (по умолчанию 1/10)
        /// </summary>
        public bool IsHiResolution
        {
            get { return (_attrs & Attributes.HiResolution) != 0; }
            private set
            {
                if (value)
                {
                    _attrs |= Attributes.HiResolution;
                }
                else
                {
                    _attrs &= ~Attributes.HiResolution;
                }
            }
        }

        /// <summary>
        /// Всегда показывать время с минутами,
        /// иначе на последней минуте убывающего таймера при установленном <see cref="HasFractionOnLastMinute"/>
        /// только секунды и доли секунд.
        /// </summary>
        public bool IsLongTimeFormat
        {
            get { return (_attrs & Attributes.LongTimeFormat) != 0; }
            private set
            {
                if (value)
                {
                    _attrs |= Attributes.LongTimeFormat;
                }
                else
                {
                    _attrs &= ~Attributes.LongTimeFormat;
                }
            }
        }

        /// <summary>
        /// Показывать доли секунды в паузе.
        /// </summary>
        /// <remarks>Только если установлен флаг <see cref="IsLongTimeFormat"/></remarks>
        public bool HasFractionWhenPaused
        {
            get { return (_attrs & Attributes.FractionWhenPaused) != 0; }
            private set
            {
                if (value)
                {
                    _attrs |= Attributes.FractionWhenPaused;
                }
                else
                {
                    _attrs &= ~Attributes.FractionWhenPaused;
                }
            }
        }

        /// <summary>
        /// Не игровой таймер (Перерыв, таймаут, ...).
        /// </summary>
        public bool IsRestTimer
        {
            get { return (_attrs & Attributes.RestTimer) != 0; }
            private set
            {
                if (value)
                {
                    _attrs |= Attributes.RestTimer;
                }
                else
                {
                    _attrs &= ~Attributes.RestTimer;
                }
            }
        }

        /// <summary>
        /// Не показывать данный таймер на знакоместах основного таймера.
        /// </summary>
        public bool IsSecondaryTimer
        {
            get { return (_attrs & Attributes.SecondaryTimer) != 0; }
            private set
            {
                if (value)
                {
                    _attrs |= Attributes.SecondaryTimer;
                }
                else
                {
                    _attrs &= ~Attributes.SecondaryTimer;
                }
            }
        }

        public string Description { get; set; }

        internal byte[] GetData()
        {
            _data[AttrsOfs] = (byte)_attrs;
            return (byte[])_data.Clone();
        }
    }
}