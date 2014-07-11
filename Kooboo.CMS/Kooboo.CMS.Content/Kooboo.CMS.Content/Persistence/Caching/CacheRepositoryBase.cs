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
using Kooboo.Common.Caching;
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
            try
            {
                //clear the cache before add to avoid get NullObject from cache.
                ClearObjectCache(item);
                innerProvider.Add(item);
            }
            finally
            {
                ClearObjectCache(item);
            }
        }
        #endregion

        #region Update
        public virtual void Update(T @new, T old)
        {
            try
            {
                innerProvider.Update(@new, old);
            }
            finally
            {
                ClearObjectCache(old);
            }
        }

        #endregion

        #region Remove
        public virtual void Remove(T item)
        {
            try
            {
                innerProvider.Remove(item);
            }
            finally
            {
                ClearObjectCache(item);
            }
        }
        #endregion

    }
}
