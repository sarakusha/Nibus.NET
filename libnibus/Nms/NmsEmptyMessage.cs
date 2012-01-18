//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsEmptyMessage.cs
// 
//-------------------------------------------------------------------

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsEmptyMessage : NmsMessage
    {
        public static readonly NmsEmptyMessage Instance = new NmsEmptyMessage();

        private NmsEmptyMessage()
        {
            Initialize(
                Address.Empty,
                Address.Empty,
                PriorityType.BelowNormal,
                NmsServiceType.None,
                false,
                0,
                false,
                new byte[0]);
        }
    }
}
