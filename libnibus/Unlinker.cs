//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Unlinker.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;

#endregion

namespace NataInfo.Nibus
{
    internal class Unlinker : IDisposable
    {
        #region Member Variables

        private IDisposable _firstCodecLink;
        private IDisposable _secondCodecLink;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public Unlinker(IDisposable firstCodecLink, IDisposable secondCodecLink)
        {
            _firstCodecLink = firstCodecLink;
            _secondCodecLink = secondCodecLink;
        }

        #endregion //Constructors

        #region Properties

        public bool Disposed
        {
            get { return _firstCodecLink == null && _secondCodecLink == null; }
        }

        #endregion //Properties

        public void Dispose()
        {
            if (_firstCodecLink != null)
            {
                _firstCodecLink.Dispose();
                _firstCodecLink = null;
            }

            if (_secondCodecLink != null)
            {
                _secondCodecLink.Dispose();
                _secondCodecLink = null;
            }
        }
    }
}
