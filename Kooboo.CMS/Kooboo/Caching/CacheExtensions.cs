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
using System.Threading.Tasks;

namespace Kooboo.Caching
{
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
}
