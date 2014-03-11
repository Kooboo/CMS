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
using Kooboo.CMS.Caching;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public abstract class SiteElementProviderBase<T> : ProviderBase<T>, ISiteElementProvider<T>
       where T : class, IPersistable, IIdentifiable
    {
        #region .ctor
        private ISiteElementProvider<T> innerProvider;
        public SiteElementProviderBase(ISiteElementProvider<T> inner)
            : base(inner)
        {
            this.innerProvider = inner;
        }
        #endregion

        #region All
        public IEnumerable<T> All(Models.Site site)
        {
            var cacheKey = GetListCacheKey();
            if (!string.IsNullOrEmpty(cacheKey))
            {
                return GetObjectCache(site).GetCache(cacheKey, () => innerProvider.All(site).ToArray());
            }
            else
            {
                return innerProvider.All(site);
            }

        }
        #endregion


        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public void InitializeToDB(Site site)
        {
            try
            {
                innerProvider.InitializeToDB(site);
            }
            finally
            {
                ClearObjectCache(site);
            }
        }

        public void ExportToDisk(Site site)
        {
            innerProvider.ExportToDisk(site);
        }
        #endregion
    }
}
