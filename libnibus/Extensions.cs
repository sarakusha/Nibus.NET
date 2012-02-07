//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Extensions.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

namespace NataInfo.Nibus
{
    /// <summary>
    /// Методы расширения LINQ.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Разделяет последовательность на подпоследовательности длиной <paramref name="maxLength"/>.
        /// </summary>
        /// <typeparam name="T">Тип элементов последовательности <paramref name="source"/>.</typeparam>
        /// <param name="source">Последовательность значений, для которых вызывается функция преобразования.</param>
        /// <param name="maxLength">Количество элементов в подпоследовательности.</param>
        /// <returns>Объект <see cref="IEnumerable{T}"/>, элементы которого содержат полученные подпоследовательности.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int maxLength)
        {
            var i = 0;
            var splits = from item in source
                         group item by i++ / maxLength into part
                         select part.AsEnumerable();
            return splits;
        }
    }
}
