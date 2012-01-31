//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// StdNms.cs
// 
//-------------------------------------------------------------------

namespace NataInfo.Nibus.Nms.Variables
{
    public enum StdNms
    {
        /// <summary>
        /// char[]  ex: 'DEVLOADER'
        /// </summary>
        SoftwareId = 1,

        /// <summary>
        /// Старшее слово - номер версии, младшее слово Build number
        /// </summary>
        SoftwareVersion = 2,

        /// <summary>
        /// количество миллисекунд с начала старта системы.
        /// </summary>
        Uptime = 3,
        Datetime = 4,

        /// <summary>
        /// температура. VT_I1/VT_I2
        /// </summary>
        Temperature = 0x80,

        /// <summary>
        /// домен. VT_UI1
        /// </summary>
        Domain = 0x81,

        /// <summary>
        /// группа. VT_UI1
        /// </summary>
        Group = 0x82,

        /// <summary>
        /// идентификатор устройства. VT_UI2
        /// </summary>
        DeviceId = 0x83,

        /// <summary>
        /// подсеть устройства. VT_UI1
        /// </summary>
        Subnet = 0x86,

        /// <summary>
        /// Версия PCB
        /// </summary>
        PcbVersion = 0x87,
    }
}