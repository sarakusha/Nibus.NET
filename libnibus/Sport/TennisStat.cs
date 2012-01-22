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

#endregion

namespace NataInfo.Nibus.Sport
{
    public class TennisStat
    {
        #region Member Variables

        private const int SportOfs = 0;
        private const int SetOfs = 2;
        private const int AttrsOfs = 3;
        private const int HomeScoreOfs = 4;
        private const int VisitingScoreOfs = 5;
        private const int GamesOfs = 6;
        private const int HomeGameScoreOfs = 26;
        private const int VisitingGameScoreOfs = 28;
        private const int Length = 30;

        private readonly byte[] _data;
        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public TennisStat()
        {
            _data = new byte[Length];
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

        public int Set
        {
            get { return _data[SetOfs]; }
            set { _data[SetOfs] = (byte)value; }
        }
        public int HomeScore
        {
            get { return _data[HomeScoreOfs]; }
            set { _data[HomeScoreOfs] = (byte)value; }
        }
        public int VisitingScore
        {
            get { return _data[VisitingScoreOfs]; }
            set { _data[VisitingScoreOfs] = (byte)value; }
        }
        public int HomeGameScore
        {
            get { return _data[HomeGameScoreOfs]; }
            set { _data[HomeGameScoreOfs] = (byte)value; }
        }
        public int VisitingGameScore
        {
            get { return _data[VisitingGameScoreOfs]; }
            set { _data[VisitingGameScoreOfs] = (byte)value; }
        }

        public Tuple<ushort, ushort>[] SetScores { get; private set; } 

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

    public static class TennisExtensions
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
