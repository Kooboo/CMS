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
        protected IProvider<T> innerProvider;
        public CacheProviderBase(IProvider<T> inner)
        {
            this.innerProvider = inner;
        }
        public virtual T Get(T dummy)
        {
            var cacheKey = GetCacheKey(dummy);

            var cached = dummy.Repository.ObjectCache().GetCache(cacheKey, () => innerProvider.Get(dummy));

            return cached;
        }

        protected abstract string GetCacheKey(T o);

        protected virtual void ClearObjectCache(T o)
        {
            o.Repository.ClearCache();
        }

        public virtual void Add(T item)
        {
            ClearObjectCache(item);
            innerProvider.Add(item);
        }

        public virtual void Update(T @new, T old)
        {
            ClearObjectCache(old);

            innerProvider.Update(@new, old);
        }

        public virtual void Remove(T item)
        {
            innerProvider.Remove(item);
            ClearObjectCache(item);
        }

    }
}
