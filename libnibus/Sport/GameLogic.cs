//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// GameLogic.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics.Contracts;
using NataInfo.Nibus.Nms;

#endregion

namespace NataInfo.Nibus.Sport
{
    public class GameLogic
    {
        #region Member Variables

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public GameLogic(NmsController nmsController)
        {
            Contract.Requires(nmsController != null);
            NmsController = nmsController;
            NmsController.InformationReportReceived += OnInformationReportReceived;
        }

        #endregion //Constructors

        #region  Events

        public event EventHandler<TimerEventArgs> TimerChanged;
        public event EventHandler<ScoreChangedArgs> HomeScoreChanged;
        public event EventHandler<ScoreChangedArgs> VisitingScoreChanged;
        public event EventHandler<PeriodChangedArgs> PeriodChanged;
        public event EventHandler<FoulsChangedArgs> HomeFoulsChanged;
        public event EventHandler<FoulsChangedArgs> VisitingFoulsChanged;
        public event EventHandler<TimebreaksChangedArgs> HomeTimebreaksChanged;
        public event EventHandler<TimebreaksChangedArgs> VisitingTimebreaksChanged;
        public event EventHandler<TeamCountChangedArgs> TeamCountChanged;
        public event EventHandler<PlayerInfoChangedEventArgs> PlayerInfoChanged;
        
        #endregion

        #region Properties

        public NmsController NmsController { get; private set; }

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
            switch (report.Id)
            {
                case (byte)GameReports.Timer:
                    SafeInvokeEvent(TimerChanged, () => new TimerEventArgs(source, report.GetTimerInfo()));
                    break;
                case (byte)GameReports.HomeTeamScore:
                    SafeInvokeEvent(HomeScoreChanged, () => new ScoreChangedArgs(source, (ushort)report.Value));
                    break;
                case (byte)GameReports.VisitingTeamScore:
                    SafeInvokeEvent(VisitingScoreChanged, () => new ScoreChangedArgs(source, (ushort)report.Value));
                    break;
                case (byte)GameReports.Period:
                    SafeInvokeEvent(PeriodChanged, () => new PeriodChangedArgs(source, (byte)report.Value));
                    break;
                case (byte)GameReports.HomeTeamFoul:
                    SafeInvokeEvent(HomeFoulsChanged, () => new FoulsChangedArgs(source, (byte)report.Value));
                    break;
                case (byte)GameReports.VisitingTeamFoul:
                    SafeInvokeEvent(VisitingFoulsChanged, () => new FoulsChangedArgs(source, (byte)report.Value));
                    break;
                case (byte)GameReports.HomeTeamTimebreaks:
                    SafeInvokeEvent(HomeTimebreaksChanged, () => new TimebreaksChangedArgs(source, (byte)report.Value));
                    break;
                case (byte)GameReports.VisitingTeamTimebreaks:
                    SafeInvokeEvent(VisitingTimebreaksChanged, () => new TimebreaksChangedArgs(source, (byte)report.Value));
                    break;
                case (byte)GameReports.TeamCount:
                    SafeInvokeEvent(TeamCountChanged, () => new TeamCountChangedArgs(source, report.Value));
                    break;
                case (byte)GameReports.PlayerInfo:
                    SafeInvokeEvent(PlayerInfoChanged, () => new PlayerInfoChangedEventArgs(source, report.GetPlayerInfo()));
                    break;
                case (byte)GameReports.PlayerStat:
                    SafeInvokeEvent(, () => new );
                    break;
                case (byte)GameReports.HomeTeamName:
                    SafeInvokeEvent(, () => new );
                    break;
                case (byte)GameReports.VisitingTeamName:
                    SafeInvokeEvent(, () => new );
                    break;
                case (byte)GameReports.HomeTeamCountry:
                    SafeInvokeEvent(, () => new );
                    break;
                case (byte)GameReports.VisitingTeamCountry:
                    SafeInvokeEvent(, () => new );
                    break;
                case (byte)GameReports.TournamentName:
                    SafeInvokeEvent(, () => new );
                    break;
                case (byte)GameReports.BallOwner:
                    SafeInvokeEvent(, () => new );
                    break;
                case (byte)GameReports.ShowMessage:
                    SafeInvokeEvent(, () => new );
                    break;
                case (byte)GameReports.ChangeSport:
                    SafeInvokeEvent(, () => new );
                    break;
            }
        }

        #endregion //Implementations
    }
}
