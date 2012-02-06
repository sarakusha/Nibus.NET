//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// PenaltyStat.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Nms.Services;

#endregion

namespace NataInfo.Nibus.Sport
{
    public class PenaltyStat
    {
        #region Member Variables

        private const int LinesOfs = 0;
        private const int MaxItems = 5;

        public const int PenaltyStatId = 28;
        
        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public PenaltyStat(int lines = MaxItems)
        {
            HomePenalties = new ReadOnlyCollection<PenaltyInfo>(new PenaltyInfo[lines]);
            VisitorPenalties = new ReadOnlyCollection<PenaltyInfo>(new PenaltyInfo[lines]);
        }

        internal PenaltyStat(byte[] data)
        {
            Contract.Requires(data.Length == 1 + data[LinesOfs] * PenaltyInfo.ItemLength * 2);
            var lines = data[LinesOfs];
            var teams = data.Skip(1).Split(PenaltyInfo.ItemLength*lines).ToArray();
            HomePenalties = new ReadOnlyCollection<PenaltyInfo>(
                teams[0].Split(PenaltyInfo.ItemLength).Select(d => new PenaltyInfo(d)).ToArray());
            VisitorPenalties = new ReadOnlyCollection<PenaltyInfo>(
                teams[1].Split(PenaltyInfo.ItemLength).Select(d => new PenaltyInfo(d)).ToArray());
        }

        #endregion //Constructors

        #region Properties

        public ReadOnlyCollection<PenaltyInfo> HomePenalties { get; private set; }
        public ReadOnlyCollection<PenaltyInfo> VisitorPenalties { get; private set; }

        #endregion //Properties

        #region Methods

        internal byte[] GetData()
        {
            var lines = HomePenalties.Count;
            var data = new byte[1 + lines * 2 * PenaltyInfo.ItemLength];
            data[LinesOfs] = (byte)(lines);
            HomePenalties.SelectMany(p => p.GetData()).ToArray().CopyTo(data, 1);
            VisitorPenalties.SelectMany(p => p.GetData()).ToArray().CopyTo(data, 1 + lines*PenaltyInfo.ItemLength);
            return data;
        }

        #endregion //Methods
    }

    public sealed class PenaltyStatChangedEventArgs : BaseInformationReportEventArgs
    {
        public PenaltyStat PenaltyStat { get; private set; }

        public PenaltyStatChangedEventArgs(Address source, PenaltyStat penaltyStat)
            : base(source)
        {
            PenaltyStat = penaltyStat;
        }
    }

    public static class PenaltyStatExtensions
    {
        public static NmsInformationReport Create(Address source, PenaltyStat penaltyStat)
        {
            return new NmsInformationReport(
                source,
                PenaltyStat.PenaltyStatId,
                NmsValueType.UInt8Array,
                penaltyStat.GetData());
        }

        public static PenaltyStat GetPenaltyStat(this NmsInformationReport report)
        {
            Contract.Requires(report.Id == PenaltyStat.PenaltyStatId);
            return new PenaltyStat((byte[])report.Value);
        }
    }
}
