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

        private readonly List<IDisposable> _links;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public Unlinker()
        {

            _links = new List<IDisposable>();
        }

        #endregion //Constructors

        public void AddLink(IDisposable link)
        {
            _links.Add(link);
        }

        public void Dispose()
        {
            _links.Where(link => link != null).ToList().ForEach(link => link.Dispose());
        }
    }
}
