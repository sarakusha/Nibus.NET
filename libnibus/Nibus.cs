//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Nibus.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;
using NataInfo.Nibus.Nms;

#endregion

namespace NataInfo.Nibus
{
    public static class Nibus
    {
        #region Member Variables
        
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        #endregion

        #region Methods

        public static INibusEndpoint<NibusDatagram, NibusDatagram> BuildNibusStack(INibusEndpoint<byte[], byte[]> bottom)
        {
            Logger.Debug("Build Stack");
            var nibusDataCodec = new NibusDataCodec();
            bottom.Decoder.LinkTo(nibusDataCodec.Decoder);
            nibusDataCodec.Encoder.LinkTo(bottom.Encoder);

            var broadcaster = new NibusBroadcastCodec();
            nibusDataCodec.Decoder.LinkTo(broadcaster.Decoder);
            broadcaster.Encoder.LinkTo(nibusDataCodec.Encoder);

            return broadcaster;
        }

        #endregion //Methods
    }
}
