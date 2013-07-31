#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Runtime.Caching;

namespace Kooboo.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultCache : ICache
    {
        #region Methods
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="callback">The callback.</param>
        public void Add(string key, object value, TimeSpan slidingExpiration, CacheRemovedCallback callback = null)
        {
            var policy = new CacheItemPolicy
            {
                SlidingExpiration = slidingExpiration
            };
            if (callback != null)
            {
                policy.RemovedCallback = delegate(CacheEntryRemovedArguments args)
                {
                    callback(new CacheRemovedCallbackArgs
                    {
                        Key = args.CacheItem.Key,
                        Value = args.CacheItem.Value
                    });
                };
            }
            MemoryCache.Default.Add(key, value, policy);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="callback">The callback.</param>
        public void Add(string key, object value, DateTime absoluteExpiration, CacheRemovedCallback callback = null)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = absoluteExpiration
            };
            if (callback != null)
            {
                policy.RemovedCallback = delegate(CacheEntryRemovedArguments args)
                {
                    callback(new CacheRemovedCallbackArgs
                    {
                        Key = args.CacheItem.Key,
                        Value = args.CacheItem.Value
                    });
                };
            }
            MemoryCache.Default.Add(key, value, policy);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object Get(string key)
        {
            return MemoryCache.Default.Get(key);
        } 
        #endregion
    }
}
