//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// Extensions.cs
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
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int maxLength)
        {
            int i = 0;
            var splits = from item in list
                         group item by i++ / maxLength into part
                         select part.AsEnumerable();
            return splits;
        }
    }
}
