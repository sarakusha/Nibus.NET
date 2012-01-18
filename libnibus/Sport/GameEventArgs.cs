using System;

namespace NataInfo.Nibus.Sport
{
    public abstract class InformationReportEventArgs : EventArgs
    {
        public Address Source { get; private set; }

        public InformationReportEventArgs(Address source)
        {
            Source = source;
        }
    }

    public sealed class TimerEventArgs : InformationReportEventArgs
    {
        public TimerEventArgs(Address source, TimerInfo timer) : base(source)
        {
            Timer = timer;
        }

        public TimerInfo Timer { get; private set; }
    }

    public sealed class ScoreChangedArgs : InformationReportEventArgs
    {
        public ScoreChangedArgs(Address source, ushort score)
            : base(source)
        {
            Score = score;
        }

        public ushort Score { get; private set; }
    }

    public sealed class PeriodChangedArgs : InformationReportEventArgs
    {
        public PeriodChangedArgs(Address source, byte period)
            : base(source)
        {
            Period = period;
        }

        public byte Period { get; private set; }
    }

    public sealed class FoulsChangedArgs : InformationReportEventArgs
    {
        public FoulsChangedArgs(Address source, byte fouls)
            : base(source)
        {
            Fouls = fouls;
        }

        public byte Fouls { get; private set; }
    }

    public sealed class TimebreaksChangedArgs : InformationReportEventArgs
    {
        public TimebreaksChangedArgs(Address source, byte timebreaks)
            : base(source)
        {
            Timebreaks = timebreaks;
        }

        public byte Timebreaks { get; private set; }
    }

    public sealed class TeamCountChangedArgs : InformationReportEventArgs
    {
        public TeamCountChangedArgs(Address source, TeamRole role, ushort count)
            : base(source)
        {
            Role = role;
            Count = count;
        }

        public TeamCountChangedArgs(Address source, object nmsData)
            : base(source)
        {
            var data = (byte[])nmsData;
            Role = (TeamRole)data[0];
            Count = BitConverter.ToUInt16(data, 1);
        }

        public TeamRole Role { get; private set; }
        public ushort Count { get; private set; }
    }

    public sealed class PlayerInfoChangedEventArgs : InformationReportEventArgs
    {
        public PlayerInfo Info { get; private set; }

        public PlayerInfoChangedEventArgs(Address source, PlayerInfo info) : base(source)
        {
            Info = info;
        }
    }
}