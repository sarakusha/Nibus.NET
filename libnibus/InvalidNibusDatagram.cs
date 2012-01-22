using System;

namespace NataInfo.Nibus
{
    /// <summary>
    /// Исключение при нарушении структуры датагарммы NiBUS или производных сообщений.
    /// </summary>
    [Serializable]
    public class InvalidNibusDatagram : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNibusDatagram"/> class.
        /// </summary>
        public InvalidNibusDatagram()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNibusDatagram"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidNibusDatagram(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNibusDatagram"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public InvalidNibusDatagram(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
