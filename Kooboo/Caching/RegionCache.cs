using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Caching
{
    public class RegionCache : ICache
    {
        public RegionCache(string regionName)
            : this(() => CacheProvider.Cache, regionName)
        {
            RegionName = regionName;
        }

        public RegionCache(Func<ICache> parentCache, string regionName)
        {
            ParentCache = parentCache;
            RegionName = regionName;
        }

        public string RegionName { get; private set; }

        public Func<ICache> ParentCache { get; private set; }

        public void Add(string key, object value, TimeSpan slidingExpiration, CacheRemovedCallback callback)
        {
            ParentCache().Add(RegionKey(key), value, slidingExpiration, callback);
        }

        public void Add(string key, object value, DateTime absoluteExpiration, CacheRemovedCallback callback)
        {
            ParentCache().Add(RegionKey(key), value, absoluteExpiration, callback);
        }

        public void Remove(string key)
        {
            ParentCache().Remove(RegionKey(key));
        }

        public object Get(string key)
        {
            return ParentCache().Get(RegionKey(key));
        }

        private string RegionKey(string key)
        {
            return RegionName + "_" + key;
        }
    }
}
