using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kooboo.Collections
{
    public class CachableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private IDictionary<TKey, TValue> cache;

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

        public TValue this[TKey key, Func<TValue> createCachingValue]
        {
            get
            {
                return this.Get(key, createCachingValue);
            }
        }

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


        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            this.cache.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return this.cache.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return this.cache.Keys; }
        }

        public bool Remove(TKey key)
        {
            return this.cache.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.cache.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return cache.Values; }
        }

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

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.cache.Add(item);
        }

        public void Clear()
        {
            this.cache.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.cache.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.cache.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.cache.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.cache.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.cache.Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.cache.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.cache.GetEnumerator();
        }

        #endregion
    }
}
