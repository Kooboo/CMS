using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Caching
{
    public static class CacheManagerFactory
    {
        private static CacheManager cacheManager;
        static CacheManagerFactory()
        {
            DefaultCacheManager = new MemoryCacheManager();
        }
        public static CacheManager DefaultCacheManager
        {
            get
            {
                return cacheManager;
            }
            set
            {
                cacheManager = value;
            }
        }

        public static void ClearWithNotify(string cacheName)
        {
            DefaultCacheManager.Clear(cacheName);
            CacheExpiredNotification.Notify(cacheName, null);
        }
    }
}
