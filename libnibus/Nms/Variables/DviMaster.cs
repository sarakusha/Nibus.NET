//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// DviMaster.cs
// 
//-------------------------------------------------------------------

// ReSharper disable UnusedMember.Global
namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// DVI-мастер.
    /// </summary>
    /// <remarks><c>dvimaster.mib.xsd</c> - mib-файл</remarks>
    public enum DviMaster
    {
        /// <summary>
        /// Тип устройства. Не является переменной.
        /// </summary>
        DeviceTypeId = 0x11,

        /// <summary>
        /// VT_UI1,R/W | яркость [0..255]
        /// </summary>
        Brightness = 0x100,

        /// <summary>
        /// VT_UI1,R/W | контраст [0..100]
        /// </summary>
        Contrast = 0x101,

        /// <summary>
        /// VT_BOOL,R/W | единая цветовая кривая для всех каналов
        /// </summary>
        UniColorCurve = 0x102,

        /// <summary>
        /// VT_UI1,R/W | гамма красного канала [0..200] => [1,0..3,0]
        /// </summary>
        GammaRed = 0x103,

        /// <summary>
        /// VT_UI1,R/W | гамма зеленого канала [0..200] => [1,0..3,0]
        /// </summary>
        GammaGreen = 0x104,

        /// <summary>
        /// VT_UI1,R/W | гамма синего канала [0..200] => [1,0..3,0]
        /// </summary>
        GammaBlue = 0x105,

        /// <summary>
        /// VT_UI1,R/W | точка белого красного канала, % [0..100]
        /// </summary>
        WpRed = 0x106,

        /// <summary>
        /// VT_UI1,R/W | точка белого зеленого канала, % [0..100]
        /// </summary>
        WpGreen = 0x107,

        /// <summary>
        /// VT_UI1,R/W | точка белого синего канала, % [0..100]
        /// </summary>
        WpBlue = 0x108,

        /// <summary>
        /// VT_I1,R/W | точка черного красного канала, [-30..30], градации исходного изображения
        /// </summary>
        BpRed = 0x109,

        /// <summary>
        /// VT_I1,R/W | точка черного зеленого канала, [-30..30], градации исходного изображения
        /// </summary>
        BpGreen = 0x10a,

        /// <summary>
        /// VT_I1,R/W | точка черного синего канала, [-30..30], градации исходного изображения
        /// </summary>
        BpBlue = 0x10b,

        /// <summary>
        /// VT_UI1,R | частота в LVDS-канале [0=40MHz,1=60MHz]
        /// </summary>
        LvdsFreq = 0x10c,

        /// <summary>
        /// VT_BOOL,R | состояние оптического передатчика
        /// </summary>
        FiberTxState = 0x10d,

        /// <summary>
        /// VT_UI1,R/W | Коммутация выхода DVI-OUT [0-fiber,1-dvi]
        /// </summary>
        DviOut = 0x10e,

        /// <summary>
        /// VT_UI1,R/W | Коммутация выхода OPT-OUT [0-fiber,1-dvi]
        /// </summary>
        FiberOut = 0x10f,

        /// <summary>
        /// VT_UI1,R/W | Коммутация выхода LVDS-OUT [0-fiber,1-dvi]
        /// </summary>
        LvdsOut = 0x110,

        /// <summary>
        /// VT_BOOL,R/W | подать тестовый сигнал на выход DVI-OUT
        /// </summary>
        DviOutTest = 0x111,

        /// <summary>
        /// VT_BOOL,R/W | подать тестовый сигнал на выход OPT-OUT
        /// </summary>
        FiberOutTest = 0x112,

        /// <summary>
        /// VT_BOOL,R/W | подать тестовый сигнал на выход LVDS-OUT
        /// </summary>
        LvdsOutTest = 0x113,

        /// <summary>
        /// VT_BOOL,R | состояние входа DVI-IN
        /// </summary>
        DviReceiverState = 0x114,

        /// <summary>
        /// VT_UI1,R | состояние выхода DVI-OUT [0-отсутствует,1-не активен,2-активен]
        /// </summary>
        DviTransmitterState = 0x115,

        /// <summary>
        /// VT_BOOL,R | состояние входа OPT-IN
        /// </summary>
        FiberReceiverState = 0x116,

        /// <summary>
        /// VT_UI4,R | Количество 4-х секундных интервалов с ошибкой по входу OPT-IN
        /// </summary>
        FiberErrors = 0x117,

        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы во входном i2c-интерфейсе
        /// </summary>
        I2CChecksumErrors = 0x118,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в i2c-интерфейсе
        /// </summary>
        I2CCollisions = 0x119,

        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы во входном rs485-интерфейсе
        /// </summary>
        ChecksumErrors = 0x11a,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в rs485-интерфейсе
        /// </summary>
        Collisions = 0x11b,

        /// <summary>
        /// VT_UI1,R | Версия прошивки PLD
        /// </summary>
        PldVersion = 0x11c,

        /// <summary>
        /// VT_BOOL,R/W | Режим теста сигнального тракта
        /// </summary>
        DatapathTestMode = 0x11d,

        /// <summary>
        /// VT_BOOL,R/W | Генератор тестовой последовательности
        /// </summary>
        DatapathTestGenerator = 0x11e,

        /// <summary>
        /// VT_UI4,R | Ошибок на DVI-IN интерфейсе при тесте сигнального тракта
        /// </summary>
        DatapathDviErrors = 0x11f,

        /// <summary>
        /// VT_UI4,R | Ошибок на OPT-IN интерфейсе при тесте сигнального тракта
        /// </summary>
        DatapathFiberErrors = 0x120,

        /// <summary>
        /// VT_UI2,R | Версия PCB
        /// </summary>
        PcbVersion = 0x121,
    }
}