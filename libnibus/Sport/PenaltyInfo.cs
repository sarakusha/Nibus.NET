using System;
using System.Collections.Generic;
using System.Linq;
using NataInfo.Nibus.Nms.Services;

namespace NataInfo.Nibus.Sport
{
    public class PenaltyInfo
    {
        private const int AttrOfs = 0;
        private const int NumberOfs = 1;
        private const int MinutesOfs = 2;
        private const int SecondsOfs = 3;
        private const int ReservedOfs = 4;
        internal const int ItemLength = 5;

        [Flags]
        private enum Attributes : byte
        {
            None = 0,
            Valid = 1,
            Active = 2
        }

        public int PlayerNo { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public bool IsValid { get; set; }
        public bool IsActive { get; set; }

        public PenaltyInfo(int playerNo, int minutes, int seconds, bool isValid = true, bool isActive = true)
        {
            PlayerNo = playerNo;
            Minutes = minutes;
            Seconds = seconds;
            IsValid = isValid;
            IsActive = isActive;
        }

        internal PenaltyInfo(IEnumerable<byte> data)
        {
            var adata = data.ToArray();
            IsValid  = (adata[AttrOfs] & (byte)Attributes.Valid) != 0;
            IsActive = (adata[AttrOfs] & (byte)Attributes.Active) != 0;
            PlayerNo = NmsMessage.UnpackByte(adata[NumberOfs]);
            Minutes = NmsMessage.UnpackByte(adata[MinutesOfs]);
            Seconds = NmsMessage.UnpackByte(adata[SecondsOfs]);
        }

        public override string ToString()
        {
            return String.Format("{0:D2} {1:D2}:{2:D2}", PlayerNo, Minutes, Seconds);
        }

        internal byte[] GetData()
        {
            var data = new byte[ItemLength];
            data[AttrOfs] =
                (byte)((IsActive ? Attributes.Active : Attributes.None) | (IsValid ? Attributes.Valid : Attributes.None));
            data[NumberOfs] = NmsMessage.PackByte(PlayerNo);
            data[MinutesOfs] = NmsMessage.PackByte(Minutes);
            data[SecondsOfs] = NmsMessage.PackByte(Seconds);
            return data;
        }
    }
}