namespace NataInfo.Nibus
{
    /// <summary>
    /// Кодек для указанного типа протокола.
    /// </summary>
    /// <typeparam name="TEncoded">Тип данных нижележащего уровня.</typeparam>
    /// <typeparam name="TDecoded">Тип преобразованных сообщений.</typeparam>
    public interface INibusProtocol<TEncoded, TDecoded> : INibusCodec<TEncoded, TDecoded> 
    {
        /// <summary>
        /// Возвращает тип протокола, для которого используется кодек.
        /// </summary>
        /// <seealso cref="NibusDatagram.ProtocolType"/>
        ProtocolType ProtocolType { get; }

        /// <summary>
        /// Возвращает front-end класс для работы с протоколом.
        /// </summary>
        INibusEndpoint<TDecoded> Protocol { get; }
    }
}