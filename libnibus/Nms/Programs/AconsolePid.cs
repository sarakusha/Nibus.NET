//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// AconsolePid.cs
// 
//-------------------------------------------------------------------

namespace NataInfo.Nibus.Nms.Programs
{
    /// <summary>
    /// model 0x0012xxxx | Консоль табло времени атаки
    /// </summary>
    public enum AconsolePid
    {
        /// <summary>
        /// старт отсчета
        /// </summary>
        Start = 0x100,

        /// <summary>
        /// стоп отсчета
        /// </summary>
        Stop = 0x101,

        /// <summary>
        /// рестарт отсчета
        /// </summary>
        Restart = 0x102,

        /// <summary>
        /// сброс отсчета
        /// </summary>
        Reset = 0x103,

        /// <summary>
        /// сигнал
        /// </summary>
        Beep = 0x104,
    }
}