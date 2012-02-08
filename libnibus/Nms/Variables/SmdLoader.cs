//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// SmdLoader.cs
// 
//-------------------------------------------------------------------

// ReSharper disable UnusedMember.Global

namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// model 0x0014xxxx | загрузчик SMD-модуля
    /// </summary>
    public enum SmdLoader
    {
        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы в rsRS485-h интерфейсе
        /// </summary>
        HChecksumErrors = 0x100,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в rsRS485-h интерфейсе
        /// </summary>
        HCollisions = 0x101,

        /// <summary>
        /// VT_UI4,R | Принято дейтаграмм по rsRS485-h интерфейсу
        /// </summary>
        HRxDatagrams = 0x102,

        /// <summary>
        /// VT_UI4,R | Отослано дейтаграмм по rsRS485-h интерфейсу
        /// </summary>
        HTxDatagrams = 0x103,

        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы в rsRS485-v интерфейсе
        /// </summary>
        VChecksumErrors = 0x104,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в rsRS485-v интерфейсе
        /// </summary>
        VCollisions = 0x105,

        /// <summary>
        /// VT_UI4,R | Принято дейтаграмм по rsRS485-v интерфейсу
        /// </summary>
        VRxDatagrams = 0x106,

        /// <summary>
        /// VT_UI4,R | Отослано дейтаграмм по rsRS485-h интерфейсу
        /// </summary>
        VTxDatagrams = 0x107,

        /// <summary>
        /// VT_UI4,R | Принято дейтаграмм от ведомого процессора
        /// </summary>
        SlaveRxDatagrams = 0x108,

        /// <summary>
        /// VT_UI4,R | Отослано дейтаграмм ведомому процессору
        /// </summary>
        SlaveTxDatagrams = 0x109,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм ведомому процессору
        /// </summary>
        SlaveCollisions = 0x10a,

        /// <summary>
        /// VT_UI4,R | Ошибок синхронизации на rsRS485-h интерфейсе
        /// </summary>
        HFramingErrors = 0x10b,

        /// <summary>
        /// VT_UI4,R | Ошибок синхронизации на rsRS485-v интерфейсе
        /// </summary>
        VFramingErrors = 0x10c,

        /// <summary>
        /// VT_UI4,R | Не отправленных дейтаграмм по rsRS485-h интерфейсу
        /// </summary>
        HTxFailedDatagrams = 0x10d,

        /// <summary>
        /// VT_UI4,R | Не отправленных дейтаграмм по rsRS485-v интерфейсу
        /// </summary>
        VTxFailedDatagrams = 0x10e,

        /// <summary>
        /// VT_UI4,R | Фальшивых start-битов на rsRS485-v интерфейсе
        /// </summary>
        VFalseStartBits = 0x10f,

        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы в i2c интерфейсе
        /// </summary>
        SlaveChecksumErrors = 0x110,

        /// <summary>
        /// VT_UI4,R | Получено байт от ведомого процессора
        /// </summary>
        SlaveBytesReceived = 0x111,

        /// <summary>
        /// VT_UI2,R | Переполнений фифо на rsRS485h-интерфейсе
        /// </summary>
        HFifoOverruns = 0x112,

        /// <summary>
        /// VT_UI2,R | Переполнений фифо на rsRS485v-интерфейсе
        /// </summary>
        VFifoOverruns = 0x113,

        /// <summary>
        /// VT_UI2,R | Переполнений фифо на i2c-интерфейсе
        /// </summary>
        SlaveFifoOverruns = 0x114,

        /// <summary>
        /// VT_UI4,R | Повторений ответа на SARP-запрос
        /// </summary>
        SarpRetries = 0x115,

        /// <summary>
        /// VT_UI4,R | Ошибок при ответе на SARP-запрос
        /// </summary>
        SarpFailedResp = 0x116,

        /// <summary>
        /// VT_UI4,R | SARP-запросов
        /// </summary>
        SarpRequests = 0x117,

        /// <summary>
        /// VT_UI4,R | SARP-ответов
        /// </summary>
        SarpResponces = 0x118,
    }
}