using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Caching;
namespace Kooboo.CMS.Account.Persistence.Caching
{
    public abstract class CacheObjectProviderBase<T>
        where T : class,IPersistable
    {
        private IRepository<T> innerRepository;
        public CacheObjectProviderBase(IRepository<T> inner)
        {
            this.innerRepository = inner;
        }

        public virtual T Get(T dummy)
        {
            var cacheKey = GetCacheKey(dummy);

            var cached = CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().GetCache(cacheKey, () => innerRepository.Get(dummy));

            return cached;
        }

        protected abstract string GetCacheKey(T o);

        protected virtual void ClearObjectCache(T o)
        {
            var cacheKey = GetCacheKey(o);
            CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().Remove(cacheKey);
        }

        public virtual void Add(T item)
        {
            innerRepository.Add(item);
            ClearObjectCache(item);
        }



        public virtual void Update(T @new, T old)
        {
            ClearObjectCache(@old);

            innerRepository.Update(@new, old);
        }

        public virtual void Remove(T item)
        {
            ClearObjectCache(item);
            innerRepository.Remove(item);
        }

    }
}
