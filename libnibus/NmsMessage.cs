using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NataInfo.Nibus
{
    internal enum NmsServiceType
    {
        None = 0,
		Read = 1,
		Write = 2,
		InformationReport = 3,
		EventNotification = 4,
		AckEventNotification = 5,
		AlterEventConditionMonitoring = 6,
		RequestDomainUpload = 7,
		InitiateUploadSequence = 8,
		UploadSegment = 9,
		RequestDomainDownload = 10,
		InitiateDownloadSequence = 11,
		DownloadSegment = 12,
		TerminateDownloadSequence = 13,
		VerifyDomainChecksum = 14,
		ExecuteProgramInvocation = 15,
		StartProgramInvocation = 16,
		Stop = 17,
		Resume = 18,
		Reset = 19,
		Shutdown = 20
	}

    internal class NmsMessage : INibusDatagram
    {
        public NmsMessage(
            Address destanation,
            Address source,
            NmsServiceType serviceType,
            bool isResponse,
            int id,
            bool isResponsible,
            IList<byte> data,
            PriorityType priority = PriorityType.Normal)
        {
            Destanation = destanation;
            Source = source;
            Priority = priority;

            ServiceType = serviceType;
            IsResponce = isResponse;
            Id = id;
            IsResponsible = isResponsible;
            Data = new ReadOnlyCollection<byte>(data);
        }

        public NmsMessage(NibusDatagram datagram)
        {
            if (datagram.Data.Count < 3)
            {
                throw new ArgumentException("Invalid data");
            }

            var cbNmsSize = datagram.Data[2] & 0x3f;
            if (datagram.Data.Count < cbNmsSize + 3)
            {
                throw new ArgumentException("Invalid data");
            }

            ServiceType = (NmsServiceType) (datagram.Data[0] >> 3);
            IsResponce = (datagram.Data[0] & 4) != 0;
            Id = ((datagram.Data[0] & 3) << 8) | datagram.Data[1];
            IsResponsible = (datagram.Data[2] & 0x80) == 0;
            Destanation = datagram.Destanation;
            Source = datagram.Source;
            Priority = datagram.Priority;
        }

        public Address Destanation { get; private set; }
        public Address Source { get; private set; }
        public PriorityType Priority { get; private set; }

        public ProtocolType Protocol
        {
            get { return ProtocolType.Nms; }
        }

        public ReadOnlyCollection<byte> Data { get; private set; }
        public NmsServiceType ServiceType { get; private set; }
        public bool IsResponce { get; private set; }
        public int Id { get; private set; }
        public bool IsResponsible { get; private set; }
    }
}
