//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Pconsole.cs
// 
//-------------------------------------------------------------------

// ReSharper disable UnusedMember.Global

namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// Пульт управления полигоном.
    /// </summary>
    /// <remarks><c>pconsole.mib.xsd</c> - mib-файл</remarks>
    public enum Pconsole
    {
        /// <summary>
        /// Тип устройства. Не является переменной.
        /// </summary>
        DeviceTypeId = 5,

        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы в rs485 интерфейсе
        /// </summary>
        RS485ChecksumErrors = 0x100,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в rs485 интерфейсе
        /// </summary>
        RS485Collisions = 0x101,

        /// <summary>
        /// VT_UI4,R | Принято дейтаграмм по rs485 интерфейсу
        /// </summary>
        RS485RxDatagrams = 0x102,

        /// <summary>
        /// VT_UI4,R | Отослано дейтаграмм по rs485 интерфейсу
        /// </summary>
        RS485TxDatagrams = 0x103,

        /// <summary>
        /// VT_UI4,R | Ошибок синхронизации на rs485 интерфейсе
        /// </summary>
        RS485FramingErrors = 0x104,

        /// <summary>
        /// VT_UI4,R | Не отправленных дейтаграмм по rs485 интерфейсу
        /// </summary>
        RS485TxFailedDatagrams = 0x105,

        /// <summary>
        /// VT_UI4,R | Игнорировано start-битов на rs485 интерфейсе
        /// </summary>
        RS485FalseStartBits = 0x106,

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
        SarpResponces = 0x203,
    }
}