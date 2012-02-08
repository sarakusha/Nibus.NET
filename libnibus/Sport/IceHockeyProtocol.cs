//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// IceHockeyProtocol.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Nms.Variables;

#endregion

namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// Протокол игры для Хоккея на льду.
    /// </summary>
    public sealed class IceHockeyProtocol : GameProtocol
    {
        #region Member Variables

        private static ProviderInfo _defaultProvider;
        private ProviderInfo _provider;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        /// <param name="nmsProtocol">The NMS protocol.</param>
        public IceHockeyProtocol(NmsProtocol nmsProtocol) : base(nmsProtocol)
        {
        }

        #endregion //Constructors

        /// <summary>
        /// Происходит при изменении статистики по удаленным игрокам.
        /// </summary>
        public event EventHandler<PenaltyStatChangedEventArgs> PenaltyStatChanged;

        #region Properties

        /// <summary>
        /// Возвращает провайдера игры.
        /// </summary>
        public override ProviderInfo Provider
        {
            get { return _provider ?? (_provider = new ProviderInfo(DefaultProvider)); }
        }

        /// <summary>
        /// Возвращает провайдера игры по умолчанию.
        /// </summary>
        public static ProviderInfo DefaultProvider
        {
            get
            {
                return _defaultProvider ??
                       (_defaultProvider = new ProviderInfo(
                                        (ushort)Providers.IceHockey,
                                        new TimerAttributes(
                                            1, 1200, true, true, false, false, true, true, false, false, "Игра"),
                                        new TimerAttributes(
                                            2, 900, false, false, false, false, true, false, true, false, "Перерыв"),
                                        new TimerAttributes(
                                            3, 3600, false, false, false, false, true, false, true, false, "Время до начала"),
                                        new TimerAttributes(
                                            4, 30, false, false, false, false, true, false, true, true, "Таймаут"),
                                        new TimerAttributes(
                                            5, 600, true, true, false, false, true, true, false, false, "Овертайм"),
                                        new TimerAttributes(
                                            6, 120, false, false, false, false, true, false, true, false, "Перерыв овертайма")));
            }
        }

        /// <summary>
        /// Загружает с универсального пульта длительность таймеров и обновляет провайдера игры.
        /// </summary>
        /// <param name="uconsole">Адрес пульта или <c>null</c>.</param>
        /// <remarks>Асинхронная операция.</remarks>
        public async void LoadProvider(Address uconsole = null)
        {
            if (uconsole == null)
            {
                uconsole = Address.Empty;
            }

            var timerInfos = new List<ReadProgressInfo>(12);
            var options = new NibusOptions
                                {
                                    Progress = new Progress<object>(o => timerInfos.Add((ReadProgressInfo)o))
                                };

            await NmsProtocol.ReadManyValuesAsync(
                options,
                uconsole,
                (int)Uconsole.Timer1Min,
                (int)Uconsole.Timer1Sec,
                (int)Uconsole.Timer2Min,
                (int)Uconsole.Timer2Sec,
                (int)Uconsole.Timer3Min,
                (int)Uconsole.Timer3Sec,
                (int)Uconsole.Timer4Min,
                (int)Uconsole.Timer4Sec,
                (int)Uconsole.Timer5Min,
                (int)Uconsole.Timer5Sec,
                (int)Uconsole.Timer6Min,
                (int)Uconsole.Timer6Sec);

            foreach (var progressInfo in timerInfos.Where(pi => !pi.IsFaulted && !pi.IsCanceled))
            {
                switch (progressInfo.Id)
                {
                    case (int)Uconsole.Timer1Min:
                        Provider.Timers[0].Duration = (uint)(Provider.Timers[0].Duration%60 + (byte)progressInfo.Value*60);
                        break;
                    case (int)Uconsole.Timer1Sec:
                        Provider.Timers[0].Duration = (Provider.Timers[0].Duration / 60)*60 + (byte)progressInfo.Value;
                        break;
                    case (int)Uconsole.Timer2Min:
                        Provider.Timers[1].Duration = (uint)(Provider.Timers[0].Duration%60 + (byte)progressInfo.Value*60);
                        break;
                    case (int)Uconsole.Timer2Sec:
                        Provider.Timers[1].Duration = (Provider.Timers[0].Duration / 60)*60 + (byte)progressInfo.Value;
                        break;
                    case (int)Uconsole.Timer3Min:
                        Provider.Timers[2].Duration = (uint)(Provider.Timers[0].Duration%60 + (byte)progressInfo.Value*60);
                        break;
                    case (int)Uconsole.Timer3Sec:
                        Provider.Timers[2].Duration = (Provider.Timers[0].Duration / 60)*60 + (byte)progressInfo.Value;
                        break;
                    case (int)Uconsole.Timer4Min:
                        Provider.Timers[3].Duration = (uint)(Provider.Timers[0].Duration%60 + (byte)progressInfo.Value*60);
                        break;
                    case (int)Uconsole.Timer4Sec:
                        Provider.Timers[3].Duration = (Provider.Timers[0].Duration / 60)*60 + (byte)progressInfo.Value;
                        break;
                    case (int)Uconsole.Timer5Min:
                        Provider.Timers[4].Duration = (uint)(Provider.Timers[0].Duration%60 + (byte)progressInfo.Value*60);
                        break;
                    case (int)Uconsole.Timer5Sec:
                        Provider.Timers[4].Duration = (Provider.Timers[0].Duration / 60)*60 + (byte)progressInfo.Value;
                        break;
                    case (int)Uconsole.Timer6Min:
                        Provider.Timers[5].Duration = (uint)(Provider.Timers[0].Duration%60 + (byte)progressInfo.Value*60);
                        break;
                    case (int)Uconsole.Timer6Sec:
                        Provider.Timers[5].Duration = (Provider.Timers[0].Duration / 60)*60 + (byte)progressInfo.Value;
                        break;
                }
            }
        }


        #endregion //Properties

        #region Methods

        /// <summary>
        /// Оповестить об изменении статистики по удаленным игрокам.
        /// </summary>
        /// <param name="penaltyStat">Статистика по удалениям.</param>
        public void FirePenaltyStatChanged(PenaltyStat penaltyStat)
        {
            FireInformationReport(penaltyStat.CreateInformationReport());
        }

        #endregion //Methods

        #region Overrides of GameProtocol

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

            if (e.InformationReport.Id == PenaltyStat.PenaltyStatId)
            {
                var report = e.InformationReport;
                var source = report.Datagram.Source;
                LazyInvokeEvent(PenaltyStatChanged, () => new PenaltyStatChangedEventArgs(source, report.GetPenaltyStat()));
                e.Identified = true;
            }
        }
        #endregion
    }
}
