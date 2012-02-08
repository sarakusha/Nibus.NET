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
    /// <summary>
    /// Текущая статистика по удалениям в матче (хоккей).
    /// </summary>
    public class PenaltyStat
    {
        #region Member Variables

        private const int LinesOfs = 0;
        private const int MaxItems = 5;

        /// <summary>
        /// Идентификатор сообщения о статистике удалений.
        /// </summary>
        public const int PenaltyStatId = 28;
        
        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        /// <param name="lines">Максимальное количество удаленных в каждой команде.</param>
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

        /// <summary>
        /// Возвращает статистику по удалениям команды хозяев.
        /// </summary>
        public ReadOnlyCollection<PenaltyInfo> HomePenalties { get; private set; }
        /// <summary>
        /// Возвращает статистику по удалениям команды гостей.
        /// </summary>
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

    /// <summary>
    /// Расширение для конвертации статистики по удалениям.
    /// </summary>
    internal static class PenaltyStatExtensions
    {
        public static NmsInformationReport CreateInformationReport(this PenaltyStat penaltyStat, Address source = null)
        {
            return new NmsInformationReport(
                source ?? Address.Empty,
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
