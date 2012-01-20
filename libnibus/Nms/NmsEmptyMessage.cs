//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsEmptyMessage.cs
// 
//-------------------------------------------------------------------

namespace NataInfo.Nibus.Nms
{
    /// <summary>
    /// "Пустое" NMS-сообщение для сброса последнего сохраненного сообщения.
    /// </summary>
    /// <seealso cref="NmsProtocol.ResetIncoming"/>
    internal sealed class NmsEmptyMessage : NmsMessage
    {
        /// <summary>
        /// Экземпляр <see cref="NmsEmptyMessage"/>.
        /// </summary>
        /// <remarks>Не используйте для отправки.</remarks>
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
