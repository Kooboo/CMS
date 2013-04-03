using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Caching
{
    public interface ICache
    {
        void Add(string key, object value, TimeSpan slidingExpiration, CacheRemovedCallback callback = null);

        void Add(string key, object value, DateTime absoluteExpiration, CacheRemovedCallback callback = null);

        void Remove(string key);

        object Get(string key);
    }

    public static class CacheExtensions
    {
        public static T GetOrAdd<T>(this ICache cache, string key, TimeSpan slidingExpiration, Func<T> func, CacheRemovedCallback callback = null)
        {
            var obj = Get<T>(cache, key);
            if (obj == null)
            {
                obj = func();
                if (obj == null)
                    return default(T);

                cache.Add(key, obj, slidingExpiration, callback);
            }

            return obj;
        }

        public static T GetOrAdd<T>(this ICache cache, string key, DateTime absoluteExpiration, Func<T> func, CacheRemovedCallback callback = null)
        {
            var obj = Get<T>(cache, key);
            if (obj == null)
            {
                obj = func();
                if (obj == null)
                    return default(T);

                cache.Add(key, obj, absoluteExpiration, callback);
            }

            return obj;
        }

        public static T Get<T>(this ICache cache, string key)
        {
            var obj = cache.Get(key);
            return (T)obj;
        }
    }

    public delegate void CacheRemovedCallback(CacheRemovedCallbackArgs args);

    public class CacheRemovedCallbackArgs
    {
        public string Key { get; set; }

        public object Value { get; set; }
    }
}
