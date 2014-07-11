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
using Kooboo.CMS.Content.Models;
using System.Runtime.Caching;
using Kooboo.Common.Caching;


namespace Kooboo.CMS.Content.Caching
{
    public static class CacheExtensions
    {
        public static ObjectCache ObjectCache(this Repository repository)
        {
            return CacheManagerFactory.DefaultCacheManager.GetObjectCache(repository.GetKey());
        }
        private static string GetKey(this Repository repository)
        {
            return "Repository:CacheObject:" + repository.Name;
        }
        public static void ClearCache(this Repository repository)
        {
            CacheManagerFactory.ClearWithNotify(repository.GetKey());
        }
    }
}
