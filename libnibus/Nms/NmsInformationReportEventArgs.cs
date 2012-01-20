using System;

namespace NataInfo.Nibus.Nms
{
    /// <summary>
    /// Класс содержащий данные о событии получения информационного сообщения.
    /// </summary>
    public class NmsInformationReportEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NmsInformationReportEventArgs"/> class.
        /// </summary>
        /// <param name="report">Информационное сообщение.</param>
        public NmsInformationReportEventArgs(NmsInformationReport report)
        {
            InformationReport = report;
        }

        /// <summary>
        /// Возвращает информационное сообщение.
        /// </summary>
        public NmsInformationReport InformationReport { get; private set; }

        /// <summary>
        /// Индикатор, что сообщение было идентифицировано и в случае подписки на него - обработано.
        /// </summary>
        public bool Identified { get; set; }
    }
}