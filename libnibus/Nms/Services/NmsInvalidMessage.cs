using System;

namespace NataInfo.Nibus.Nms.Services
{
    internal sealed class NmsInvalidMessage : NmsMessage
    {
        public NibusDatagram Original { get; private set; }
        public Exception Exception { get; private set; }

        public NmsInvalidMessage(NibusDatagram original, Exception exception)
        {
            Original = original;
            Exception = exception;
            Initialize(original.Source, original.Destanation, PriorityType.Normal, NmsServiceType.Invalid, false, 0,
                       false, new byte[0]);
        }
    }
}
