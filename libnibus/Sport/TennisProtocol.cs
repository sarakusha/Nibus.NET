//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// TennisProtocol.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using NataInfo.Nibus.Nms;

#endregion

namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// Протокол игры в Большой теннис.
    /// </summary>
    public sealed class TennisProtocol : GameProtocol
    {
        #region Member Variables

        /// <summary>
        /// Идентификатор статистики по теннису.
        /// </summary>
        public const int TennisStats = 32;
        private static ProviderInfo _provider;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        /// <param name="nmsProtocol">The NMS protocol.</param>
        public TennisProtocol(NmsProtocol nmsProtocol) : base(nmsProtocol)
        {
        }

        #endregion //Constructors

        #region Events

        /// <summary>
        /// Происходит при изменении статистики.
        /// </summary>
        public event EventHandler<TennisStatChangedEventArgs> TennisStatChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Возвращает провайдера игры.
        /// </summary>
        public override ProviderInfo Provider
        {
            get
            {
                return _provider ??
                       (_provider =
                        new ProviderInfo(
                            (ushort)Providers.Tennis,
                            new TimerAttributes(
                                1, TimerAttributes.Infinity, true, false, false, false, true, false, false, false, "Время игры")));
            }
        }

        #endregion //Properties

        #region Methods

        #endregion //Methods

        #region Implementations

        /// <summary>
        /// Обработчик события от <see cref="NataInfo.Nibus.Nms.NmsProtocol.InformationReportReceived"/>.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">The <see cref="NataInfo.Nibus.Nms.NmsInformationReportEventArgs"/> instance containing the event data.</param>
        protected override void OnInformationReportReceived(object sender, NmsInformationReportEventArgs e)
        {
            base.OnInformationReportReceived(sender, e);
            if (e.Identified)
            {
                return;
            }

            if (e.InformationReport.Id == TennisStats)
            {
                var report = e.InformationReport;
                var source = report.Datagram.Source;
                LazyInvokeEvent(TennisStatChanged, () => new TennisStatChangedEventArgs(source, report.GetTennisStat()));
                e.Identified = true;
            }
        }

        #endregion //Implementations
    }
}