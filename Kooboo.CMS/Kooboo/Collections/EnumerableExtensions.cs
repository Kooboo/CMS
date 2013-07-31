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

namespace Kooboo.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Methods
        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            int i = 0;
            foreach (var each in source)
            {
                if (predicate(each))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        /// <summary>
        /// Returns the index of the first occurrence in a sequence by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="list">A sequence in which to locate a value.</param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <returns>
        /// The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1.
        /// </returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value) where TSource : IEquatable<TSource>
        {

            return list.IndexOf<TSource>(value, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Returns the index of the first occurrence in a sequence by using a specified IEqualityComparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="list">A sequence in which to locate a value.</param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1.
        /// </returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value, IEqualityComparer<TSource> comparer)
        {
            int index = 0;
            foreach (var item in list)
            {
                if (comparer.Equals(item, value))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }


        /// <summary>
        /// Compares the specified origianl.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="origianl">The origianl.</param>
        /// <param name="current">The current.</param>
        /// <param name="match">The match.</param>
        /// <param name="update">The update.</param>
        /// <returns></returns>
        public static ComparedResult<T> Compare<T>(this IEnumerable<T> origianl, IEnumerable<T> current, Func<T, T, bool> match, Action<T, T> update = null)
        {
            var result = new ComparedResult<T>();

            foreach (var o in origianl)//filter deleted object,update object
            {
                var find = current.FirstOrDefault(c => match(c, o));

                if (find == null)//it should be removed from original collection
                {
                    result.Deleted.Add(o);
                }
                else //it should be updated with latest object
                {
                    if (update != null)
                    {
                        update(o, find);
                    }

                    result.Updated.Add(o);
                }

            }

            foreach (var c in current)
            {
                var find = origianl.FirstOrDefault(o => match(o, c));
                if (find == null)//filter added object
                {
                    result.Added.Add(c);
                }
            }

            return result;
        }
        #endregion
    }
}
