//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// GameReceiver.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics.Contracts;
using NataInfo.Nibus.Nms;

#endregion

namespace NataInfo.Nibus.Sport
{
    public class GameReceiver
    {
        #region Member Variables

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public GameReceiver(NmsProtocol nmsProtocol)
        {
            Contract.Requires(nmsProtocol != null);
            NmsProtocol = nmsProtocol;
            NmsProtocol.InformationReportReceived += OnInformationReportReceived;
        }

        #endregion //Constructors

        #region  Events

        public event EventHandler<TimerChangedEventArgs> TimerChanged;
        public event EventHandler<ScoreChangedEventArgs> HomeScoreChanged;
        public event EventHandler<ScoreChangedEventArgs> VisitingScoreChanged;
        public event EventHandler<PeriodChangedEventArgs> PeriodChanged;
        public event EventHandler<FoulsChangedEventArgs> HomeFoulsChanged;
        public event EventHandler<FoulsChangedEventArgs> VisitingFoulsChanged;
        public event EventHandler<TimebreaksChangedEventArgs> HomeTimebreaksChanged;
        public event EventHandler<TimebreaksChangedEventArgs> VisitingTimebreaksChanged;
        public event EventHandler<TeamCountChangedEventArgs> TeamCountChanged;
        public event EventHandler<PlayerInfoChangedEventArgs> PlayerInfoChanged;
        public event EventHandler<PlayerStatChangedEventArgs> PlayerStatChanged;
        public event EventHandler<TeamNameChangedEventArgs> HomeNameChanged;
        public event EventHandler<TeamNameChangedEventArgs> VisitingNameChanged;
        public event EventHandler<CountryChangedEventArgs> HomeCountryChanged;
        public event EventHandler<CountryChangedEventArgs> VisitingCountryChanged;
        public event EventHandler<TournamentChangedEventArgs> TournamentChanged;
        public event EventHandler<BallOwnerChangedEventArgs> BallOwnerChanged;
        public event EventHandler<ShowInfoMessageEventArgs> ShowInfoMessage;
        public event EventHandler<SportChangedEventArgs> SportChanged;
        
        #endregion

        #region Properties

        public NmsProtocol NmsProtocol { get; private set; }

        #endregion //Properties

        #region Methods

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
                case (byte)GameReports.Timer:
                    SafeInvokeEvent(TimerChanged, () => new TimerChangedEventArgs(source, report.GetTimerInfo()));
                    break;
                case (byte)GameReports.HomeTeamScore:
                    SafeInvokeEvent(HomeScoreChanged, () => new ScoreChangedEventArgs(source, (ushort)report.Value));
                    break;
                case (byte)GameReports.VisitingTeamScore:
                    SafeInvokeEvent(VisitingScoreChanged, () => new ScoreChangedEventArgs(source, (ushort)report.Value));
                    break;
                case (byte)GameReports.Period:
                    SafeInvokeEvent(PeriodChanged, () => new PeriodChangedEventArgs(source, (byte)report.Value));
                    break;
                case (byte)GameReports.HomeTeamFoul:
                    SafeInvokeEvent(HomeFoulsChanged, () => new FoulsChangedEventArgs(source, (byte)report.Value));
                    break;
                case (byte)GameReports.VisitingTeamFoul:
                    SafeInvokeEvent(VisitingFoulsChanged, () => new FoulsChangedEventArgs(source, (byte)report.Value));
                    break;
                case (byte)GameReports.HomeTeamTimebreaks:
                    SafeInvokeEvent(HomeTimebreaksChanged, () => new TimebreaksChangedEventArgs(source, (byte)report.Value));
                    break;
                case (byte)GameReports.VisitingTeamTimebreaks:
                    SafeInvokeEvent(VisitingTimebreaksChanged, () => new TimebreaksChangedEventArgs(source, (byte)report.Value));
                    break;
                case (byte)GameReports.TeamCount:
                    SafeInvokeEvent(TeamCountChanged, () => new TeamCountChangedEventArgs(source, report.Value));
                    break;
                case (byte)GameReports.PlayerInfo:
                    SafeInvokeEvent(PlayerInfoChanged, () => new PlayerInfoChangedEventArgs(source, report.GetPlayerInfo()));
                    break;
                case (byte)GameReports.PlayerStat:
                    SafeInvokeEvent(PlayerStatChanged, () => new PlayerStatChangedEventArgs(source, report.GetPlayerStat()));
                    break;
                case (byte)GameReports.HomeTeamName:
                    SafeInvokeEvent(HomeNameChanged, () => new TeamNameChangedEventArgs(source, (string)report.Value));
                    break;
                case (byte)GameReports.VisitingTeamName:
                    SafeInvokeEvent(VisitingNameChanged, () => new TeamNameChangedEventArgs(source, (string)report.Value));
                    break;
                case (byte)GameReports.HomeTeamCountry:
                    SafeInvokeEvent(HomeCountryChanged, () => new CountryChangedEventArgs(source, (string)report.Value));
                    break;
                case (byte)GameReports.VisitingTeamCountry:
                    SafeInvokeEvent(VisitingCountryChanged, () => new CountryChangedEventArgs(source, (string)report.Value));
                    break;
                case (byte)GameReports.TournamentName:
                    SafeInvokeEvent(TournamentChanged, () => new TournamentChangedEventArgs(source, (string)report.Value));
                    break;
                case (byte)GameReports.BallOwner:
                    SafeInvokeEvent(BallOwnerChanged, () => new BallOwnerChangedEventArgs(source, (BallOwner)(byte)report.Value));
                    break;
                case (byte)GameReports.ShowMessage:
                    SafeInvokeEvent(ShowInfoMessage, () => new ShowInfoMessageEventArgs(source, report.GetInfoMessage()));
                    break;
                case (byte)GameReports.ChangeSport:
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
