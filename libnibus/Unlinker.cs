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
    /// <summary>
    /// Класс для разрыва связи между кодеками <see cref="INibusCodec{TEncoded,TDecoded}"/>,
    /// созданной с помощью <see cref="NibusCodec{TEncoded,TDecoded}.ConnectTo{T}"/>
    /// </summary>
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
        /// <param name="firstCodecLink">Связь от первого кодека.</param>
        /// <param name="secondCodecLink">Связь от второго кодека.</param>
        public Unlinker(IDisposable firstCodecLink, IDisposable secondCodecLink)
        {
            _firstCodecLink = firstCodecLink;
            _secondCodecLink = secondCodecLink;
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this <see cref="Unlinker"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
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
