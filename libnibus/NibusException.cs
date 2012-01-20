using System;

namespace NataInfo.Nibus
{
    /// <summary>
    /// Исключение при возникновении ошибки при приеме/передаче в NiBUS.
    /// </summary>
    public class NibusException : Exception
    {
        /// <summary>
        /// Возвращает код ошибки.
        /// </summary>
        /// <value>
        /// <c>0</c> - ошибки нет.
        /// </value>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NibusException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        public NibusException(int errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
