namespace NataInfo.Nibus.Sport
{
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
}