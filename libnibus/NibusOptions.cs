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
    public class NibusOptions
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NibusOptions()
        {
            Attempts = 1;
            Timeout = TimeSpan.FromSeconds(2);
            Token = CancellationToken.None;
            Progress = null;
        }

        #endregion //Constructors

        #region Properties

        public int Attempts { get; set; }

        public TimeSpan Timeout { get; set; }

        public IProgress<int> Progress { get; set; }

        public CancellationToken Token { get; set; }

        #endregion //Properties
    }
}
