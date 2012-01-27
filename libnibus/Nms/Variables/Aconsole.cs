//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Aconsole.cs
// 
//-------------------------------------------------------------------

// ReSharper disable UnusedMember.Global

namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// model 0x0012xxxx | Консоль табло времени атаки
    /// </summary>
    public enum Aconsole
    {
        /// <summary>
        /// VT_BOOL,R/W | Направление таймера времени атаки
        /// </summary>
        TimerDirection = 0x100,

        /// <summary>
        /// VT_UI1,R/W | Интервал таймера времени атаки [1..99]
        /// </summary>
        TimerInterval = 0x101,

        /// <summary>
        /// VT_UI1,R/W | Длительность основного сигнала, с/10 [1..50]
        /// </summary>
        MainSignalDuration = 0x102,

        /// <summary>
        /// VT_UI1,R/W | Длительность ручного сигнала, с/10 [1..50]
        /// </summary>
        ManualSignalDuration = 0x103,

        /// <summary>
        /// VT_UI1,R/W | Яркость табло времени атаки [0..5]
        /// </summary>
        Brightness = 0x104,

        /// <summary>
        /// VT_BOOL,R/W | Функция трансивера
        /// </summary>
        Bridge = 0x105,

        /// <summary>
        /// VT_UI2,R | Переполнений очереди событий
        /// </summary>
        MsgQueueOverruns = 0x106,

        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы в rs485-интерфейсе
        /// </summary>
        RS485ChecksumErrors = 0x107,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в rs485-интерфейс
        /// </summary>
        RS485Collisions = 0x108,

        /// <summary>
        /// VT_UI4,R | Принято дейтаграмм из rs485-интерфейса
        /// </summary>
        RS485RxDatagrams = 0x109,

        /// <summary>
        /// VT_UI4,R | Отослано дейтаграмм в rs485-интерфейс
        /// </summary>
        RS485TxDatagrams = 0x10a,

        /// <summary>
        /// VT_UI4,R | Ошибок синхронизации на rs485-интерфейсе
        /// </summary>
        RS485FramingErrors = 0x10b,

        /// <summary>
        /// VT_UI4,R | Не отправленных дейтаграмм по rs485-интерфейсу
        /// </summary>
        RS485TxFailedDatagrams = 0x10c,

        /// <summary>
        /// VT_UI4,R | Получено байт по rs485-интерфейсу
        /// </summary>
        RS485BytesReceived = 0x10d,

        /// <summary>
        /// VT_UI4,R | Отправлено байт по rs485-интерфейсу
        /// </summary>
        RS485BytesTransmitted = 0x10e,

        /// <summary>
        /// VT_UI4,R | Отброшено дейтаграм из rs485-интерфейса
        /// </summary>
        RS485DatagramsIgnored = 0x10f,

        /// <summary>
        /// VT_UI4,R | Обнаружено маркеров захвата шины
        /// </summary>
        NibusTakeoverMarkers = 0x110,

        /// <summary>
        /// VT_UI4,R | Обнаружено маркеров освобождения шины
        /// </summary>
        NibusFreeMarkers = 0x111,

        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы в i2c-интерфейсе
        /// </summary>
        I2CChecksumErrors = 0x112,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в i2c-интерфейс
        /// </summary>
        I2CCollisions = 0x113,

        /// <summary>
        /// VT_UI4,R | Принято дейтаграмм из i2c-интерфейса
        /// </summary>
        I2CRxDatagrams = 0x114,

        /// <summary>
        /// VT_UI4,R | Отослано дейтаграмм в i2c-интерфейс
        /// </summary>
        I2CTxDatagrams = 0x115,

        /// <summary>
        /// VT_UI4,R | Не отправленных дейтаграмм по i2c-интерфейсу
        /// </summary>
        I2CTxFailedDatagrams = 0x116,

        /// <summary>
        /// VT_UI4,R | Получено байт по i2c-интерфейсу
        /// </summary>
        I2CBytesReceived = 0x117,

        /// <summary>
        /// VT_UI4,R | Отправлено байт по i2c-интерфейсу
        /// </summary>
        I2CBytesTransmitted = 0x118,

        /// <summary>
        /// VT_UI2,R/W | Значение таймера времени атаки [0..999]
        /// </summary>
        TimerValue = 0x119,

        /// <summary>
        /// VT_UI1,R/W | Доп. интервал таймера времени атаки [1..99]
        /// </summary>
        AuxTimerInterval = 0x11a,

        /// <summary>
        /// VT_BOOL,R/W | Функция остановки 24 секунд, при остановке игры
        /// </summary>
        Stop24 = 0x11b,
    }
}