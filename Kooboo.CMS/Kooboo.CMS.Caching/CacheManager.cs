#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
#region usings
using System.Runtime.Caching;
#endregion

namespace Kooboo.CMS.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CacheManager
    {
        #region Const fields
        /// <summary>
        /// 
        /// </summary>
        const string GLOBAL_CACHE_NAME = "___GlobalCache___";
        #endregion

        #region Methods
        /// <summary>
        /// Gets the object cache.
        /// </summary>
        /// <param name="cacheName">Name of the cache.</param>
        /// <returns></returns>
        public abstract ObjectCache GetObjectCache(string cacheName);

        /// <summary>
        /// Removes the object cache.
        /// </summary>
        /// <param name="cacheName">Name of the cache.</param>
        protected abstract void RemoveObjectCache(string cacheName);

        /// <summary>
        /// Clears the specified cache name.
        /// </summary>
        /// <param name="cacheName">Name of the cache.</param>
        public virtual void Clear(string cacheName)
        {
            RemoveObjectCache(cacheName);
        }

        /// <summary>
        /// Globals the object cache.
        /// </summary>
        /// <returns></returns>
        public virtual ObjectCache GlobalObjectCache()
        {
            return GetObjectCache(GLOBAL_CACHE_NAME);
        }
        /// <summary>
        /// Clears the global object cache.
        /// </summary>
        public virtual void ClearGlobalObjectCache()
        {
            CacheManagerFactory.ClearWithNotify(GLOBAL_CACHE_NAME);
        }
        #endregion
    }
}
