using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Caching
{
    public class CacheProvider
    {
        public static TimeSpan DefaultSlidingExpiration = TimeSpan.FromMinutes(1);

        static CacheProvider()
        {
            Cache = new DefaultCache();
        }

        public static ICache Cache { get; set; }

        private static ICache _repositoryCache = new RegionCache("Repository");
        public static ICache RepositoryCache
        {
            get
            {
                return _repositoryCache;
            }
        }

        private static ICache _serviceCache = new RegionCache("Service");
        public static ICache ServiceCache
        {
            get
            {
                return _serviceCache;
            }
        }
    }
}
