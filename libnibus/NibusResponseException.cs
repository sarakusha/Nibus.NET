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

        /// <summary>
        /// Initializes a new instance of the <see cref="NibusResponseException"/> class.
        /// </summary>
        public NibusResponseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NibusResponseException"/> class.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public NibusResponseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NibusResponseException"/> class.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="inner">Вложенное исключение.</param>
        public NibusResponseException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NibusResponseException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        ///   
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected NibusResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ErrorCode = info.GetInt32("NibusErrorCode");
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception>
        ///   
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        ///   </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("NibusErrorCode", ErrorCode);
        }
    }
}
