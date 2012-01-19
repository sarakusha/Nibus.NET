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

#endregion

namespace NataInfo.Nibus.Sport
{
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

        public TeamRole Role
        {
            get { return (TeamRole)_data[RoleOfs]; }
            private set { _data[RoleOfs] = (byte)value; }
        }

        public int Index
        {
            get { return _data[IndexOfs]; }
            private set { _data[IndexOfs] = (byte)value; }
        }

        public int Number
        {
            get { return _data[NumberOfs]; }
            private set { _data[NumberOfs] = (byte)value; }
        }

        public int Goals
        {
            get { return _data[GoalsOfs]; }
            private set { _data[GoalsOfs] = (byte)value; }
        }

        public int Fouls
        {
            get { return _data[FoulsOfs]; }
            private set { _data[FoulsOfs] = (byte)value; }
        }

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

        public byte Stat1
        {
            get { return _data[Stat1Ofs]; }
            private set { _data[Stat1Ofs] = value; }
        }

        public byte Stat2
        {
            get { return _data[Stat2Ofs]; }
            private set { _data[Stat2Ofs] = value; }
        }

        public byte Stat3
        {
            get { return _data[Stat3Ofs]; }
            private set { _data[Stat3Ofs] = value; }
        }

        #endregion //Properties

        internal byte[] GetData()
        {
            return _data;
        }
    }

    public static class PlayerStatExtensions
    {
        public static NmsInformationReport Create(Address source, PlayerStat stat)
        {
            return new NmsInformationReport(
                source,
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