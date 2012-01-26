using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    /// <summary>
    /// Информация о кодеке.
    /// </summary>
    public interface ICodecInfo
    {
        /// <summary>
        /// Возвращает описание кодека.
        /// </summary>
        string Description { get; }
    }

    /// <summary>
    /// Интерфейс для NiBUS-кодеков.
    /// </summary>
    /// <typeparam name="TEncoded">Тип сообщений нижележащего уровня.</typeparam>
    /// <typeparam name="TDecoded">Тип преобразованных сообщений.</typeparam>
    public interface INibusCodec<TEncoded, TDecoded> : ICodecInfo, IDisposable
    {
        /// <summary>
        /// Возвращает кодировщик в нижележащий уровень.
        /// </summary>
        IPropagatorBlock<TDecoded, TEncoded> Encoder { get; }

        /// <summary>
        /// Возвращает декодер с нижележащего уровня.
        /// </summary>
        IPropagatorBlock<TEncoded, TDecoded> Decoder { get; }
    }

    /// <summary>
    /// Абстрактный базовый класс для NiBUS-кодеков.
    /// </summary>
    /// <typeparam name="TEncoded">Тип сообщений нижележащего уровня.</typeparam>
    /// <typeparam name="TDecoded">Тип преобразованных сообщений.</typeparam>
    public abstract class NibusCodec<TEncoded, TDecoded> : INibusCodec<TEncoded, TDecoded>
    {
        private Unlinker _unlinker = new Unlinker();

        #region INibusCodec<TEncoded,TDecoded> Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed
        {
            get { return _unlinker != null; }
        }
        /// <summary>
        /// Возвращает кодировщик в нижележащий уровень.
        /// </summary>
        public IPropagatorBlock<TDecoded, TEncoded> Encoder { get; protected set; }

        /// <summary>
        /// Возвращает декодер с нижележащего уровня.
        /// </summary>
        public IPropagatorBlock<TEncoded, TDecoded> Decoder { get; protected set; }

        /// <summary>
        /// Подключает к кодеку нижележащего уровня.
        /// </summary>
        /// <param name="bottomCodec">Низлежащий кодек в стеке протоколов.</param>
        /// <returns>
        /// Объект, вызвав у которого <see cref="IDisposable.Dispose"/>, можно прервать связь.
        /// </returns>
        public virtual IDisposable ConnectTo<T>(INibusCodec<T, TEncoded> bottomCodec)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(bottomCodec != null);
            Contract.Requires(bottomCodec.Decoder != null);
            Contract.Requires(bottomCodec.Encoder != null);
            Contract.Requires(Decoder != null);
            Contract.Requires(Encoder != null);
            return LinkTo(bottomCodec);
        }

        /// <summary>
        /// Подключает к транспорту.
        /// </summary>
        /// <param name="transport">Транспорт.</param>
        /// <returns>
        /// Объект, вызвав у которого <see cref="IDisposable.Dispose"/>, можно прервать связь.
        /// </returns>
        public virtual IDisposable ConnectTo(INibusEndpoint<TEncoded> transport)
        {
            Contract.Requires(!IsDisposed);
            Contract.Requires(transport != null);
            Contract.Requires(transport.IncomingMessages != null);
            Contract.Requires(transport.OutgoingMessages != null);
            Contract.Requires(Decoder != null);
            Contract.Requires(Encoder != null);

            return LinkTo(transport);
        }

        protected IDisposable LinkTo<T>(INibusCodec<T, TEncoded> bottomCodec)
        {
            // Удаляем старые сообщения
            var receivable = bottomCodec.Decoder as IReceivableSourceBlock<TEncoded>;
            if (receivable != null)
            {
                IList<TEncoded> oldMessages;
                receivable.TryReceiveAll(out oldMessages);
            }

            var unlinker = new Unlinker();
            unlinker.AddLink(bottomCodec.Decoder.LinkTo(Decoder));
            unlinker.AddLink(Encoder.LinkTo(bottomCodec.Encoder));
            
            _unlinker.AddLink(unlinker);
            return unlinker;
        }

        protected IDisposable LinkTo(INibusEndpoint<TEncoded> transport)
        {
            // Удаляем старые сообщения
            IList<TEncoded> oldMessages;
            transport.IncomingMessages.TryReceiveAll(out oldMessages);

            var unlinker = new Unlinker();
            unlinker.AddLink(transport.IncomingMessages.LinkTo(Decoder));
            unlinker.AddLink(Encoder.LinkTo(transport.OutgoingMessages));
            
            _unlinker.AddLink(unlinker);
            return unlinker;
        }

        protected IDisposable LinkTo<T>(INibusCodec<T, TEncoded> bottomCodec, Predicate<TEncoded> filter,
                                        bool discardsMessages = false)
        {
            // Удаляем старые сообщения
            var receivable = bottomCodec.Decoder as IReceivableSourceBlock<TEncoded>;
            if (receivable != null)
            {
                if (discardsMessages)
                {
                    IList<TEncoded> oldMessages;
                    receivable.TryReceiveAll(out oldMessages);
                }
                else
                {
                    TEncoded oldMessage;
                    while (receivable.TryReceive(filter, out oldMessage))
                    {
                    }
                }
            }

            var unlinker = new Unlinker();
            unlinker.AddLink(bottomCodec.Decoder.LinkTo(Decoder, filter, discardsMessages));
            unlinker.AddLink(Encoder.LinkTo(bottomCodec.Encoder));
            _unlinker.AddLink(unlinker);

            return unlinker;
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (_unlinker)
                {
                    _unlinker = null;
                }
            }
        }

        #region Implementation of ICodecInfo

        /// <summary>
        /// Возвращает описание кодека.
        /// </summary>
        public string Description { get; protected set; }

        #endregion
    }
}