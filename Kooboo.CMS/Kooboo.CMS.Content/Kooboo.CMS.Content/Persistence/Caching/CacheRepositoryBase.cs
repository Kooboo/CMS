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
using Kooboo.CMS.Content.Caching;
using Kooboo.CMS.Caching;
namespace Kooboo.CMS.Content.Persistence.Caching
{
    public abstract class CacheProviderBase<T>
        where T : class, IRepositoryElement
    {
        #region .ctor
        protected IContentElementProvider<T> innerProvider;
        public CacheProviderBase(IContentElementProvider<T> inner)
        {
            this.innerProvider = inner;
        }
        #endregion

        #region Get
        public virtual T Get(T dummy)
        {
            var cacheKey = GetCacheKey(dummy);

            var cached = dummy.Repository.ObjectCache().GetCache(cacheKey, () => innerProvider.Get(dummy));

            return cached;
        }
        #endregion

        #region GetCacheKey
        protected abstract string GetCacheKey(T o);
        #endregion

        #region ClearObjectCache
        protected virtual void ClearObjectCache(T o)
        {
            o.Repository.ClearCache();
        }
        #endregion

        #region Add
        public virtual void Add(T item)
        {
            ClearObjectCache(item);
            innerProvider.Add(item);
        }
        #endregion

        #region Update
        public virtual void Update(T @new, T old)
        {
            ClearObjectCache(old);

            innerProvider.Update(@new, old);
        }

        #endregion

        #region Remove
        public virtual void Remove(T item)
        {
            innerProvider.Remove(item);
            ClearObjectCache(item);
        }
        #endregion

    }
}
