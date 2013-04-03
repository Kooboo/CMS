using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace Kooboo.Caching
{
    public class DefaultCache : ICache
    {
        public void Add(string key, object value, TimeSpan slidingExpiration, CacheRemovedCallback callback = null)
        {
            var policy = new CacheItemPolicy 
            { 
                SlidingExpiration = slidingExpiration
            };
            if (callback != null) 
            {
                policy.RemovedCallback = delegate (CacheEntryRemovedArguments args)
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

        public void Remove(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        public object Get(string key)
        {
            return MemoryCache.Default.Get(key);
        }
    }
}
