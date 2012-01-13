using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    [ContractClass(typeof(INibusCodecContract<,>))]
    public interface INibusCodec<TEncoded, TDecoded> : IDisposable
    {
        IPropagatorBlock<TDecoded, TEncoded> Encoder { get; }
        IPropagatorBlock<TEncoded, TDecoded> Decoder { get; }

        IDisposable LinkTo<TNext>(INibusCodec<TDecoded, TNext> topCodec);
        IDisposable LinkTo<TNext>(INibusCodec<TDecoded, TNext> topCodec, Predicate<TDecoded> filter);
    }

    public abstract class NibusCodec<TEncoded, TDecoded> : INibusCodec<TEncoded, TDecoded>
    {
        private Unlinker _unlinker;

        #region INibusCodec<TEncoded,TDecoded> Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IPropagatorBlock<TDecoded, TEncoded> Encoder { get; protected set; }

        public IPropagatorBlock<TEncoded, TDecoded> Decoder { get; protected set; }

        public IDisposable LinkTo<TNext>(INibusCodec<TDecoded, TNext> topCodec)
        {
            if (_unlinker != null)
            {
                _unlinker.Dispose();
                _unlinker = null;
            }

            var decoderLink = Decoder.LinkTo(topCodec.Decoder);
            var encoderLink = topCodec.Encoder.LinkTo(Encoder);
            _unlinker = new Unlinker(decoderLink, encoderLink);
            
            return _unlinker;
        }

        public IDisposable LinkTo<TNext>(INibusCodec<TDecoded, TNext> topCodec, Predicate<TDecoded> filter)
        {
            if (_unlinker != null)
            {
                _unlinker.Dispose();
                _unlinker = null;
            }

            var decoderLink = Decoder.LinkTo(topCodec.Decoder, filter);
            var encoderLink = topCodec.Encoder.LinkTo(Encoder);
            _unlinker = new Unlinker(decoderLink, encoderLink);

            return _unlinker;
        }
        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_unlinker != null)
                {
                    _unlinker.Dispose();
                    _unlinker = null;
                }
            }
        }
    }

    // ReSharper disable InconsistentNaming
    [ContractClassFor(typeof(INibusCodec<,>))]
    public abstract class INibusCodecContract<TEncoded, TDecoded> : INibusCodec<TEncoded, TDecoded>
    {
        public void Dispose()
        {
        }

        public IPropagatorBlock<TDecoded, TEncoded> Encoder
        {
            get { return null; }
        }

        public IPropagatorBlock<TEncoded, TDecoded> Decoder
        {
            get { return null; }
        }

        public IDisposable LinkTo<TNext>(INibusCodec<TDecoded, TNext> topCodec)
        {
            Contract.Requires(topCodec != null);
            Contract.Requires(topCodec.Decoder != null);
            Contract.Requires(topCodec.Encoder != null);
            Contract.Requires(Decoder != null);
            Contract.Requires(Encoder != null);

            return null;
        }

        public IDisposable LinkTo<TNext>(INibusCodec<TDecoded, TNext> topCodec, Predicate<TDecoded> filter)
        {
            Contract.Requires(topCodec != null);
            Contract.Requires(topCodec.Decoder != null);
            Contract.Requires(topCodec.Encoder != null);
            Contract.Requires(Decoder != null);
            Contract.Requires(Encoder != null);
            Contract.Requires(filter != null);

            return null;
        }
    }
    // ReSharper restore InconsistentNaming
}