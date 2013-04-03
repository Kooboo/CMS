using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace Kooboo.CMS.Caching
{
    public class MemoryCacheManager : CacheManager
    {
        static IDictionary<string, MemoryCache> objectCaches = new Dictionary<string, MemoryCache>();

        public override ObjectCache GetObjectCache(string name)
        {
            if (!objectCaches.ContainsKey(name))
            {
                lock (objectCaches)
                {
                    if (!objectCaches.ContainsKey(name))
                    {
                        MemoryCache memoryCache = new MemoryCache(name);
                        objectCaches.Add(name, memoryCache);
                    }
                }
            }
            return objectCaches[name];
        }

        protected override void RemoveObjectCache(string name)
        {
            if (objectCaches.ContainsKey(name))
            {
                lock (objectCaches)
                {
                    if (objectCaches.ContainsKey(name))
                    {

                        MemoryCache memoryCache = objectCaches[name];
                        objectCaches.Remove(name);

                        memoryCache.Dispose();
                    }
                }
            }

        }
    }
}
