//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Nibus.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;
using NLog;

#endregion

namespace NataInfo.Nibus
{
    public static class Nibus
    {
        #region Member Variables

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Methods

        public static INibusEndpoint<NibusDatagram> BuildNibusStack(
            INibusEndpoint<byte[]> transport)
        {
            Contract.Requires(transport != null);
            Contract.Requires(transport.Decoder != null);
            Contract.Requires(transport.Encoder != null);
            Logger.Debug("Build Stack");
            var nibusDataCodec = new NibusDataCodec();
            transport.LinkTo(nibusDataCodec);

            var nibusBroadcaster = new BroadcastCodec<NibusDatagram>();
            nibusDataCodec.LinkTo(nibusBroadcaster);

            return nibusBroadcaster;
        }

        #endregion //Methods
    }
}