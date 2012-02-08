namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// Содержит сведения об изменении статистики по удалениям.
    /// </summary>
    public sealed class PenaltyStatChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Возвращает текущую статистику по удалениям.
        /// </summary>
        public PenaltyStat PenaltyStat { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PenaltyStatChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="penaltyStat">Статистика по удалениям.</param>
        public PenaltyStatChangedEventArgs(Address source, PenaltyStat penaltyStat)
            : base(source)
        {
            PenaltyStat = penaltyStat;
        }
    }
}