﻿//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// GameProtocol.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks.Dataflow;
using NataInfo.Nibus.Nms;
using NataInfo.Nibus.Nms.Services;

#endregion

// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// Базовый протокол для всех видов спорта.
    /// </summary>
    public abstract class GameProtocol
    {
        #region Member Variables

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        /// <param name="nmsProtocol">The NMS protocol.</param>
        protected GameProtocol(NmsProtocol nmsProtocol)
        {
            Contract.Requires(nmsProtocol != null);
            NmsProtocol = nmsProtocol;
            NmsProtocol.InformationReportReceived += OnInformationReportReceived;
        }

        #endregion //Constructors

        #region  Events

        /// <summary>
        /// Происходит при изменении таймера.
        /// </summary>
        public event EventHandler<TimerChangedEventArgs> TimerChanged;

        /// <summary>
        /// Происходит при изменении счета команды хозяев.
        /// </summary>
        public event EventHandler<ScoreChangedEventArgs> HomeScoreChanged;

        /// <summary>
        /// Происходит при изменении счета команды гостей.
        /// </summary>
        public event EventHandler<ScoreChangedEventArgs> VisitorScoreChanged;

        /// <summary>
        /// Происходит при изменении периода.
        /// </summary>
        public event EventHandler<PeriodChangedEventArgs> PeriodChanged;

        /// <summary>
        /// Происходит при изменении счета команды хозяев.
        /// </summary>
        public event EventHandler<FoulsChangedEventArgs> HomeFoulsChanged;

        /// <summary>
        /// Происходит при изменении счета команды гостей.
        /// </summary>
        public event EventHandler<FoulsChangedEventArgs> VisitorFoulsChanged;

        /// <summary>
        /// Происходит при изменении количества фолов команды хозяев.
        /// </summary>
        public event EventHandler<TimebreaksChangedEventArgs> HomeTimebreaksChanged;

        /// <summary>
        /// Происходит при изменении количества фолов команды гостей.
        /// </summary>
        public event EventHandler<TimebreaksChangedEventArgs> VisitorTimebreaksChanged;

        /// <summary>
        /// Происходит при изменении количества игроков в команде.
        /// </summary>
        public event EventHandler<TeamCountChangedEventArgs> TeamCountChanged;

        /// <summary>
        /// Происходит при изменении информации об игроке.
        /// </summary>
        public event EventHandler<PlayerInfoChangedEventArgs> PlayerInfoChanged;

        /// <summary>
        /// Происходит при изменении статистики по игроку.
        /// </summary>
        public event EventHandler<PlayerStatChangedEventArgs> PlayerStatChanged;

        /// <summary>
        ///Происходит при изменении названия команды хозяев.
        /// </summary>
        public event EventHandler<TeamNameChangedEventArgs> HomeNameChanged;

        /// <summary>
        /// Происходит при изменении названия команды гостей.
        /// </summary>
        public event EventHandler<TeamNameChangedEventArgs> VisitorNameChanged;

        /// <summary>
        /// Происходит при изменении страны/города команды хозяев.
        /// </summary>
        public event EventHandler<CountryChangedEventArgs> HomeCountryChanged;

        /// <summary>
        /// Происходит при изменении страны/города команды гостей.
        /// </summary>
        public event EventHandler<CountryChangedEventArgs> VisitorCountryChanged;

        /// <summary>
        /// Происходит при изменении названия турнира.
        /// </summary>
        public event EventHandler<TournamentChangedEventArgs> TournamentChanged;

        /// <summary>
        /// Происходит при изменении владении мячом (очередью подачи).
        /// </summary>
        public event EventHandler<BallOwnerChangedEventArgs> BallOwnerChanged;

        /// <summary>
        /// Происходит при получении требования вывода информационного сообщения.
        /// </summary>
        public event EventHandler<ShowInfoMessageEventArgs> ShowInfoMessage;

        /// <summary>
        /// Происходит при смене провайдера игры (смене вида спорта).
        /// </summary>
        public event EventHandler<SportChangedEventArgs> SportChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Возвращает NMS-протокол.
        /// </summary>
        public NmsProtocol NmsProtocol { get; private set; }

        public abstract ProviderInfo Provider { get; }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Передать информационное сообщение.
        /// </summary>
        /// <param name="informationReport">Информационное сообщение.</param>
        protected void FireInformationReport(NmsInformationReport informationReport)
        {
            NmsProtocol.OutgoingMessages.Post(informationReport);
        }

        /// <summary>
        /// Передать событие таймера.
        /// </summary>
        /// <param name="timerInfo">Данные таймера.</param>
        public void FireTimerChaged(TimerInfo timerInfo)
        {
            FireInformationReport(NmsTimerExtentions.Create(Address.Empty, timerInfo));
        }

        /// <summary>
        /// Оповестить об изменении счета команды хозяев.
        /// </summary>
        /// <param name="homeScore">Счет команды хозяев.</param>
        public void FireHomeScoreChanged(ushort homeScore)
        {
            FireInformationReport(
                new NmsInformationReport(Address.Empty, (int)GameReports.HomeTeamScore, NmsValueType.UInt16, homeScore));
        }

        /// <summary>
        /// Оповестить об изменении счета команды гостей.
        /// </summary>
        /// <param name="visitorScore">Счет команды гостей.</param>
        public void FireVisitorScoreChanged(ushort visitorScore)
        {
            FireInformationReport(
                new NmsInformationReport(
                    Address.Empty, (int)GameReports.VisitingTeamScore, NmsValueType.UInt16, visitorScore));
        }

        /// <summary>
        /// Оповестить об изменении периода.
        /// </summary>
        /// <param name="period">Период.</param>
        public void FirePeriodChanged(byte period)
        {
            FireInformationReport(
                new NmsInformationReport(Address.Empty, (int)GameReports.Period, NmsValueType.UInt8, period));
        }

        /// <summary>
        /// Оповестить об изменении фолов команды хозяев.
        /// </summary>
        /// <param name="homeFouls">Количество фолов.</param>
        public void FireHomeFoulsChanged(byte homeFouls)
        {
            FireInformationReport(
                new NmsInformationReport(Address.Empty, (int)GameReports.HomeTeamFouls, NmsValueType.UInt8, homeFouls));
        }

        /// <summary>
        /// Оповестить об изменении фолов команды гостей.
        /// </summary>
        /// <param name="visitorFouls">Количество фолов.</param>
        public void FireVisitorFoulsChanged(byte visitorFouls)
        {
            FireInformationReport(
                new NmsInformationReport(
                    Address.Empty, (int)GameReports.VisitingTeamFouls, NmsValueType.UInt8, visitorFouls));
        }

        /// <summary>
        /// Оповестить об изменении перерывов команды хозяев.
        /// </summary>
        /// <param name="homeTimebreaks">Количество перерывов.</param>
        public void FireHomeTimebreaksChanged(byte homeTimebreaks)
        {
            FireInformationReport(
                new NmsInformationReport(
                    Address.Empty, (int)GameReports.HomeTeamTimebreaks, NmsValueType.UInt8, homeTimebreaks));
        }

        /// <summary>
        /// Оповестить об изменении перерывов команды гостей.
        /// </summary>
        /// <param name="visitorTimebreaks">Количество перерывов.</param>
        public void FireVisitorTimebreaksChanged(byte visitorTimebreaks)
        {
            FireInformationReport(
                new NmsInformationReport(
                    Address.Empty, (int)GameReports.VisitingTeamTimebreaks, NmsValueType.UInt8, visitorTimebreaks));
        }

        /// <summary>
        /// Оповестить об изменении количества членов команды.
        /// </summary>
        /// <param name="role">Принадлежность к команде.</param>
        /// <param name="count">Количество игроков.</param>
        public void FireTeamCountChanged(TeamRole role, ushort count)
        {
            var data = new byte[3];
            data[0] = (byte)role;
            BitConverter.GetBytes(count).CopyTo(data, 1);
            FireInformationReport(
                new NmsInformationReport(Address.Empty, (int)GameReports.TeamCount, NmsValueType.UInt8Array, data));
        }

        /// <summary>
        /// Оповестить об изменении информации об игроке.
        /// </summary>
        /// <param name="playerInfo">Информация об игроке.</param>
        public void FirePlayerInfo(PlayerInfo playerInfo)
        {
            FireInformationReport(PlayerInfoExtensions.Create(Address.Empty, playerInfo));
        }

        /// <summary>
        /// Оповестить об изменении статистики игрока.
        /// </summary>
        /// <param name="playerStat">Статистика игрока.</param>
        public void FirePlayerStat(PlayerStat playerStat)
        {
            FireInformationReport(PlayerStatExtensions.Create(Address.Empty, playerStat));
        }

        /// <summary>
        /// Оповестить об изменении названия команды хозяев.
        /// </summary>
        /// <param name="homeName">Название команды.</param>
        public void FireHomeNameChanged(string homeName)
        {
            FireInformationReport(
                new NmsInformationReport(Address.Empty, (int)GameReports.HomeTeamName, NmsValueType.String, homeName));
        }

        /// <summary>
        /// Оповестить об изменении названия команды гостей.
        /// </summary>
        /// <param name="visitorName">Название команды.</param>
        public void FireVisitorNameChanged(string visitorName)
        {
            FireInformationReport(
                new NmsInformationReport(Address.Empty, (int)GameReports.VisitingTeamName, NmsValueType.String, visitorName));
        }

        /// <summary>
        /// Оповестить об изменении страны/города команды хозяев.
        /// </summary>
        /// <param name="homeCountry">Название страны/города.</param>
        public void FireHomeCountryChanged(string homeCountry)
        {
            FireInformationReport(
                new NmsInformationReport(Address.Empty, (int)GameReports.HomeTeamCountry, NmsValueType.String, homeCountry));
        }

        /// <summary>
        /// Оповестить об изменении страны/города команды гостей.
        /// </summary>
        /// <param name="visitorCountry">Название страны/города.</param>
        public void FireVisitorCountryChanged(string visitorCountry)
        {
            FireInformationReport(
                new NmsInformationReport(Address.Empty, (int)GameReports.VisitingTeamCountry, NmsValueType.String, visitorCountry));
        }

        /// <summary>
        /// Оповестить об изменении названия турнира.
        /// </summary>
        /// <param name="tournamentName">Название турнира.</param>
        public void FireTournamentChaged(string tournamentName)
        {
            FireInformationReport(
                new NmsInformationReport(Address.Empty, (int)GameReports.TournamentName, NmsValueType.String, tournamentName));
        }

        /// <summary>
        /// Оповестить об изменении владением мячом (очереью подачи).
        /// </summary>
        /// <param name="ballOwner">Индикатор владаения мячом (очередность подачи).</param>
        public void FireBallOwnerChanged(BallOwner ballOwner)
        {
            FireInformationReport(
                new NmsInformationReport(Address.Empty, (int)GameReports.BallOwner, NmsValueType.UInt8, (byte)ballOwner));
        }

        /// <summary>
        /// Оповестить о необходимости вывода информационного сообщения.
        /// </summary>
        /// <param name="infoMessage">Информационное сообщение.</param>
        public void FireShowInfoMessage(InfoMessage infoMessage)
        {
            FireInformationReport(InfoMessageExtensions.Create(Address.Empty, infoMessage));
        }

        /// <summary>
        /// Оповестить о смене провайдера игры (смене спорта).
        /// </summary>
        /// <seealso cref="Provider"/>
        public void FireSportChanged()
        {
            FireInformationReport(ProviderExtensions.Create(Address.Empty, Provider));
        }

        #endregion //Methods

        #region Implementations

        protected void SafeInvokeEvent<T>(EventHandler<T> eventHandler, Func<T> getArgs) where T : EventArgs
        {
            if (eventHandler != null)
            {
                eventHandler(this, getArgs());
            }
        }

        protected virtual void OnInformationReportReceived(object sender, NmsInformationReportEventArgs e)
        {
            var report = e.InformationReport;
            var source = report.Datagram.Source;
            e.Identified = true;
            switch (report.Id)
            {
                case (int)GameReports.Timer:
                    SafeInvokeEvent(TimerChanged, () => new TimerChangedEventArgs(source, report.GetTimerInfo()));
                    break;
                case (int)GameReports.HomeTeamScore:
                    SafeInvokeEvent(HomeScoreChanged, () => new ScoreChangedEventArgs(source, (ushort)report.Value));
                    break;
                case (int)GameReports.VisitingTeamScore:
                    SafeInvokeEvent(VisitorScoreChanged, () => new ScoreChangedEventArgs(source, (ushort)report.Value));
                    break;
                case (int)GameReports.Period:
                    SafeInvokeEvent(PeriodChanged, () => new PeriodChangedEventArgs(source, (byte)report.Value));
                    break;
                case (int)GameReports.HomeTeamFouls:
                    SafeInvokeEvent(HomeFoulsChanged, () => new FoulsChangedEventArgs(source, (byte)report.Value));
                    break;
                case (int)GameReports.VisitingTeamFouls:
                    SafeInvokeEvent(VisitorFoulsChanged, () => new FoulsChangedEventArgs(source, (byte)report.Value));
                    break;
                case (int)GameReports.HomeTeamTimebreaks:
                    SafeInvokeEvent(
                        HomeTimebreaksChanged, () => new TimebreaksChangedEventArgs(source, (byte)report.Value));
                    break;
                case (int)GameReports.VisitingTeamTimebreaks:
                    SafeInvokeEvent(
                        VisitorTimebreaksChanged, () => new TimebreaksChangedEventArgs(source, (byte)report.Value));
                    break;
                case (int)GameReports.TeamCount:
                    SafeInvokeEvent(TeamCountChanged, () => new TeamCountChangedEventArgs(source, report.Value));
                    break;
                case (int)GameReports.PlayerInfo:
                    SafeInvokeEvent(
                        PlayerInfoChanged, () => new PlayerInfoChangedEventArgs(source, report.GetPlayerInfo()));
                    break;
                case (int)GameReports.PlayerStat:
                    SafeInvokeEvent(
                        PlayerStatChanged, () => new PlayerStatChangedEventArgs(source, report.GetPlayerStat()));
                    break;
                case (int)GameReports.HomeTeamName:
                    SafeInvokeEvent(HomeNameChanged, () => new TeamNameChangedEventArgs(source, (string)report.Value));
                    break;
                case (int)GameReports.VisitingTeamName:
                    SafeInvokeEvent(
                        VisitorNameChanged, () => new TeamNameChangedEventArgs(source, (string)report.Value));
                    break;
                case (int)GameReports.HomeTeamCountry:
                    SafeInvokeEvent(HomeCountryChanged, () => new CountryChangedEventArgs(source, (string)report.Value));
                    break;
                case (int)GameReports.VisitingTeamCountry:
                    SafeInvokeEvent(
                        VisitorCountryChanged, () => new CountryChangedEventArgs(source, (string)report.Value));
                    break;
                case (int)GameReports.TournamentName:
                    SafeInvokeEvent(
                        TournamentChanged, () => new TournamentChangedEventArgs(source, (string)report.Value));
                    break;
                case (int)GameReports.BallOwner:
                    SafeInvokeEvent(
                        BallOwnerChanged, () => new BallOwnerChangedEventArgs(source, (BallOwner)(byte)report.Value));
                    break;
                case (int)GameReports.ShowMessage:
                    SafeInvokeEvent(
                        ShowInfoMessage, () => new ShowInfoMessageEventArgs(source, report.GetInfoMessage()));
                    break;
                case (int)GameReports.ChangeSport:
                    SafeInvokeEvent(SportChanged, () => new SportChangedEventArgs(source, report.GetProviderInfo()));
                    break;
                default:
                    e.Identified = false;
                    break;
            }
        }

        #endregion //Implementations
    }
}