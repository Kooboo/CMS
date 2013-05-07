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
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Caching;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System.Runtime.Caching;

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public abstract class ProviderBase<T> : IProvider<T>
        where T : class,  IPersistable, IIdentifiable
    {
        #region .ctor
        private IProvider<T> innerProvider;
        public ProviderBase(IProvider<T> inner)
        {
            this.innerProvider = inner;
        }
        #endregion

        #region Get
        public virtual T Get(T dummy)
        {
            if (dummy == null)
            {
                return dummy;
            }
            var cacheKey = GetItemCacheKey(dummy);

            var cached = GetObjectCache(GetSite(dummy)).GetCache(cacheKey, () => innerProvider.Get(dummy));

            return cached;
        }
        #endregion

        #region GetSite
        protected virtual Site GetSite(T item)
        {
            if (item is ISiteObject)
            {
                return (((ISiteObject)item).Site);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region GetObjectCache
        protected virtual ObjectCache GetObjectCache(Site site)
        {
            if (site == null)
            {
                return CacheManagerFactory.DefaultCacheManager.GlobalObjectCache();
            }
            else
            {
                return site.ObjectCache();
            }
        }
        #endregion

        #region GetListCacheKey
        protected virtual string GetListCacheKey()
        {
            //do not cache the list;
            return null;
        }
        #endregion

        #region GetItemCacheKey
        protected abstract string GetItemCacheKey(T o);
        #endregion

        #region ClearObjectCache
        protected virtual void ClearObjectCache(Site site)
        {
            //为保证页面缓存的及时更新，一旦任何数据修改都清空整站缓存。
            if (site == null)
            {
                CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
            }
            else
            {
                site.ClearCache();
            }
        }
        #endregion

        #region Add
        public virtual void Add(T item)
        {
            ClearObjectCache(GetSite(item));

            innerProvider.Add(item);

        }
        #endregion

        #region Update
        public virtual void Update(T @new, T old)
        {
            ClearObjectCache(GetSite(old));

            innerProvider.Update(@new, old);
        }
        #endregion

        #region Remove
        public virtual void Remove(T item)
        {
            ClearObjectCache(GetSite(item));
            innerProvider.Remove(item);
        }
        #endregion

        #region All
        public virtual IEnumerable<T> All()
        {
            var cacheKey = GetListCacheKey();
            if (!string.IsNullOrEmpty(cacheKey))
            {
                return GetObjectCache(null).GetCache(cacheKey, () => innerProvider.All().ToArray());
            }
            else
            {
                return innerProvider.All();
            }
        }
        #endregion
    }
}
