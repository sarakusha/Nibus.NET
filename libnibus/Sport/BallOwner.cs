namespace NataInfo.Nibus.Sport
{
    /// <summary>
    /// »ндикатор владени€ м€чом (очередность подачи).
    /// </summary>
    public enum BallOwner : byte
    {
        /// <summary>
        /// —крыть индикатор подачи (очередность неопределена).
        /// </summary>
        Hidden = 0,

        /// <summary>
        /// ћ€ч (подача) у команды хозаев.
        /// </summary>
        Home = 1,

        /// <summary>
        /// ћ€ч (подача) у команды гостей.
        /// </summary>
        Visitor = 2
    }
}