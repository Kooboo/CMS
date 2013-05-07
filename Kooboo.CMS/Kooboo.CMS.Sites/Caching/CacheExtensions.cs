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
using Kooboo.CMS.Sites.Models;
using System.Runtime.Caching;
using Kooboo.CMS.Caching;

namespace Kooboo.CMS.Sites.Caching
{
    public static class CacheExtensions
    {
        public static ObjectCache ObjectCache(this Site site)
        {
            return CacheManagerFactory.DefaultCacheManager.GetObjectCache(site.GetKey());
        }
        private static string GetKey(this Site site)
        {
            return "Site:" + site.FullName.ToLower();
        }
        public static void ClearCache(this Site site)
        {
            CacheManagerFactory.ClearWithNotify(site.GetKey());
            CacheManagerFactory.DefaultCacheManager.Clear(site.GetKey());
            foreach (var sub in Persistence.Providers.SiteProvider.ChildSites(site))
            {
                ClearCache(sub);
            }
        }
    }
}
