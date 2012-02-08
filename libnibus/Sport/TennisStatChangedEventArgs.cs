namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// Содержит сведения об изменении статистики по теннису.
    /// </summary>
    public sealed class TennisStatChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Возвращает статистику по теннису.
        /// </summary>
        public TennisStat TennisStat { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TennisStatChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="tennisStat">Статистика.</param>
        public TennisStatChangedEventArgs(Address source, TennisStat tennisStat) : base(source)
        {
            TennisStat = tennisStat;
        }
    }
}