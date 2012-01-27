//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Mcdvi.cs
// 
//-------------------------------------------------------------------

// ReSharper disable UnusedMember.Global

namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// model 0x001bxxxx | плата управления модулем индикации MC-DVI
    /// </summary>
    public enum Mcdvi
    {
        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы в rs485-h интерфейсе
        /// </summary>
        RS485HChecksumErrors = 0x100,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в rs485-h интерфейсе
        /// </summary>
        RS485HCollisions = 0x101,

        /// <summary>
        /// VT_UI4,R | Принято дейтаграмм по rs485-h интерфейсу
        /// </summary>
        RS485HRxDatagrams = 0x102,

        /// <summary>
        /// VT_UI4,R | Отослано дейтаграмм по rs485-h интерфейсу
        /// </summary>
        RS485HTxDatagrams = 0x103,

        /// <summary>
        /// VT_UI4,R | Ошибок синхронизации на rs485-h интерфейсе
        /// </summary>
        RS485HFramingErrors = 0x104,

        /// <summary>
        /// VT_UI4,R | Не отправленных дейтаграмм по rs485-h интерфейсу
        /// </summary>
        RS485HTxFailedDatagrams = 0x105,

        /// <summary>
        /// VT_UI4,R | Фальшивых start-битов на rs485-h интерфейсе
        /// </summary>
        RS485HFalseStartBits = 0x106,

        /// <summary>
        /// VT_UI2,R | Переполнений fifo на rs485-h интерфейсе
        /// </summary>
        RS485HFifoOverruns = 0x107,

        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы в rs485-v интерфейсе
        /// </summary>
        RS485VChecksumErrors = 0x10a,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в rs485-v интерфейсе
        /// </summary>
        RS485VCollisions = 0x10b,

        /// <summary>
        /// VT_UI4,R | Принято дейтаграмм по rs485-v интерфейсу
        /// </summary>
        RS485VRxDatagrams = 0x10c,

        /// <summary>
        /// VT_UI4,R | Отослано дейтаграмм по rs485-h интерфейсу
        /// </summary>
        RS485VTxDatagrams = 0x10d,

        /// <summary>
        /// VT_UI4,R | Ошибок синхронизации на rs485-v интерфейсе
        /// </summary>
        RS485VFramingErrors = 0x10e,

        /// <summary>
        /// VT_UI4,R | Не отправленных дейтаграмм по rs485-v интерфейсу
        /// </summary>
        RS485VTxFailedDatagrams = 0x10f,

        /// <summary>
        /// VT_UI4,R | Потеряно байт в rs485-v интерфейсе
        /// </summary>
        RS485VBytesLost = 0x110,

        /// <summary>
        /// VT_UI2,R | Переполнений fifo на rs485-v интерфейсе
        /// </summary>
        RS485VFifoOverruns = 0x111,

        /// <summary>
        /// VT_UI4,R | Обнаружено маркеров захвата шины rs485-h интерфейсу
        /// </summary>
        RS485HTakeoverMarkers = 0x112,

        /// <summary>
        /// VT_UI4,R | Обнаружено маркеров освобождения шины rs485-h интерфейсу
        /// </summary>
        RS485HFreeMarkers = 0x113,

        /// <summary>
        /// VT_UI4,R | Таймаутов освобождения шины по rs485-h интерфейсу
        /// </summary>
        RS485HFreeTimeouts = 0x114,

        /// <summary>
        /// VT_UI4,R | Принято байт по rs485-h интерфейсу
        /// </summary>
        RS485HBytesReceived = 0x115,

        /// <summary>
        /// VT_UI4,R | Отослано байт по rs485-h интерфейсу
        /// </summary>
        RS485HBytesTransmitted = 0x116,

        /// <summary>
        /// VT_UI4,R | Обнаружено маркеров захвата шины rs485-v интерфейсу
        /// </summary>
        RS485VTakeoverMarkers = 0x117,

        /// <summary>
        /// VT_UI4,R | Обнаружено маркеров освобождения шины rs485-v интерфейсу
        /// </summary>
        RS485VFreeMarkers = 0x118,

        /// <summary>
        /// VT_UI4,R | Таймаутов освобождения шины по rs485-v rs485-v интерфейсу
        /// </summary>
        RS485VFreeTimeouts = 0x119,

        /// <summary>
        /// VT_UI4,R | Принято байт по rs485-v интерфейсу
        /// </summary>
        RS485VBytesReceived = 0x11a,

        /// <summary>
        /// VT_UI4,R | Отослано байт по rs485-v интерфейсу
        /// </summary>
        RS485VBytesTransmitted = 0x11b,

        /// <summary>
        /// VT_UI1,R | Результат POST pcb-процесса
        /// </summary>
        UpostResult = 0x120,

        /// <summary>
        /// VT_UI1,R | Версия PCB
        /// </summary>
        PcbVersion = 0x122,

        /// <summary>
        /// VT_UI1,R | версия PLD1
        /// </summary>
        Pld1Version = 0x123,

        /// <summary>
        /// VT_UI1,R/W | подсеть
        /// </summary>
        Subnet = 0x124,

        /// <summary>
        /// VT_I1, R | ошибки при сохранении конфигурации
        /// </summary>
        PersistError = 0x125,

        /// <summary>
        /// VT_UI4, R | перезагрузок PLD
        /// </summary>
        PldReboots = 0x126,

        /// <summary>
        /// VT_UI1,R | версия PLD2
        /// </summary>
        Pld2Version = 0x127,

        /// <summary>
        /// VT_UI1,R | количество тестов реализованных в PLD
        /// </summary>
        PldTests = 0x128,

        /// <summary>
        /// VT_UI1,R | ревизия PLD1
        /// </summary>
        Pld1Revision = 0x129,

        /// <summary>
        /// VT_UI1,R | ревизия PLD1
        /// </summary>
        Pld2Revision = 0x12a,

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

        /// <summary>
        /// VT_UI1, R | количество PLD в MC
        /// </summary>
        PldQty = 0x280,

        /// <summary>
        /// VT_UI1, R | горизонтальное разрешение модуля
        /// </summary>
        ModuleHres = 0x281,

        /// <summary>
        /// VT_UI1, R | вертикальное разрешение модуля
        /// </summary>
        ModuleVres = 0x282,

        /// <summary>
        /// VT_UI1, R | горизонтальное разрешение платы индикации
        /// </summary>
        IpcbHres = 0x283,

        /// <summary>
        /// VT_UI1, R | вертикальное разрешение платы индикации
        /// </summary>
        IpcbVres = 0x284,

        /// <summary>
        /// VT_UI1, R | возможности PLD1
        /// </summary>
        Pld1Caps = 0x285,

        /// <summary>
        /// VT_UI1, R | возможности PLD2
        /// </summary>
        Pld2Caps = 0x286,

        /// <summary>
        /// VT_UI1, R | количество плат индикации
        /// </summary>
        IpcbQty = 0x287,

        /// <summary>
        /// VT_UI4, R/W | серийный номер платы индикации 0
        /// </summary>
        Ipcb0Serial = 0x290,

        /// <summary>
        /// VT_UI4, R/W | серийный номер платы индикации 1
        /// </summary>
        Ipcb1Serial = 0x291,

        /// <summary>
        /// VT_UI4, R/W | серийный номер платы индикации 2
        /// </summary>
        Ipcb2Serial = 0x292,

        /// <summary>
        /// VT_UI4, R/W | серийный номер платы индикации 3
        /// </summary>
        Ipcb3Serial = 0x293,

        /// <summary>
        /// VT_UI4, R/W | серийный номер платы индикации 4
        /// </summary>
        Ipcb4Serial = 0x294,

        /// <summary>
        /// VT_UI4, R/W | серийный номер платы индикации 5
        /// </summary>
        Ipcb5Serial = 0x295,

        /// <summary>
        /// VT_UI4, R/W | серийный номер платы индикации 6
        /// </summary>
        Ipcb6Serial = 0x296,

        /// <summary>
        /// VT_UI4, R/W | серийный номер платы индикации 7
        /// </summary>
        Ipcb7Serial = 0x297,

        /// <summary>
        /// VT_LPSTR, R | дата калибровки платы индикации 0
        /// </summary>
        Ipcb0CalDate = 0x298,

        /// <summary>
        /// VT_LPSTR, R | дата калибровки платы индикации 1
        /// </summary>
        Ipcb1CalDate = 0x299,

        /// <summary>
        /// VT_LPSTR, R | дата калибровки платы индикации 2
        /// </summary>
        Ipcb2CalDate = 0x29a,

        /// <summary>
        /// VT_LPSTR, R | дата калибровки платы индикации 3
        /// </summary>
        Ipcb3CalDate = 0x29b,

        /// <summary>
        /// VT_LPSTR, R | дата калибровки платы индикации 4
        /// </summary>
        Ipcb4CalDate = 0x29c,

        /// <summary>
        /// VT_LPSTR, R | дата калибровки платы индикации 5
        /// </summary>
        Ipcb5CalDate = 0x29d,

        /// <summary>
        /// VT_LPSTR, R | дата калибровки платы индикации 6
        /// </summary>
        Ipcb6CalDate = 0x29e,

        /// <summary>
        /// VT_LPSTR, R | дата калибровки платы индикации 7
        /// </summary>
        Ipcb7CalDate = 0x29f,

        /// <summary>
        /// VT_UI4, R/W | конфигурация таблицы MC Extender 0
        /// </summary>
        McExt0Conf = 0x2a0,

        /// <summary>
        /// VT_UI4, R/W | конфигурация таблицы MC Extender 1
        /// </summary>
        McExt1Conf = 0x2a1,

        /// <summary>
        /// VT_UI4, R/W | конфигурация таблицы MC Extender 2
        /// </summary>
        McExt2Conf = 0x2a2,

        /// <summary>
        /// VT_UI4, R/W | конфигурация таблицы MC Extender 3
        /// </summary>
        McExt3Conf = 0x2a3,

        /// <summary>
        /// VT_UI4, R/W | конфигурация таблицы MC Extender 4
        /// </summary>
        McExt4Conf = 0x2a4,

        /// <summary>
        /// VT_UI4, R/W | конфигурация таблицы MC Extender 5
        /// </summary>
        McExt5Conf = 0x2a5,

        /// <summary>
        /// VT_UI4, R/W | конфигурация таблицы MC Extender 6
        /// </summary>
        McExt6Conf = 0x2a6,

        /// <summary>
        /// VT_UI4, R/W | конфигурация таблицы MC Extender 7
        /// </summary>
        McExt7Conf = 0x2a7,

        /// <summary>
        /// VT_UI2, R | VCC1 платы индикации 0
        /// </summary>
        Ipcb0Vcc1 = 0x2a8,

        /// <summary>
        /// VT_UI2, R | VCC1 платы индикации 1
        /// </summary>
        Ipcb1Vcc1 = 0x2a9,

        /// <summary>
        /// VT_UI2, R | VCC1 платы индикации 2
        /// </summary>
        Ipcb2Vcc1 = 0x2aa,

        /// <summary>
        /// VT_UI2, R | VCC1 платы индикации 3
        /// </summary>
        Ipcb3Vcc1 = 0x2ab,

        /// <summary>
        /// VT_UI2, R | VCC1 платы индикации 4
        /// </summary>
        Ipcb4Vcc1 = 0x2ac,

        /// <summary>
        /// VT_UI2, R | VCC1 платы индикации 5
        /// </summary>
        Ipcb5Vcc1 = 0x2ad,

        /// <summary>
        /// VT_UI2, R | VCC1 платы индикации 6
        /// </summary>
        Ipcb6Vcc1 = 0x2ae,

        /// <summary>
        /// VT_UI2, R | VCC1 платы индикации 7
        /// </summary>
        Ipcb7Vcc1 = 0x2af,

        /// <summary>
        /// VT_UI2, R | VCC2 платы индикации 0
        /// </summary>
        Ipcb0Vcc2 = 0x2b0,

        /// <summary>
        /// VT_UI2, R | VCC2 платы индикации 1
        /// </summary>
        Ipcb1Vcc2 = 0x2b1,

        /// <summary>
        /// VT_UI2, R | VCC2 платы индикации 2
        /// </summary>
        Ipcb2Vcc2 = 0x2b2,

        /// <summary>
        /// VT_UI2, R | VCC2 платы индикации 3
        /// </summary>
        Ipcb3Vcc2 = 0x2b3,

        /// <summary>
        /// VT_UI2, R | VCC2 платы индикации 4
        /// </summary>
        Ipcb4Vcc2 = 0x2b4,

        /// <summary>
        /// VT_UI2, R | VCC2 платы индикации 5
        /// </summary>
        Ipcb5Vcc2 = 0x2b5,

        /// <summary>
        /// VT_UI2, R | VCC2 платы индикации 6
        /// </summary>
        Ipcb6Vcc2 = 0x2b6,

        /// <summary>
        /// VT_UI2, R | VCC2 платы индикации 7
        /// </summary>
        Ipcb7Vcc2 = 0x2b7,

        /// <summary>
        /// VT_UI4, R | статуc MC Extender 0
        /// </summary>
        McExt0Status = 0x2c0,

        /// <summary>
        /// VT_UI4, R | статуc MC Extender 1
        /// </summary>
        McExt1Status = 0x2c1,

        /// <summary>
        /// VT_UI4, R | статуc MC Extender 2
        /// </summary>
        McExt2Status = 0x2c2,

        /// <summary>
        /// VT_UI4, R | статуc MC Extender 3
        /// </summary>
        McExt3Status = 0x2c3,

        /// <summary>
        /// VT_UI4, R | статуc MC Extender 4
        /// </summary>
        McExt4Status = 0x2c4,

        /// <summary>
        /// VT_UI4, R | статуc MC Extender 5
        /// </summary>
        McExt5Status = 0x2c5,

        /// <summary>
        /// VT_UI4, R | статуc MC Extender 6
        /// </summary>
        McExt6Status = 0x2c6,

        /// <summary>
        /// VT_UI4, R | статуc MC Extender 7
        /// </summary>
        McExt7Status = 0x2c7,

        /// <summary>
        /// VT_LPSTR, R | целевая точка калибровки платы индикации 0
        /// </summary>
        Ipcb0Tinfo = 0x2d0,

        /// <summary>
        /// VT_LPSTR, R | целевая точка калибровки платы индикации 1
        /// </summary>
        Ipcb1Tinfo = 0x2d1,

        /// <summary>
        /// VT_LPSTR, R | целевая точка калибровки платы индикации 2
        /// </summary>
        Ipcb2Tinfo = 0x2d2,

        /// <summary>
        /// VT_LPSTR, R | целевая точка калибровки платы индикации 3
        /// </summary>
        Ipcb3Tinfo = 0x2d3,

        /// <summary>
        /// VT_LPSTR, R | целевая точка калибровки платы индикации 4
        /// </summary>
        Ipcb4Tinfo = 0x2d4,

        /// <summary>
        /// VT_LPSTR, R | целевая точка калибровки платы индикации 5
        /// </summary>
        Ipcb5Tinfo = 0x2d5,

        /// <summary>
        /// VT_LPSTR, R | целевая точка калибровки платы индикации 6
        /// </summary>
        Ipcb6Tinfo = 0x2d6,

        /// <summary>
        /// VT_LPSTR, R | целевая точка калибровки платы индикации 7
        /// </summary>
        Ipcb7Tinfo = 0x2d7,

        /// <summary>
        /// VT_UI1, R/W | текущий режим индикации
        /// </summary>
        IndicationMode = 0x301,

        /// <summary>
        /// VT_BOOL, R/W | зеркалирование
        /// </summary>
        Mirror = 0x302,

        /// <summary>
        /// VT_UI1, R/W | поворот
        /// </summary>
        Rotate = 0x303,

        /// <summary>
        /// VT_UI1, R/W | яркость отображения
        /// </summary>
        Brightness = 0x304,

        /// <summary>
        /// VT_UI1, R/W | контраст отображения
        /// </summary>
        Contrast = 0x305,

        /// <summary>
        /// VT_UI2, R/W | горизонтальное разрешение экрана
        /// </summary>
        Hres = 0x306,

        /// <summary>
        /// VT_UI2, R/W | вертикальное разрешение экрана
        /// </summary>
        Vres = 0x307,

        /// <summary>
        /// VT_UI2, R/W | горизонтальное смещение экрана
        /// </summary>
        Hofs = 0x308,

        /// <summary>
        /// VT_UI2, R/W | вертикальное смещение экрана
        /// </summary>
        Vofs = 0x309,

        /// <summary>
        /// VT_BOOL,R/W | единая цветовая кривая для всех каналов
        /// </summary>
        UniColorCurve = 0x30a,

        /// <summary>
        /// VT_UI1, R/W | гамма экрана
        /// </summary>
        Gamma = 0x30b,

        /// <summary>
        /// VT_I1, R/W | точка черного
        /// </summary>
        Bp = 0x30c,

        /// <summary>
        /// VT_UI1, R/W | точка белого, % [0..100]
        /// </summary>
        Wp = 0x30d,

        /// <summary>
        /// VT_UI1, R/W | гамма красного канала
        /// </summary>
        GammaRed = Gamma,

        /// <summary>
        /// VT_UI1, R/W | гамма зеленого канала
        /// </summary>
        GammaGreen = 0x30e,

        /// <summary>
        /// VT_UI1, R/W | гамма синего канала
        /// </summary>
        GammaBlue = 0x30f,

        /// <summary>
        /// VT_I1, R/W | точка черного красного канала
        /// </summary>
        BpRed = Bp,

        /// <summary>
        /// VT_I1, R/W | точка черного зеленого канала
        /// </summary>
        BpGreen = 0x310,

        /// <summary>
        /// VT_I1, R/W | точка черного синего канала
        /// </summary>
        BpBlue = 0x311,

        /// <summary>
        /// VT_I1,R/W | точка белого красного канала, % [0..100]
        /// </summary>
        WpRed = Wp,

        /// <summary>
        /// VT_UI1,R/W | точка белого зеленого канала, % [0..100]
        /// </summary>
        WpGreen = 0x312,

        /// <summary>
        /// VT_UI1,R/W | точка белого синего канала, % [0..100]
        /// </summary>
        WpBlue = 0x313,

        /// <summary>
        /// VT_UI1, R | статус PLD
        /// </summary>
        PldStatus = 0x314,

        /// <summary>
        /// VT_UI2, R | 3.3V VCC
        /// </summary>
        CoreVcc = 0x315,

        /// <summary>
        /// VT_UI2, R | 1.5V VCC
        /// </summary>
        PldcoreVcc = 0x316,

        /// <summary>
        /// VT_UI2, R | 5.0V VCC
        /// </summary>
        LedVcc = 0x317,

        /// <summary>
        /// VT_BOOL, R/W | режим произвольной адресации
        /// </summary>
        Aad = 0x322,

        /// <summary>
        /// VT_BOOL, R/W | продолжить режим произвольной адресации на выходе данного модуля
        /// </summary>
        Caad = 0x323,

        /// <summary>
        /// VT_BOOL, R/W | режим виртуального пиксела
        /// </summary>
        Vpixel = 0x324,

        /// <summary>
        /// VT_UI4, R/W | цвет визуальных тестов
        /// </summary>
        TestColor = 0x325,

        /// <summary>
        /// VT_BOOL, R/W | использовать попиксельную коррекцию яркости
        /// </summary>
        Ppba = 0x326,

        /// <summary>
        /// VT_UI1, R | текущий статус использования PPBA
        /// </summary>
        PpbaStatus = 0x327,

        /// <summary>
        /// VT_BOOL, R/W | запрет использования разрешения 640x480
        /// </summary>
        LowResolution = 0x328,

        /// <summary>
        /// VT_UI1, R | используемая частота DVI-потока
        /// </summary>
        DviClock = 0x329,

        /// <summary>
        /// VT_UI2, R | device identifier
        /// </summary>
        DeviceId = 0x32a,

        /// <summary>
        /// VT_UI1, R/W | разрешенная частота DVI-потока
        /// </summary>
        DviClockLimit = 0x32b,

        /// <summary>
        /// VT_UI1, R | RGA
        /// </summary>
        Rga = 0x347,

        /// <summary>
        /// VT_UI1, R | RGB
        /// </summary>
        Rgb = 0x348,

        /// <summary>
        /// VT_UI1, R | RGC
        /// </summary>
        Rgc = 0x349,

        /// <summary>
        /// VT_UI4, R | ошибок при перезагрузке PPBA
        /// </summary>
        PpbaErrors = 0x34a,

        /// <summary>
        /// VT_BOOL, R | занятость PPBA
        /// </summary>
        PpbaBusy = 0x34b,

        /// <summary>
        /// VT_BOOL, R | журналировать операции с PPBA
        /// </summary>
        PpbaLog = 0x34c,

        /// <summary>
        /// VT_UI2, R | реинициализаций TWI
        /// </summary>
        TwiReboots = 0x34d,
    }
}