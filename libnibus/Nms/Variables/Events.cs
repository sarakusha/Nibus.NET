//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Events.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace NataInfo.Nibus.Nms.Variables
{
    /// <summary>
    /// Стандартные события.
    /// </summary>
    /// <seealso cref="NmsProtocol.FireEventNotification"/>
    public enum Events
    {
        /// <summary>
        /// нажатие кнопки (старший байт определяет скан-код)
        /// </summary>
        KeyDown = 1,

        /// <summary>
        /// отжатие кнопки (старший байт определяет скан-код)
        /// </summary>
        KeyUp = 2,

        /// <summary>
        /// старт/рестарт устройства
        /// </summary>
        PowerUp = 3,

        /// <summary>
        /// устройство живо
        /// </summary>
        HeartBeat = 4,

        /// <summary>
        /// запрос состояния игры
        /// </summary>
        RequestGameInfo = 5,

        /// <summary>
        /// игра остановлена
        /// </summary>
        GameStopped = 6,

        /// <summary>
        /// игра запущена
        /// </summary>
        GameRunned = 7,

        /// <summary>
        /// сработал красный фонарь
        /// </summary>
        RedLampAlert = 8,

        /// <summary>
        /// оператором запрошено тестирование устройств
        /// </summary>
        BeginTests = 9,

        /// <summary>
        /// оператором остановлено тестирование устройств
        /// </summary>
        EndTests = 10,

        /// <summary>
        /// строка пробежала      
        /// </summary>
        ClineEmpty = 0x100,
    }
}