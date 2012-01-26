namespace NataInfo.Nibus.Nms
{
    /// <summary>
    /// Идентификаторы типа сервиса NMS.
    /// </summary>
    public enum NmsServiceType
    {
        /// <summary>
        /// Неверно сформированное NMS-сообщение.
        /// Только для внутреннего использования.
        /// </summary>
        Invalid = -1,
        /// <summary>
        /// Используется в пакетных сообщениях <see cref="NmsReadMany"/>
        /// </summary>
        None = 0,
        /// <summary>
        /// Прочитать значение переменной.
        /// </summary>
        Read = 1,
        /// <summary>
        /// Изменить значение переменной.
        /// </summary>
        Write = 2,
        /// <summary>
        /// Распространение значения переменной.
        /// </summary>
        InformationReport = 3,
        /// <summary>
        /// Сигнализация событий.
        /// </summary>
        EventNotification = 4,
        /// <summary>
        /// Подтверждение события.
        /// </summary>
        AckEventNotification = 5,
        /// <summary>
        /// Запрет/разрешение сигнализации события.
        /// </summary>
        AlterEventConditionMonitoring = 6,
        /// <summary>
        /// Запрос на считывание домена.
        /// </summary>
        RequestDomainUpload = 7,
        /// <summary>
        /// Начало считывания.
        /// </summary>
        InitiateUploadSequence = 8,
        /// <summary>
        /// Считывание данных из устройства.
        /// </summary>
        UploadSegment = 9,
        /// <summary>
        /// Запрос на загрузку домена.
        /// </summary>
        RequestDomainDownload = 10,
        /// <summary>
        /// Начало загрузки.
        /// </summary>
        InitiateDownloadSequence = 11,
        /// <summary>
        /// Передача данных в устройство.
        /// </summary>
        DownloadSegment = 12,
        TerminateDownloadSequence = 13,
        /// <summary>
        /// Проверка контрольной суммы домена.
        /// </summary>
        VerifyDomainChecksum = 14,
        /// <summary>
        /// Выполнить программу.
        /// </summary>
        ExecuteProgramInvocation = 15,
        /// <summary>
        /// Начать выполнение программы.
        /// </summary>
        StartProgramInvocation = 16,
        /// <summary>
        /// Остановить программу.
        /// </summary>
        Stop = 17,
        /// <summary>
        /// Продолжить выполнение.
        /// </summary>
        Resume = 18,
        /// <summary>
        /// Реинициализация программы.
        /// </summary>
        Reset = 19,
        /// <summary>
        /// Выключение устройства.
        /// </summary>
        Shutdown = 20
    }
}