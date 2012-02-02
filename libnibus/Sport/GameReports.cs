namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// Общие игровые события.
    /// </summary>
    public enum GameReports
    {
        /// <summary>
        /// Событие таймера.
        /// </summary>
        Timer = 5,

        /// <summary>
        /// Счет команды хозяев.
        /// </summary>
        HomeTeamScore = 6,

        /// <summary>
        /// Счет команды гостей.
        /// </summary>
        VisitingTeamScore = 7,

        /// <summary>
        /// Период.
        /// </summary>
        Period = 8,

        /// <summary>
        /// Количество фолов команды хозяев.
        /// </summary>
        HomeTeamFouls = 9,

        /// <summary>
        /// Количества фолов команды гостей.
        /// </summary>
        VisitingTeamFouls = 10,

        /// <summary>
        /// Количество перерывов команды хозяев.
        /// </summary>
        HomeTeamTimebreaks = 14,

        /// <summary>
        /// Количество перерывов команды гостей.
        /// </summary>
        VisitingTeamTimebreaks = 15,

        /// <summary>
        /// Количество игроков в команде.
        /// </summary>
        TeamCount = 16,

        /// <summary>
        /// Информация о игроке.
        /// </summary>
        PlayerInfo = 17,

        /// <summary>
        /// Статистика по игроку.
        /// </summary>
        PlayerStat = 18,

        /// <summary>
        /// Название команды хозяев.
        /// </summary>
        HomeTeamName = 19,

        /// <summary>
        /// Название команды гостей.
        /// </summary>
        VisitingTeamName = 20,

        /// <summary>
        /// Город/страна команды хозяев.
        /// </summary>
        HomeTeamCountry = 21,

        /// <summary>
        /// Город/страна команды гостей.
        /// </summary>
        VisitingTeamCountry = 22,

        /// <summary>
        /// Название турнира.
        /// </summary>
        TournamentName = 23,

        /// <summary>
        /// Указатель владения мячом.
        /// </summary>
        BallOwner = 24,

        /// <summary>
        /// Вывод информационного сообщения.
        /// </summary>
        ShowMessage = 25,

        /// <summary>
        /// Переключение провайдера игры (смена вида спорта).
        /// </summary>
        ChangeSport = 27
    }

    /// <summary>
    /// Принадлежность к команде.
    /// </summary>
    public enum TeamRole : byte
    {
        /// <summary>
        /// Команда хозяев.
        /// </summary>
        Home = 0,

        /// <summary>
        /// Команда гостей.
        /// </summary>
        Visitor = 1,

        /// <summary>
        /// Судья.
        /// </summary>
        Judge = 2
    }

    /// <summary>
    /// Функция члена команды.
    /// </summary>
    public enum PlayerFunction : byte
    {
        /// <summary>
        /// Обычный игрок.
        /// </summary>
        Team = 0,

        /// <summary>
        /// Запасной игрок.
        /// </summary>
        Reserve = 1,

        /// <summary>
        /// Капитан команды.
        /// </summary>
        Captain = 2,

        /// <summary>
        /// Тренер.
        /// </summary>
        Coach = 3
    }

    /// <summary>
    /// Индикатор владения мячом (очередность подачи).
    /// </summary>
    public enum BallOwner : byte
    {
        /// <summary>
        /// Скрыть индикатор подачи (очередность неопределена).
        /// </summary>
        Hidden = 0,

        /// <summary>
        /// Мяч (подача) у команды хозаев.
        /// </summary>
        Home = 1,

        /// <summary>
        /// Мяч (подача) у команды гостей.
        /// </summary>
        Visitor = 2
    }
}