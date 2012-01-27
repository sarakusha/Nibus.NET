using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks.Dataflow;
using NataInfo.Nibus.Nms.Services;

namespace NataInfo.Nibus.Nms
{
    /// <summary>
    /// Кодек NMS-протокола.
    /// </summary>
    public class NmsCodec : NibusCodec<NibusDatagram, NmsMessage>, INibusProtocol<NibusDatagram, NmsMessage>
    {
        private readonly BroadcastTransformBlock<NibusDatagram, NmsMessage> _decoder;
        private NmsProtocol _protocol;
        private static decimal _nmsErrors;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NmsCodec"/> class.
        /// </summary>
        public NmsCodec()
        {
            Contract.Ensures(Decoder != null);
            Contract.Ensures(Encoder != null);
            _decoder = new BroadcastTransformBlock<NibusDatagram, NmsMessage>(SafeDecode);
            Decoder = _decoder;
            Encoder = new TransformBlock<NmsMessage, NibusDatagram>(m => m.Datagram);
        }

        /// <summary>
        /// Возвращает общее количество NMS-пакетов с ошибками.
        /// </summary>
        public static decimal NmsErrors
        {
            get { return _nmsErrors; }
        }

        #region Implementation of INibusProtocol

        /// <summary>
        /// Возвращает тип протокола, для которого используется кодек.
        /// </summary>
        /// <returns><see cref="NataInfo.Nibus.ProtocolType.Nms"/></returns>
        public ProtocolType ProtocolType
        {
            get { return ProtocolType.Nms; }
        }

        #endregion

        INibusEndpoint<NmsMessage> INibusProtocol<NibusDatagram, NmsMessage>.Protocol
        {
            get { return Protocol; }
        }

        /// <summary>
        /// Возвращает front-end класс для работы с протоколом NMS.
        /// </summary>
        public NmsProtocol Protocol
        {
            get { return _protocol ?? (_protocol = new NmsProtocol(_decoder.Source, Encoder)); }
        }

        /// <summary>
        /// Подключает к кодеку нижележащего уровня.
        /// </summary>
        /// <param name="bottomCodec">Нижележащий кодек в стеке протоколов.</param>
        /// <returns>Объект, вызвав у которого <see cref="IDisposable.Dispose"/>, можно прервать связь.</returns>
        public override IDisposable ConnectTo<T>(INibusCodec<T, NibusDatagram> bottomCodec)
        {
            return LinkTo(bottomCodec, datagram => datagram.ProtocolType == ProtocolType);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_protocol != null)
            {
                _protocol.Dispose();
                _protocol = null;
            }
        }

        /// <summary>
        /// Безопасное декодирование нижележащего протокола.
        /// </summary>
        /// <param name="datagram">Датаграмма.</param>
        /// <returns>
        /// Декодированное NMS-сообщение, потомок от <see cref="NmsMessage"/>,
        /// если ошибка, то <see cref="NmsInvalidMessage"/>
        /// </returns>
        /// <remarks>
        /// Функция декодирования !должна! проверить на валидность датаграмму
        /// и в случае ошибки сгенерировать исключение <see cref="InvalidNibusDatagram"/>
        /// </remarks>
        private static NmsMessage SafeDecode(NibusDatagram datagram)
        {
            try
            {
                return NmsMessage.CreateFrom(datagram);
            }
            catch (Exception e)
            {
                _nmsErrors++;
                return new NmsInvalidMessage(datagram, e);
            }
        }
    }
}