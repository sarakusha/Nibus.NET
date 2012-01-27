//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Siolynx.cs
// 
//-------------------------------------------------------------------

// ReSharper disable UnusedMember.Global
namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// model 0x0007xxxx | Транскодер последовательных интерфейсов (SioLynx)
    /// </summary>
    public enum Siolynx
    {
        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в rs485-h интерфейсе
        /// </summary>
        RS485Collisions = 0x100,

        /// <summary>
        /// VT_UI4,R | Принято байт по rs485 интерфейсу
        /// </summary>
        RS485BytesReceived = 0x101,

        /// <summary>
        /// VT_UI4,R | Отослано дейтаграмм по rs485-интерфейсу
        /// </summary>
        RS485TxDatagrams = 0x102,

        /// <summary>
        /// VT_UI4,R | Обнаружено маркеров захвата шины
        /// </summary>
        NibusTakeoverMarkers = 0x103,

        /// <summary>
        /// VT_UI4,R | Обнаружено маркеров освобождения шины
        /// </summary>
        NibusFreeMarkers = 0x104,

        /// <summary>
        /// VT_UI4,R | Таймаутов освобождения шины
        /// </summary>
        NibusFreeTimeouts = 0x105,

        /// <summary>
        /// VT_UI4,R | Ошибок синхронизации на rs485 интерфейсе
        /// </summary>
        RS485FramingErrors = 0x106,

        /// <summary>
        /// VT_UI4,R | Ошибок синхронизации на rs232 интерфейсе
        /// </summary>
        RS232FramingErrors = 0x107,

        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы в rs232 интерфейсе
        /// </summary>
        RS232ChecksumErrors = 0x108,

        /// <summary>
        /// VT_UI4,R | Принято дейтаграмм по rs232 интерфейсу
        /// </summary>
        RS232RxDatagrams = 0x109,

        /// <summary>
        /// VT_UI4,R | Не отправленных дейтаграмм по rs485-интерфейсу
        /// </summary>
        RS485TxFailedDatagrams = 0x10a,

        /// <summary>
        /// VT_UI4,R | Фальшивых start-битов на rs485-интерфейсе
        /// </summary>
        RS485FalseStartBits = 0x10b,

        /// <summary>
        /// VT_UI4,R | Переполнений фифо на rs232-интерфейсе
        /// </summary>
        RS232FifoOverruns = 0x10c,

        /// <summary>
        /// VT_UI2,R | Байт в журнале событий
        /// </summary>
        LogSize = 0x10d,

        /// <summary>
        /// VT_BOOL | выходной контакт
        /// </summary>
        MisoPin = 0x10e,

        /// <summary>
        /// VT_UI1 | 
        /// </summary>
        ConvData = 0x10f,
    }
}