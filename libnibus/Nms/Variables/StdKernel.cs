//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// StdKernel.cs
// 
//-------------------------------------------------------------------

// ReSharper disable UnusedMember.Global
namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// model 0xxxxxxxxx | стандартный ISP загрузчик
    /// </summary>
    public enum StdKernel
    {
        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы в rs485 интерфейсе
        /// </summary>
        ChecksumErrors = 0x100,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в rs485 интерфейсе
        /// </summary>
        Collisions = 0x101,

        /// <summary>
        /// VT_UI4,R | Принято дейтаграмм по rs485 интерфейсу
        /// </summary>
        RxDatagrams = 0x102,

        /// <summary>
        /// VT_UI4,R | Отослано дейтаграмм по rs485 интерфейсу
        /// </summary>
        TxDatagrams = 0x103,

        /// <summary>
        /// VT_UI4,R | Ошибок синхронизации на rs485 интерфейсе
        /// </summary>
        FramingErrors = 0x104,

        /// <summary>
        /// VT_UI4,R | Не отправленных дейтаграмм по rs485 интерфейсу
        /// </summary>
        TxFailedDatagrams = 0x105,

        /// <summary>
        /// VT_UI4,R | Игнорировано start-битов на rs485 интерфейсе
        /// </summary>
        FalseStartBits = 0x106,

        /// <summary>
        /// VT_UI4,R | Обнаружено маркеров захвата шины
        /// </summary>
        NibusTakeoverMarkers = 0x107,

        /// <summary>
        /// VT_UI4,R | Обнаружено маркеров освобождения шины
        /// </summary>
        NibusFreeMarkers = 0x108,

        /// <summary>
        /// VT_UI4,R | Таймаутов освобождения шины
        /// </summary>
        NibusFreeTimeouts = 0x109,

        /// <summary>
        /// VT_UI4,R | Принято байт по rs485 интерфейсу
        /// </summary>
        BytesReceived = 0x10a,

        /// <summary>
        /// VT_UI4,R | Передано байт по rs485 интерфейсу
        /// </summary>
        BytesTransmitted = 0x10b,

        /// <summary>
        /// VT_UI4,R | Переполнений fifo на rs485 интерфейсе
        /// </summary>
        FifoOverruns = 0x10c,

        /// <summary>
        /// VT_UI1,R | Результат POST kernel'a
        /// </summary>
        PostResult = 0x11f,

        /// <summary>
        /// VT_UI1,R | Результат POST pcb-процесса
        /// </summary>
        UpostResult = 0x120,

        /// <summary>
        /// VT_UI1,R | Версия PCB
        /// </summary>
        PcbVersion = 0x122,

        /// <summary>
        /// VT_I1, R | ошибки при сохранении конфигурации
        /// </summary>
        PersistError = 0x123,

        /// <summary>
        /// VT_UI4,R | Повторений ответа на SARP-запрос
        /// </summary>
        SarpRetries = 0x200,

        /// <summary>
        /// VT_UI4,R | Ошибок при ответе на SARP-запрос
        /// </summary>
        SarpFailedResp = 0x201,

        /// <summary>
        /// VT_UI4,R | SARP-запросов
        /// </summary>
        SarpRequests = 0x202,

        /// <summary>
        /// VT_UI4,R | SARP-ответов
        /// </summary>
        SarpResponces = 0x203
    }
}