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
        /// VT_BOOL,R/W	| Возрастание времени | Игровой таймер
        /// </summary>
        Timer1Direct = 0x130,

        /// <summary>
        /// VT_UI1,R/W	| Минуты | Игровой таймер
        /// </summary>
        Timer1Min = 0x131,

        /// <summary>
        /// VT_UI1,R/W	| Секунды | Игровой таймер
        /// </summary>
        Timer1Sec = 0x132,

        /// <summary>
        /// VT_UI1,R/W	| Показывать милисекунды | Игровой таймер
        /// </summary>
        Timer1ShowMs = 0x133,

        /// <summary>
        /// VT_UI1,R/W	| Другая информация | Игровой таймер
        /// </summary>
        Timer1Represt = 0x134,

        /// <summary>
        /// VT_BOOL,R/W	| Возрастание времени | Таймер перерыва
        /// </summary>
        Timer2Direct = 0x135,

        /// <summary>
        /// VT_UI1,R/W	| Минуты | Таймер перерыва
        /// </summary>
        Timer2Min = 0x136,

        /// <summary>
        /// VT_UI1,R/W	| Секунды | Таймер перерыва
        /// </summary>
        Timer2Sec = 0x137,

        /// <summary>
        /// VT_UI1,R/W	| Показывать милисекунды | Таймер перерыва
        /// </summary>
        Timer2ShowMs = 0x138,

        /// <summary>
        /// VT_UI1,R/W	| Другая информация | Таймер перерыва
        /// </summary>
        Timer2Represt = 0x139,

        /// <summary>
        /// VT_BOOL,R/W	| Возрастание времени | Время до начала матча
        /// </summary>
        Timer3Direct = 0x13a,

        /// <summary>
        /// VT_UI1,R/W	| Минуты | Время до начала матча
        /// </summary>
        Timer3Min = 0x13b,

        /// <summary>
        /// VT_UI1,R/W	| Секунды | Время до начала матча
        /// </summary>
        Timer3Sec = 0x13c,

        /// <summary>
        /// VT_UI1,R/W	| Показывать милисекунды | Время до начала матча
        /// </summary>
        Timer3ShowMs = 0x13d,

        /// <summary>
        /// VT_UI1,R/W	| Другая информация | Время до начала матча
        /// </summary>
        Timer3Represt = 0x13e,

        /// <summary>
        /// VT_BOOL,R/W	| Возрастание времени | Таймаут
        /// </summary>
        Timer4Direct = 0x13f,

        /// <summary>
        /// VT_UI1,R/W	| Минуты | Таймаут
        /// </summary>
        Timer4Min = 0x140,

        /// <summary>
        /// VT_UI1,R/W	| Секунды | Таймаут
        /// </summary>
        Timer4Sec = 0x141,

        /// <summary>
        /// VT_UI1,R/W	| Показывать милисекунды | Таймаут
        /// </summary>
        Timer4ShowMs = 0x142,

        /// <summary>
        /// VT_UI1,R/W	| Другая информация | Таймаут
        /// </summary>
        Timer4Represt = 0x143,

        /// <summary>
        /// VT_BOOL,R/W	| Возрастание времени | Овертайм
        /// </summary>
        Timer5Direct = 0x144,

        /// <summary>
        /// VT_UI1,R/W	| Минуты | Овертайм
        /// </summary>
        Timer5Min = 0x145,

        /// <summary>
        /// VT_UI1,R/W	| Секунды | Овертайм
        /// </summary>
        Timer5Sec = 0x146,

        /// <summary>
        /// VT_UI1,R/W	| Показывать милисекунды | Овертайм
        /// </summary>
        Timer5ShowMs = 0x147,

        /// <summary>
        /// VT_UI1,R/W	| Другая информация | Овертайм
        /// </summary>
        Timer5Represt = 0x148,

        /// <summary>
        /// VT_BOOL,R/W	| Возрастание времени | Перерыв овертайма
        /// </summary>
        Timer6Direct = 0x149,

        /// <summary>
        /// VT_UI1,R/W	| Минуты | Перерыв овертайма
        /// </summary>
        Timer6Min = 0x14a,

        /// <summary>
        /// VT_UI1,R/W	| Секунды | Перерыв овертайма
        /// </summary>
        Timer6Sec = 0x14b,

        /// <summary>
        /// VT_UI1,R/W	| Показывать милисекунды | Перерыв овертайма
        /// </summary>
        Timer6ShowMs = 0x14c,

        /// <summary>
        /// VT_UI1,R/W	| Другая информация | Перерыв овертайма
        /// </summary>
        Timer6Represt = 0x14d,

        /// <summary>
        /// Автоматический запуск перерыва(овертайма)
        /// </summary>
        AutoStartRest = 0x14e,

        /// <summary>
        /// Задержка запуска перерыва(овертайма) (сек.)
        /// </summary>
        DelayStartRest = 0x14f,

        /// <summary>
        /// Автоматическое увеличение периода.
        /// </summary>
        IncPeriod = 0x150,

        /// <summary>
        /// Запуск времени до игры.
        /// </summary>
        StartBeforeGame = 0x151,

        /// <summary>
        /// Кол-во тайм-аутов в периоде.
        /// </summary>
        MaxRests = 0x152,

        /// <summary>
        /// Старт/Стоп на кнопке "старт".
        /// </summary>
        StopOnStart = 0x153,

        /// <summary>
        /// Игра с овертаймом.
        /// </summary>
        EnableOvertime = 0x154,

        /// <summary>
        /// Время штрафа №1 (мин.)
        /// </summary>
        Penalty1 = 0x155,

        /// <summary>
        /// Время штрафа №2 (мин.)
        /// </summary>
        Penalty2 = 0x156,

        /// <summary>
        /// Время штрафа №3 (мин.)
        /// </summary>
        Penalty3 = 0x157,

        /// <summary>
        /// Сигналы при отсчете времени до игры.
        /// </summary>
        SignalBeforeGame = 0x158,

        /// <summary>
        /// Сигналы при отсчете времени перерыва.
        /// </summary>
        SignalRest = 0x159,

        /// <summary>
        /// Кол-во строк для штрафов
        /// </summary>
        PenaltyRows = 0x15a,

        /// <summary>
        /// Пароль для защиты
        /// </summary>
        VaridPassword = 0x160,

        /// <summary>
        /// Ошибок синхронизации на rs232 интерфейсе
        /// </summary>
        FramingErrors = 0x161,

        /// <summary>
        /// Ошибок контрольной суммы в rs232 интерфейсе
        /// </summary>
        ChecksumErrors = 0x162,

        /// <summary>
        /// Принято дейтаграмм по rs232 интерфейсу
        /// </summary>
        RxDatagrams = 0x163,

        /// <summary>
        /// Переполнений фифо
        /// </summary>
        FifoOverruns = 0x164,

        /// <summary>
        /// Отослано дейтаграмм по rs232 интерфейсу
        /// </summary>
        TxDatagrams = 0x166,

        /// <summary>
        /// Не отправленных дейтаграмм по rs232 интерфейсу
        /// </summary>
        TxFailedDatagrams = 0x167,

        /// <summary>
        /// Кол-во периодов.
        /// </summary>
        MaxPeriod = 0x165,

        /// <summary>
        /// Выдавать сигнал на окончание таймаута.[x]
        /// </summary>
        SignalTimeout = 0x169,

        /// <summary>
        /// Время штрафа №4 (мин.)[x]
        /// </summary>
        Penalty4 = 0x16a,

        /// <summary>
        /// Использовать задержку штрафов.
        /// </summary>
        PenaltyDelay = 0x16b,

        /// <summary>
        /// VT_BOOL | Штраф в секундах при вводе с пульта.[x]
        /// </summary>
        PenaltyInSec = 0x16c,

        /// <summary>
        /// Сбрасывать таймауты по истечении периода.
        /// </summary>
        ResetTimeout = 0x16d,

        /// <summary>
        /// Максимальное количество овертаймов за период.[x]
        /// </summary>
        MaxOvertime = 0x16e,
    }
}