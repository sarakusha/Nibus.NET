namespace NataInfo.Nibus
{
    public interface INibusProtocol<TEncoded, TDecoded> : INibusCodec<TEncoded, TDecoded>
    {
        ProtocolType Protocol { get; }
    }
}
