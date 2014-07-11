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
using System.Web;
using System.Web.Mvc;
using Kooboo.Common.Caching;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    public class CacheController : Controller
    {
        public virtual ActionResult Clear(string objectCacheName, string cacheKey, string source)
        {
            if (source != AppDomain.CurrentDomain.FriendlyName)
            {
                if (!string.IsNullOrEmpty(cacheKey))
                {
                    var objectCache = CacheManagerFactory.DefaultCacheManager.GetObjectCache(objectCacheName);
                    if (objectCache != null)
                    {
                        objectCache.SetExpired(cacheKey);
                        //objectCache[cacheKey] = null;
                    }
                }
                else
                {
                    CacheManagerFactory.DefaultCacheManager.Clear(objectCacheName);
                }
            }
            return null;
        }

    }
}
