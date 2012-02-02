//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// TennisStat.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Nms.Services;

#endregion

namespace NataInfo.Nibus.Sport
{
    public class TennisStat
    {
        #region Member Variables

        private const int SportOfs = 0;
        private const int SetOfs = 2;
        private const int AttrsOfs = 3;
        private const int FirstPlayerSetsOfs = 4;
        private const int SecondPlayerSetsOfs = 5;
        private const int GamesOfs = 6;
        private const int FirstPlayerPointsOfs = 26;
        private const int SecondPlayerPointsOfs = 28;
        private const int Length = 30;

        private readonly byte[] _data;
        #endregion

        [Flags]
        private enum Attributes : byte
        {
            BallOwner = 1,
            TaiBreak = 7
        }

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public TennisStat()
        {
            _data = new byte[Length];
            _data[SportOfs] = 14;
            SetScores = new Tuple<ushort, ushort>[5];
            for (int i = 0; i < 5; i++)
            {
                SetScores[i] = new Tuple<ushort, ushort>(0,0);
            }
        }

        internal TennisStat(byte[] data)
        {
            Contract.Requires(data != null);
            Contract.Requires(data.Length == Length);
            Contract.Requires(data[SportOfs] == 14);    
            _data = new byte[Length];
            Array.Copy(data, _data, Length);

            SetScores = new Tuple<ushort, ushort>[5];
            for (int i = 0; i < 5; i++)
            {
                var left = BitConverter.ToUInt16(_data, GamesOfs + i*4);
                var right = BitConverter.ToUInt16(_data, GamesOfs + i * 4 + 2);
                SetScores[i] = new Tuple<ushort, ushort>(left, right);
            }
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Номер текущего сета.
        /// </summary>
        public int Set
        {
            get { return _data[SetOfs]; }
            set { _data[SetOfs] = (byte)value; }
        }

        /// <summary>
        /// Количество выигранных сетов первым игроком.
        /// </summary>
        public int FirstPlayerSets
        {
            get { return _data[FirstPlayerSetsOfs]; }
            set { _data[FirstPlayerSetsOfs] = (byte)value; }
        }

        /// <summary>
        /// Количество выигранных сетов вторым игроком.
        /// </summary>
        public int SecondPlayerSets
        {
            get { return _data[SecondPlayerSetsOfs]; }
            set { _data[SecondPlayerSetsOfs] = (byte)value; }
        }

        /// <summary>
        /// Количество выигранных подач в текущем гейме у первого игрока.
        /// </summary>
        public int FirstPlayerPoints
        {
            get { return _data[FirstPlayerPointsOfs]; }
            set { _data[FirstPlayerPointsOfs] = (byte)value; }
        }

        /// <summary>
        /// Количество выигранных подач в текущем гейме у второго игрока.
        /// </summary>
        public int SecondPlayerPoints
        {
            get { return _data[SecondPlayerPointsOfs]; }
            set { _data[SecondPlayerPointsOfs] = (byte)value; }
        }

        /// <summary>
        /// Возвращает набор пар со счетом по выигранным геймам в каждом из сетов.
        /// </summary>
        /// <remarks>
        /// Всего пять пар. Первый элемент пары - количество выигранных геймов первым игроком,
        /// второй элемент пары - количество выигранных геймов вторым игроком
        /// </remarks>
        public Tuple<ushort, ushort>[] SetScores { get; private set; }

        /// <summary>
        /// Чья очередь подавать.
        /// </summary>
        /// <value>
        /// <c>BallOwner.Home</c> - подает первый игрок, <c>BallOwner.Visitor</c> - второй.
        /// </value>
        public BallOwner BallOwner
        {
            get { return (_data[AttrsOfs] & (byte)Attributes.BallOwner) == 0 ? BallOwner.Home : BallOwner.Visitor; }
            set
            {
                if (value == BallOwner.Visitor)
                {
                    _data[AttrsOfs] |= (byte)Attributes.BallOwner;
                }
                else
                {
                    _data[AttrsOfs] &= (byte)~Attributes.BallOwner;
                }
            }
        }

        /// <summary>
        /// Индикатор розыгрыша гейма по системе тай-брейк.
        /// </summary>
        public bool IsTieBreak
        {
            get { return (_data[AttrsOfs] & (byte)Attributes.TaiBreak) != 0; }
            set
            {
                if (value)
                {
                    _data[AttrsOfs] |= (byte)Attributes.TaiBreak;
                }
                else
                {
                    _data[AttrsOfs] &= (byte)~Attributes.TaiBreak;
                }
            }
        }

        #endregion //Properties

        #region Methods

        internal byte[] GetData()
        {
            for (int i = 0; i < 5; i++)
            {
                BitConverter.GetBytes(SetScores[i].Item1).CopyTo(_data, GamesOfs + i*4);
                BitConverter.GetBytes(SetScores[i].Item2).CopyTo(_data, GamesOfs + i*4 + 2);
            }

            return _data;
        }
        #endregion //Methods
    }

    public sealed class TennisStatChangedEventArgs : BaseInformationReportEventArgs
    {
        public TennisStat TennisStat { get; private set; }

        public TennisStatChangedEventArgs(Address source, TennisStat tennisStat) : base(source)
        {
            TennisStat = tennisStat;
        }
    }

    internal static class TennisExtensions
    {
        public static NmsInformationReport Create(Address source, TennisStat tennisStat)
        {
            return new NmsInformationReport(
                source,
                TennisProtocol.TennisStats,
                NmsValueType.UInt8Array,
                tennisStat.GetData());
        }

        public static TennisStat GetTennisStat(this NmsInformationReport report)
        {
            Contract.Requires(report.Id == TennisProtocol.TennisStats);
            return new TennisStat((byte[])report.Value);
        }
    }
}
