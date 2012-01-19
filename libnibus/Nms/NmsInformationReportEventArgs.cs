using System;

namespace NataInfo.Nibus.Nms
{
    public class NmsInformationReportEventArgs : EventArgs
    {
        public NmsInformationReportEventArgs(NmsInformationReport report)
        {
            InformationReport = report;
        }

        public NmsInformationReport InformationReport { get; private set; }
        public bool Identified { get; set; }
    }
}