//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// McdviPid.cs
// 
//-------------------------------------------------------------------

namespace NataInfo.Nibus.Nms.Programs
{
    public enum McdviPid
    {
        /// <summary>
        /// очистка файловой системы платы индикации
        /// </summary>
        ClearPpbatf = 0x100,

        /// <summary>
        /// повторная загрузка таблиц попиксельной коррекции
        /// </summary>
        ReloadPpbat = 0x101,

        /// <summary>
        /// изменение серийного номера платы индикации
        /// </summary>
        WriteIpcbSerial = 0x102,

        /// <summary>
        /// изменение даты калибровки платы индикации
        /// </summary>
        WriteIpcbCalDate = 0x103,

        /// <summary>
        /// запись конфигурации
        /// </summary>
        WriteConfig = 0x104,

        /// <summary>
        /// изменение способа измерения VCC плат индикации
        /// </summary>
        WriteIpcbVccMtype = 0x105,

        /// <summary>
        /// запись целевой точки калибровки
        /// </summary>
        WriteIpcbTinfo = 0x106,
    }
}
