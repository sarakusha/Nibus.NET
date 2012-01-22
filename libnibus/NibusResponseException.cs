using System;
using System.Runtime.Serialization;

namespace NataInfo.Nibus
{
    /// <summary>
    /// Исключение при получении кода ошибки в полученном подтверждении/ответе на запрос.
    /// </summary>
    [Serializable]
    public class NibusResponseException : Exception
    {
        /// <summary>
        /// Возвращает код ошибки.
        /// </summary>
        /// <value>
        /// <c>0</c> - ошибки нет.
        /// </value>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NibusResponseException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        public NibusResponseException(int errorCode)
        {
            ErrorCode = errorCode;
        }

        public NibusResponseException()
        {
        }

        public NibusResponseException(string message) : base(message)
        {
        }

        public NibusResponseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NibusResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ErrorCode = info.GetInt32("NibusErrorCode");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("NibusErrorCode", ErrorCode);
        }
    }
}
