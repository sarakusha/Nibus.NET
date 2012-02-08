//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// PlayerStat.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics.Contracts;
using System.Linq;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Nms.Services;

#endregion

namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// Статистика игрока.
    /// </summary>
    public class PlayerStat
    {
        #region Member Variables

        private const int RoleOfs = 0;
        private const int NumberOfs = 1;
        private const int IndexOfs = 2;
        private const int GoalsOfs = 3;
        private const int FoulsOfs = 4;
        private const int AttrOfs = 5;
        private const int Stat1Ofs = 6;
        private const int Stat2Ofs = 7;
        private const int Stat3Ofs = 8;
        private const int Length = 9;

        private readonly byte[] _data;

        #endregion

        [Flags]
        private enum Attributes : byte
        {
            Active = 1,
            StatPresent = 2
        }

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        /// <param name="role">Принадлежность к команде.</param>
        /// <param name="index">Индекс игрока (начинается с 0).</param>
        /// <param name="number">Номер игрока.</param>
        /// <param name="goals">Заработанные очки.</param>
        /// <param name="fouls">Полученные фолы.</param>
        /// <param name="isActive"><c>true</c> - игрок на площадке, иначе - <c>false</c>.</param>
        /// <param name="stats">Дополнительная статистика по игроку (не более 3 значений).</param>
        public PlayerStat(
            TeamRole role, int index, int number, int goals, int fouls, bool isActive, params byte[] stats)
        {
            Role = role;
            Index = index;
            Number = number;
            Goals = goals;
            Fouls = fouls;
            IsActive = isActive;
            _data = new byte[Length];
            HasStats = stats.Length > 0;
            stats.Take(3).ToArray().CopyTo(_data, Stat1Ofs);
        }

        internal PlayerStat(byte[] data)
        {
            Contract.Requires(data.Length >= Length);
            _data = new byte[Length];
            Array.Copy(data, _data, Length);
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Возвращает принадлежность к команде.
        /// </summary>
        public TeamRole Role
        {
            get { return (TeamRole)_data[RoleOfs]; }
            private set { _data[RoleOfs] = (byte)value; }
        }

        /// <summary>
        /// Возвращает индекс игрока в списке.
        /// </summary>
        /// <remarks>Нумерация начинается с <c>0</c>.</remarks>
        public int Index
        {
            get { return _data[IndexOfs]; }
            private set { _data[IndexOfs] = (byte)value; }
        }

        /// <summary>
        /// Возвращает номер игрока.
        /// </summary>
        public int Number
        {
            get { return _data[NumberOfs]; }
            private set { _data[NumberOfs] = (byte)value; }
        }

        /// <summary>
        /// Возвращает количество очков, заработанных игроком.
        /// </summary>
        public int Goals
        {
            get { return _data[GoalsOfs]; }
            private set { _data[GoalsOfs] = (byte)value; }
        }

        /// <summary>
        /// Возвращает количество фолов/штрафов полученных игроком.
        /// </summary>
        public int Fouls
        {
            get { return _data[FoulsOfs]; }
            private set { _data[FoulsOfs] = (byte)value; }
        }

        /// <summary>
        /// Индикатор нахождения игрока на площадке.
        /// </summary>
        public bool IsActive
        {
            get { return (_data[AttrOfs] & (byte)Attributes.Active) != 0; }
            private set
            {
                if (value)
                {
                    _data[AttrOfs] |= (byte)Attributes.Active;
                }
                else
                {
                    _data[AttrOfs] &= (byte)~Attributes.Active;
                }
            }
        }

        /// <summary>
        /// Имеется акутальная статистика по броскам.
        /// </summary>
        /// <seealso cref="Stat1"/>
        /// <seealso cref="Stat2"/>
        /// <seealso cref="Stat3"/>
        public bool HasStats
        {
            get { return (_data[AttrOfs] & (byte)Attributes.StatPresent) != 0; }
            private set
            {
                if (value)
                {
                    _data[AttrOfs] |= (byte)Attributes.StatPresent;
                }
                else
                {
                    _data[AttrOfs] &= (byte)~Attributes.StatPresent;
                }
            }
        }

        /// <summary>
        /// Возвращает статистику 1.
        /// </summary>
        /// <remarks>Количество 1-очковых бросков.</remarks>
        public byte Stat1
        {
            get
            {
                Contract.Requires(HasStats);
                return _data[Stat1Ofs];
            }
        }

        /// <summary>
        /// Возвращает статистику 2.
        /// </summary>
        /// <remarks>Количество 2-очковых бросков.</remarks>
        public byte Stat2
        {
            get
            {
                Contract.Requires(HasStats);
                return _data[Stat2Ofs];
            }
        }

        /// <summary>
        /// Возвращает статистику 3.
        /// </summary>
        /// <remarks>Количество 3-очковых бросков.</remarks>
        public byte Stat3
        {
            get
            {
                Contract.Requires(HasStats);
                return _data[Stat3Ofs];
            }
        }

        #endregion //Properties

        internal byte[] GetData()
        {
            return _data;
        }
    }

    internal static class PlayerStatExtensions
    {
        public static NmsInformationReport CreateInformationReport(this PlayerStat stat, Address source = null)
        {
            return new NmsInformationReport(
                source ?? Address.Empty,
                (int)GameReports.PlayerStat,
                NmsValueType.UInt8Array,
                stat.GetData());
        }

        public static PlayerStat GetPlayerStat(this NmsInformationReport report)
        {
            Contract.Requires(report.Id == (byte)GameReports.PlayerStat);
            return new PlayerStat((byte[])report.Value);
        }
    }
}