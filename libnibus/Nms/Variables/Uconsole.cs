//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Uconsole.cs
// 
//-------------------------------------------------------------------

namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// @model 0x0012xxxx | Консоль универсальная
    /// </summary>
    public enum Uconsole
    {
        /// <summary>
        /// VT_UI1,R | Результат POST pcb-процесса
        /// </summary>
        UpostResult = 0x110,

        /// <summary>
        /// VT_UI1,R | Версия PCB
        /// </summary>
        PcbVersion = 0x111,

        /// <summary>
        /// VT_I1, R | ошибки при сохранении конфигурации
        /// </summary>
        PersistError = 0x112,

        /// <summary>
        /// VT_UI4,R | Повторений ответа на SARP-запрос
        /// </summary>
        SarpRetries = 0x113,

        /// <summary>
        /// VT_UI4,R | Ошибок при ответе на SARP-запрос
        /// </summary>
        SarpFailedResp = 0x114,

        /// <summary>
        /// VT_UI4,R | SARP-запросов
        /// </summary>
        SarpRequests = 0x115,

        /// <summary>
        /// VT_UI4,R | SARP-ответов
        /// </summary>
        SarpResponces = 0x116,

        /// <summary>
        /// VT_BOOL,R/W	| Возрастание времени
        /// </summary>
        Timer1Direct = 0x130,

        /// <summary>
        /// VT_UI1,R/W	| Минуты
        /// </summary>
        Timer1Min = 0x131,

        /// <summary>
        /// VT_UI1,R/W	| Секунды
        /// </summary>
        Timer1Sec = 0x132,

        /// <summary>
        /// VT_UI1,R/W	| Показывать милисекунды
        /// </summary>
        Timer1ShowMs = 0x133,

        /// <summary>
        /// VT_UI1,R/W	| Другая информация
        /// </summary>
        Timer1Represt = 0x134,

        Timer2Direct = 0x135,
        Timer2Min = 0x136,
        Timer2Sec = 0x137,
        Timer2ShowMs = 0x138,
        Timer2Represt = 0x139,

        Timer3Direct = 0x13a,
        Timer3Min = 0x13b,
        Timer3Sec = 0x13c,
        Timer3ShowMs = 0x13d,
        Timer3Represt = 0x13e,

        Timer4Direct = 0x13f,
        Timer4Min = 0x140,
        Timer4Sec = 0x141,
        Timer4ShowMs = 0x142,
        Timer4Represt = 0x143,

        Timer5Direct = 0x144,
        Timer5Min = 0x145,
        Timer5Sec = 0x146,
        Timer5ShowMs = 0x147,
        Timer5Represt = 0x148,

        Timer6Direct = 0x149,
        Timer6Min = 0x14a,
        Timer6Sec = 0x14b,
        Timer6ShowMs = 0x14c,
        Timer6Represt = 0x14d,

        StartRest = 0x14e,
        RestDelay = 0x14f,
        IncPeriod = 0x150,
        BeforeGameTimer = 0x151,
        MaxRests = 0x152,
        StopOnStart = 0x153,
        EnableOvertime = 0x154,

        Penalty1 = 0x155,
        Penalty2 = 0x156,
        Penalty3 = 0x157,

        SignalBeforeGame = 0x158,
        SignalRest = 0x159,
        PenaltyRows = 0x15a,

        VaridPassword = 0x160,

        RS232FramingErrors = 0x161,
        RS232ChecksumErrors = 0x162,
        RS232RxDatagrams = 0x163,
        RS232FifoOverruns = 0x164,
        RS232TxDatagrams = 0x166,
        RS232TxFailedDatagrams = 0x167,

        MaxPeriod = 0x165,
        SignalTimeout = 0x169,

        Penalty4 = 0x16a,
        PenaltyDelay = 0x16b,
        PenaltyInSec = 0x16c,

        ResetTimeout = 0x16d,
        MaxOvertime = 0x16e,
    }
}