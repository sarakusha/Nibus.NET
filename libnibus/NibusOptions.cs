//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NibusOptions.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

#endregion

namespace NataInfo.Nibus
{
    /// <summary>
    /// Параметры NiBUS-операций.
    /// </summary>
    public class NibusOptions
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NibusOptions() : this(null)
        {
        }

        public NibusOptions(NibusOptions other)
        {
            if (other != null)
            {
                Attempts = other.Attempts;
                Timeout = other.Timeout;
                Token = other.Token;
                Progress = other.Progress;
            }
            else
            {
                Attempts = 1;
                Timeout = TimeSpan.FromSeconds(2);
                Token = CancellationToken.None;
                Progress = null;
            }
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Количество попыток для повторных запросов в случае неудачи.
        /// </summary>
        public int Attempts { get; set; }

        /// <summary>
        /// Задает величину таймаута ожидания ответа на запросы.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Интерфейс для информрования о прогрессе длительной опреации.
        /// </summary>
        public IProgress<object> Progress { get; set; }

        /// <summary>
        /// Токен отмены операции.
        /// </summary>
        public CancellationToken Token { get; set; }

        #endregion //Properties
    }
}
