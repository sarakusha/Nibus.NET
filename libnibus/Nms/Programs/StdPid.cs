//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// StdPid.cs
// 
//-------------------------------------------------------------------

namespace NataInfo.Nibus.Nms.Programs
{
    public enum StdPid
    {
        /// <summary>
        ///подпрограмма выдачи сигнала
        // @parm VT_UI2 | длительность, ms
        // @parm VT_UI1 | источник
        /// </summary>
        Sound = 2,

        /// <summary>
        /// подпрограмма самотестирования
        /// </summary>
        SelfTest = 3,

        /// <summary>
        /// Обнулить статистику
        /// </summary>
        ClearStat = 4,

        /// <summary>
        /// Обнулить журнал
        /// </summary>
        ClearLog = 5
    }
}