namespace NataInfo.Nibus
{
    /// <summary>
    /// Приоритет сообщения. Применяется при коллизиях.
    /// </summary>
    public enum PriorityType
    {
        /// <summary>
        /// Наивысший приоритет.
        /// </summary>
        Realtime = 0,

        /// <summary>
        /// Приоритет выше нормального.
        /// </summary>
        AboveNormal = 1,

        /// <summary>
        /// Нормальный приоритет.
        /// </summary>
        Normal = 2,

        /// <summary>
        /// Низший приоритет.
        /// </summary>
        BelowNormal = 3
    }
}