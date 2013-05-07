#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

namespace Kooboo.CMS.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public static class CacheManagerFactory
    {
        #region fields
        private static CacheManager cacheManager;
        #endregion

        #region properties

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

        #endregion

        #region .ctor
        static CacheManagerFactory()
        {
            DefaultCacheManager = new MemoryCacheManager();
        }
        #endregion        

        #region Methods
        public static void ClearWithNotify(string cacheName)
        {
            DefaultCacheManager.Clear(cacheName);
            CacheExpiredNotification.Notify(cacheName, null);
        } 
        #endregion
    }
}
