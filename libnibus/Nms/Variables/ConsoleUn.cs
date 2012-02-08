//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// ConsoleUn.cs
// 
//-------------------------------------------------------------------

// ReSharper disable UnusedMember.Global
namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// Универсальный четырех-кнопочный пульт.
    /// </summary>
    /// <remarks><c>console_un.mib.xsd</c> - mib-файл</remarks>
    public enum ConsoleUn
    {
        /// <summary>
        /// Тип устройства. Не является переменной.
        /// </summary>
        DeviceTypeId = 3,
        /// <summary>
        /// VT_BOOL | внутренний светодиод 1
        /// </summary>
        Led1 = 0x100,

        /// <summary>
        /// VT_BOOL | внутренний светодиод 2
        /// </summary>
        Led2 = 0x101,

        /// <summary>
        /// VT_BOOL | внутренний светодиод 3
        /// </summary>
        Led3 = 0x102,

        /// <summary>
        /// VT_BOOL | внутренний светодиод 4
        /// </summary>
        Led4 = 0x103,

        /// <summary>
        /// VT_BOOL | внешний источник 1
        /// </summary>
        Led5 = 0x104,

        /// <summary>
        /// VT_BOOL | внешний источник 2
        /// </summary>
        Led6 = 0x105,

        /// <summary>
        /// VT_UI1 | все выводы вместе
        /// </summary>
        AllLeds = 0x106,
    }
}