//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Unlinker.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace NataInfo.Nibus
{
    internal class Unlinker : IDisposable
    {
        #region Member Variables

        private IDisposable _decoderLink;
        private IDisposable _encoderLink;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public Unlinker(IDisposable decoderLink, IDisposable encoderLink)
        {
            _decoderLink = decoderLink;
            _encoderLink = encoderLink;
        }

        #endregion //Constructors

        #region Properties

        public bool Disposed
        {
            get { return _decoderLink == null && _encoderLink == null; }
        }

        #endregion //Properties

        public void Dispose()
        {
            if (_decoderLink != null)
            {
                _decoderLink.Dispose();
                _decoderLink = null;
            }

            if (_encoderLink != null)
            {
                _encoderLink.Dispose();
                _encoderLink = null;
            }
        }
    }
}
