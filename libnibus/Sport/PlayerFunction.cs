namespace NataInfo.Nibus.Sport
{
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
}