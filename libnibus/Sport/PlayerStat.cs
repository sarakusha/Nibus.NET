//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// PlayerStat.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private byte[] _data;

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
        public PlayerStat(TeamRole role, int index, int number, int goals, int fouls, bool isActive, params byte[] stats)
        {
            Role = role;
            Index = index;
            Number = number;
            Goals = goals;
            Fouls = fouls;
            IsActive = isActive;
            _data = new byte[Length];
        }

        #endregion //Constructors

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


        #region Properties



        #endregion //Properties

        #region Methods

        #endregion //Methods

        #region Implementations

        #endregion //Implementations
    }
}
