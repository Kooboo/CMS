#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Kooboo.Collections
{
    /// <summary>
    /// 
    /// </summary>

    public static class DictionaryExtensions
    {
        /// <summary>
        /// To the name value collection.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dic">The dic.</param>
        /// <returns></returns>
        public static NameValueCollection ToNameValueCollection<TKey, TValue>(this IDictionary<TKey, TValue> dic)
        {
            NameValueCollection nameValues = new NameValueCollection();
            foreach (var key in dic.Keys)
            {
                nameValues[key.ToString()] = dic[key] == null ? "" : dic[key].ToString();
            }
            return nameValues;
        }

        /// <summary>
        /// Merges the specified dic.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dic">The dic.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            dic[key] = value;
            return dic;
        }

        /// <summary>
        /// Merges the specified source.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="dic1">The dic1.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> dic1)
        {
            if (dic1 != null)
            {
                foreach (var item in dic1)
                {
                    if (!source.ContainsKey(item.Key))
                    {
                        source.Add(item);
                    }
                }
            }
            return source;
        }
        
    }
}
