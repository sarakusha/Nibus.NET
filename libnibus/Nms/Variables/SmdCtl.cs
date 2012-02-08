//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// SmdCtl.cs
// 
//-------------------------------------------------------------------

// ReSharper disable UnusedMember.Global

namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// Плата индикации на SMD
    /// </summary>
    /// <remarks><c>smdctl.mib.xsd</c> - mib-файл</remarks>
    public enum SmdCtl
    {
        /// <summary>
        /// Тип устройства. Не является переменной.
        /// </summary>
        DeviceTypeId = 0x15,
        /// <summary>
        /// VT_UI4,R | Ошибок контрольной суммы в nibus-интерфейсе
        /// </summary>
        ChecksumErrors = 0x100,

        /// <summary>
        /// VT_UI4,R | Коллизий при передаче дейтаграмм в nibus-интерфейс
        /// </summary>
        Collisions = 0x101,

        /// <summary>
        /// VT_UI4,R | Принято дейтаграмм
        /// </summary>
        RxDatagrams = 0x102,

        /// <summary>
        /// VT_UI4,R | Отослано дейтаграмм
        /// </summary>
        TxDatagrams = 0x103,

        /// <summary>
        /// VT_UI1, R/W | текущий режим индикации
        /// </summary>
        IndicationMode = 0x110,

        /// <summary>
        /// VT_UI1, R/W | яркость отображения
        /// </summary>
        Brightness = 0x111,

        /// <summary>
        /// VT_UI1, R/W | контраст отображения
        /// </summary>
        Contrast = 0x112,

        /// <summary>
        /// VT_UI2, R/W | горизонтальное разрешение экрана
        /// </summary>
        Hres = 0x113,

        /// <summary>
        /// VT_UI2, R/W | вертикальное разрешение экрана
        /// </summary>
        Vres = 0x114,

        /// <summary>
        /// VT_UI2, R/W | горизонтальное смещение экрана
        /// </summary>
        Hofs = 0x115,

        /// <summary>
        /// VT_UI2, R/W | вертикальное смещение экрана
        /// </summary>
        Vofs = 0x116,

        /// <summary>
        /// VT_UI1, R/W | гамма экрана
        /// </summary>
        Gamma = 0x117,

        /// <summary>
        /// VT_I1, R/W | точка черного
        /// </summary>
        Bp = 0x118,

        /// <summary>
        /// VT_UI1, R/W | точка белого, % [0..100]
        /// </summary>
        Wp = 0x119,

        /// <summary>
        /// VT_UI1, R | статус PLD
        /// </summary>
        PldStatus = 0x11a,

        /// <summary>
        /// VT_UI2, R | версия PLD
        /// </summary>
        PldVersion = 0x11b,

        /// <summary>
        /// VT_I1, R | ошибки при сохранении конфигурации
        /// </summary>
        PersistError = 0x11c,

        /// <summary>
        /// VT_I1, R | потерь синхронизации 
        /// </summary>
        PldSyncLosts = 0x11d,

        /// <summary>
        /// VT_BOOL, R/W | удаленное управление индикацией
        /// </summary>
        RemoteCtl = 0x11e,

        /// <summary>
        /// VT_BOOL, R/W | зеркалирование
        /// </summary>
        Mirror = 0x11f,

        /// <summary>
        /// VT_UI1, R/W | поворот
        /// </summary>
        Rotate = 0x120,

        /// <summary>
        /// VT_BOOL,R/W | единая цветовая кривая для всех каналов
        /// </summary>
        UniColorCurve = 0x121,

        /// <summary>
        /// VT_UI1, R/W | гамма красного канала
        /// </summary>
        GammaRed = Gamma,

        /// <summary>
        /// VT_UI1, R/W | гамма зеленого канала
        /// </summary>
        GammaGreen = 0x122,

        /// <summary>
        /// VT_UI1, R/W | гамма синего канала
        /// </summary>
        GammaBlue = 0x123,

        /// <summary>
        /// VT_I1, R/W | точка черного красного канала
        /// </summary>
        BpRed = Bp,

        /// <summary>
        /// VT_I1, R/W | точка черного зеленого канала
        /// </summary>
        BpGreen = 0x124,

        /// <summary>
        /// VT_I1, R/W | точка черного синего канала
        /// </summary>
        BpBlue = 0x125,

        /// <summary>
        /// VT_I1,R/W | точка белого красного канала, % [0..100]
        /// </summary>
        WpRed = Wp,

        /// <summary>
        /// VT_UI1,R/W | точка белого зеленого канала, % [0..100]
        /// </summary>
        WpGreen = 0x126,

        /// <summary>
        /// VT_UI1,R/W | точка белого синего канала, % [0..100]
        /// </summary>
        WpBlue = 0x127,

        /// <summary>
        /// VT_UI1,R/W | подсеть
        /// </summary>
        Subnet = 0x128,

        /// <summary>
        /// VT_BOOL,R/W | режим произвольной адресации
        /// </summary>
        Aad = 0x129,
    }
}