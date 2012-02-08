using System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// Базовый абстрактный класс для сведений о событии получения спортивных информационных сообщений.
    /// </summary>
    public abstract class BaseInformationReportEventArgs : EventArgs
    {
        /// <summary>
        /// Возвращает адрес источника сообщения.
        /// </summary>
        public Address Source { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseInformationReportEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника сообщения.</param>
        protected BaseInformationReportEventArgs(Address source)
        {
            Source = source;
        }
    }

    /// <summary>
    /// Содержит сведения об изменении таймера.
    /// </summary>
    public sealed class TimerChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimerChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="timer">Информация таймера.</param>
        public TimerChangedEventArgs(Address source, TimerInfo timer) : base(source)
        {
            Timer = timer;
        }

        /// <summary>
        /// Возвращает информацию о таймере.
        /// </summary>
        public TimerInfo Timer { get; private set; }
    }

    /// <summary>
    /// Содержит сведения об изменении счета.
    /// </summary>
    public sealed class ScoreChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="score">Количество очков.</param>
        public ScoreChangedEventArgs(Address source, ushort score)
            : base(source)
        {
            Score = score;
        }

        /// <summary>
        /// Возвращает количество очков.
        /// </summary>
        public ushort Score { get; private set; }
    }

    /// <summary>
    /// Содержит сведения об изменении периода.
    /// </summary>
    public sealed class PeriodChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="period">Период.</param>
        public PeriodChangedEventArgs(Address source, byte period)
            : base(source)
        {
            Period = period;
        }

        /// <summary>
        /// Возвращает период.
        /// </summary>
        public byte Period { get; private set; }
    }

    /// <summary>
    /// Содержит сведенияоб изменении 
    /// </summary>
    public sealed class FoulsChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FoulsChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="fouls">Количество фолов.</param>
        public FoulsChangedEventArgs(Address source, byte fouls)
            : base(source)
        {
            Fouls = fouls;
        }

        /// <summary>
        /// Возвращает количество фолов.
        /// </summary>
        public byte Fouls { get; private set; }
    }

    /// <summary>
    /// Содержит сведения об изменении 
    /// </summary>
    public sealed class TimebreaksChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimebreaksChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="timebreaks">Количество перерывов.</param>
        public TimebreaksChangedEventArgs(Address source, byte timebreaks)
            : base(source)
        {
            Timebreaks = timebreaks;
        }

        /// <summary>
        /// Возвращает количество перерывов.
        /// </summary>
        public byte Timebreaks { get; private set; }
    }

    /// <summary>
    /// Содержит сведения об изменении 
    /// </summary>
    public sealed class TeamCountChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCountChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="role">Принадлежность команды.</param>
        /// <param name="count">Количество членов команды.</param>
        public TeamCountChangedEventArgs(Address source, TeamRole role, ushort count)
            : base(source)
        {
            Role = role;
            Count = count;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCountChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="nmsData">Данные NMS.</param>
        internal TeamCountChangedEventArgs(Address source, object nmsData)
            : base(source)
        {
            var data = (byte[])nmsData;
            Role = (TeamRole)data[0];
            Count = BitConverter.ToUInt16(data, 1);
        }

        /// <summary>
        /// Возвращает принадлежность команды.
        /// </summary>
        public TeamRole Role { get; private set; }
        /// <summary>
        /// Возвращает количество членов команды.
        /// </summary>
        public ushort Count { get; private set; }
    }

    /// <summary>
    /// Содержит сведения об изменении информации об игроке. 
    /// </summary>
    public sealed class PlayerInfoChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Возвращает информацию об игроке.
        /// </summary>
        public PlayerInfo Info { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerInfoChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="info">Информация об игроке.</param>
        public PlayerInfoChangedEventArgs(Address source, PlayerInfo info)
            : base(source)
        {
            Info = info;
        }
    }

    /// <summary>
    /// Содержит сведения об изменении статистики игрока (баскетбол).
    /// </summary>
    public sealed class PlayerStatChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Возвращает статистику по игроку.
        /// </summary>
        public PlayerStat Stat { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStatChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="stat">Статистика по игрку.</param>
        public PlayerStatChangedEventArgs(Address source, PlayerStat stat)
            : base(source)
        {
            Stat = stat;
        }
    }

    /// <summary>
    /// Содержит сведения об изменении названия команды.
    /// </summary>
    public sealed class TeamNameChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Возвращает название команды.
        /// </summary>
        public string TeamName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamNameChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="teamName">Название команды.</param>
        public TeamNameChangedEventArgs(Address source, string teamName)
            : base(source)
        {
            TeamName = teamName;
        }
    }

    /// <summary>
    /// Содержит сведения об изменении страны/города у команды.
    /// </summary>
    public sealed class CountryChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Возвращает страну/город у команды.
        /// </summary>
        public string Country { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="country">Название страны/города.</param>
        public CountryChangedEventArgs(Address source, string country)
            : base(source)
        {
            Country = country;
        }
    }

    /// <summary>
    /// Содержит сведения об изменении названии турнира.
    /// </summary>
    public sealed class TournamentChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Возвращает название турнира.
        /// </summary>
        public string Tournament { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="tournament">Название турнира.</param>
        public TournamentChangedEventArgs(Address source, string tournament)
            : base(source)
        {
            Tournament = tournament;
        }
    }

    /// <summary>
    /// Содержит сведения об изменении владения мячом.
    /// </summary>
    public sealed class BallOwnerChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Возвращает индикатор влядения мячом.
        /// </summary>
        public BallOwner BallOwner { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BallOwnerChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="ballOwner">Владаение мячом.</param>
        public BallOwnerChangedEventArgs(Address source, BallOwner ballOwner)
            : base(source)
        {
            BallOwner = ballOwner;
        }
    }

    /// <summary>
    /// Содержит сведения о необходимости вывода информационного сообщения на экран.
    /// </summary>
    public sealed class ShowInfoMessageEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Возвращает информационное сообщение.
        /// </summary>
        public InfoMessage InfoMessage { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowInfoMessageEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="infoMessage">Информационное сообщение.</param>
        public ShowInfoMessageEventArgs(Address source, InfoMessage infoMessage)
            : base(source)
        {
            InfoMessage = infoMessage;
        }
    }

    /// <summary>
    /// Содержит сведения об изменении провайдера игры (вида спорта).
    /// </summary>
    public sealed class SportChangedEventArgs : BaseInformationReportEventArgs
    {
        /// <summary>
        /// Возвращает провайдера игры.
        /// </summary>
        public ProviderInfo ProviderInfo { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SportChangedEventArgs"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="providerInfo">Провайдер игры.</param>
        public SportChangedEventArgs(Address source, ProviderInfo providerInfo)
            : base(source)
        {
            ProviderInfo = providerInfo;
        }
    }
}