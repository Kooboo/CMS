#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
#region usings

using System;
using System.Collections.Generic;

#endregion

namespace Kooboo.CMS.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public static class CacheExpiredNotification
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public static List<INotifyCacheExpired> Notifiactions = new List<INotifyCacheExpired>();
        #endregion

        #region Methods
        /// <summary>
        /// Notifies the specified object cache name.
        /// </summary>
        /// <param name="objectCacheName">Name of the object cache.</param>
        /// <param name="cacheKey">The cache key.</param>
        public static void Notify(string objectCacheName, string cacheKey)
        {
            if (Notifiactions != null)
            {
                try
                {
                    foreach (var item in Notifiactions)
                    {
                        item.Notify(objectCacheName, cacheKey);
                    }
                }
                catch (Exception e)
                {
                    Kooboo.HealthMonitoring.Log.LogException(e);
                }

            }
        }
        #endregion
    }
}
