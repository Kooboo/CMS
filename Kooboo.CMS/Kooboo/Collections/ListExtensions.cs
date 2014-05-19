#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Collections
{
    public static class ListExtensions
    {
        public static List<T> Add<T>(this List<T> list, T item, IEqualityComparer<T> comparer)
        {
            if (!list.Contains(item, comparer))
            {
                list.Add(item);
            }
            return list;
        }

        public static List<T> AddRange<T>(this List<T> list, IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            foreach (var item in items)
            {
                list.Add(item, comparer);
            }
            return list;
        }
    }
}
