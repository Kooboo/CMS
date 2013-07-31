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

namespace Kooboo.Collections
{
    public class CachableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Fields
        private IDictionary<TKey, TValue> cache; 
        #endregion

        #region .ctor
        public CachableDictionary()
        {
            cache = new Dictionary<TKey, TValue>();
        }
        public CachableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.cache = dictionary;
        }
        public CachableDictionary(IEqualityComparer<TKey> comparer)
        {
            this.cache = new Dictionary<TKey, TValue>(comparer);
        }
        public CachableDictionary(int capacity)
        {
            this.cache = new Dictionary<TKey, TValue>(capacity);
        }
        public CachableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            this.cache = new Dictionary<TKey, TValue>(dictionary, comparer);
        }
        public CachableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            this.cache = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        /// <value>
        ///
        /// </value>
        /// <param name="key">The key.</param>
        /// <param name="createCachingValue">The create caching value.</param>
        /// <returns></returns>
        public TValue this[TKey key, Func<TValue> createCachingValue]
        {
            get
            {
                return this.Get(key, createCachingValue);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="createCachingValue">The create caching value.</param>
        /// <returns></returns>
        public TValue Get(TKey key, Func<TValue> createCachingValue)
        {
            TValue value = default(TValue);
            if (this.TryGetValue(key, out value))
            {
                return value;
            }

            lock (cache)
            {
                if (!cache.TryGetValue(key, out value))
                {
                    value = createCachingValue();
                    cache[key] = value;
                }
            }

            return value;
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, TValue value)
        {
            this.cache.Add(key, value);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(TKey key)
        {
            return this.cache.ContainsKey(key);
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        public ICollection<TKey> Keys
        {
            get { return this.cache.Keys; }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            return this.cache.Remove(key);
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.cache.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public ICollection<TValue> Values
        {
            get { return cache.Values; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <value>
        /// 
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                return this.cache[key];
            }
            set
            {
                this.cache[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.cache.Add(item);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.cache.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.cache.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.cache.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get { return this.cache.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return this.cache.IsReadOnly; }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.cache.Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.cache.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.cache.GetEnumerator();
        }

        #endregion
    }
}
