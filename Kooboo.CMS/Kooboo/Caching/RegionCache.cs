#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;

namespace Kooboo.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public class RegionCache : ICache
    {
        #region .ctor
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
        #endregion

        #region Properties
        /// <summary>
        /// Gets the name of the region.
        /// </summary>
        /// <value>
        /// The name of the region.
        /// </value>
        public string RegionName { get; private set; }

        /// <summary>
        /// Gets the parent cache.
        /// </summary>
        /// <value>
        /// The parent cache.
        /// </value>
        public Func<ICache> ParentCache { get; private set; }

        #endregion

        #region Methods
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="callback">The callback.</param>
        public void Add(string key, object value, TimeSpan slidingExpiration, CacheRemovedCallback callback)
        {
            ParentCache().Add(RegionKey(key), value, slidingExpiration, callback);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="callback">The callback.</param>
        public void Add(string key, object value, DateTime absoluteExpiration, CacheRemovedCallback callback)
        {
            ParentCache().Add(RegionKey(key), value, absoluteExpiration, callback);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            ParentCache().Remove(RegionKey(key));
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object Get(string key)
        {
            return ParentCache().Get(RegionKey(key));
        }

        /// <summary>
        /// Regions the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string RegionKey(string key)
        {
            return RegionName + "_" + key;
        }
        #endregion
    }
}
