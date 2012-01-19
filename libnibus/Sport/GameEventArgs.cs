using System;

namespace NataInfo.Nibus.Sport
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public abstract class BaseInformationReportEventArgs : EventArgs
    {
        public Address Source { get; private set; }

        protected BaseInformationReportEventArgs(Address source)
        {
            Source = source;
        }
    }

    public sealed class TimerChangedEventArgs : BaseInformationReportEventArgs
    {
        public TimerChangedEventArgs(Address source, TimerInfo timer) : base(source)
        {
            Timer = timer;
        }

        public TimerInfo Timer { get; private set; }
    }

    public sealed class ScoreChangedEventArgs : BaseInformationReportEventArgs
    {
        public ScoreChangedEventArgs(Address source, ushort score)
            : base(source)
        {
            Score = score;
        }

        public ushort Score { get; private set; }
    }

    public sealed class PeriodChangedEventArgs : BaseInformationReportEventArgs
    {
        public PeriodChangedEventArgs(Address source, byte period)
            : base(source)
        {
            Period = period;
        }

        public byte Period { get; private set; }
    }

    public sealed class FoulsChangedEventArgs : BaseInformationReportEventArgs
    {
        public FoulsChangedEventArgs(Address source, byte fouls)
            : base(source)
        {
            Fouls = fouls;
        }

        public byte Fouls { get; private set; }
    }

    public sealed class TimebreaksChangedEventArgs : BaseInformationReportEventArgs
    {
        public TimebreaksChangedEventArgs(Address source, byte timebreaks)
            : base(source)
        {
            Timebreaks = timebreaks;
        }

        public byte Timebreaks { get; private set; }
    }

    public sealed class TeamCountChangedEventArgs : BaseInformationReportEventArgs
    {
        public TeamCountChangedEventArgs(Address source, TeamRole role, ushort count)
            : base(source)
        {
            Role = role;
            Count = count;
        }

        public TeamCountChangedEventArgs(Address source, object nmsData)
            : base(source)
        {
            var data = (byte[])nmsData;
            Role = (TeamRole)data[0];
            Count = BitConverter.ToUInt16(data, 1);
        }

        public TeamRole Role { get; private set; }
        public ushort Count { get; private set; }
    }

    public sealed class PlayerInfoChangedEventArgs : BaseInformationReportEventArgs
    {
        public PlayerInfo Info { get; private set; }

        public PlayerInfoChangedEventArgs(Address source, PlayerInfo info) : base(source)
        {
            Info = info;
        }
    }

    public sealed class PlayerStatChangedEventArgs : BaseInformationReportEventArgs
    {
        public PlayerStat Stat { get; private set; }

        public PlayerStatChangedEventArgs(Address source, PlayerStat stat) : base(source)
        {
            Stat = stat;
        }
    }

    public sealed class TeamNameChangedEventArgs : BaseInformationReportEventArgs
    {
        public string TeamName { get; private set; }

        public TeamNameChangedEventArgs(Address source, string teamName) : base(source)
        {
            TeamName = teamName;
        }
    }

    public sealed class CountryChangedEventArgs : BaseInformationReportEventArgs
    {
        public string Country { get; private set; }

        public CountryChangedEventArgs(Address source, string country)
            : base(source)
        {
            Country = country;
        }
    }

    public sealed class TournamentChangedEventArgs : BaseInformationReportEventArgs
    {
        public string Tournament { get; private set; }

        public TournamentChangedEventArgs(Address source, string tournament)
            : base(source)
        {
            Tournament = tournament;
        }
    }

    public sealed class BallOwnerChangedEventArgs : BaseInformationReportEventArgs
    {
        public BallOwner BallOwner { get; private set; }

        public BallOwnerChangedEventArgs(Address source, BallOwner ballOwner) : base(source)
        {
            BallOwner = ballOwner;
        }
    }

    public sealed class ShowInfoMessageEventArgs : BaseInformationReportEventArgs
    {
        public InfoMessage InfoMessage { get; private set; }

        public ShowInfoMessageEventArgs(Address source, InfoMessage infoMessage) : base(source)
        {
            InfoMessage = infoMessage;
        }
    }

    public sealed class SportChangedEventArgs : BaseInformationReportEventArgs
    {
        public ProviderInfo ProviderInfo { get; private set; }

        public SportChangedEventArgs(Address source, ProviderInfo providerInfo) : base(source)
        {
            ProviderInfo = providerInfo;
        }
    }
}