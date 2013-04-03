using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Caching;

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public abstract class CacheObjectProviderBase<T>
        where T : class,IPersistable
    {
        private IProvider<T> innerRepository;
        public CacheObjectProviderBase(IProvider<T> inner)
        {
            this.innerRepository = inner;
        }
        public virtual T Get(T dummy)
        {
            if (dummy == null)
            {
                return dummy;
            }
            var cacheKey = GetCacheKey(dummy);
            T o = default(T);
            var cached = dummy.Site.ObjectCache().GetCache(cacheKey, () => innerRepository.Get(dummy));

            return cached;
        }

        protected abstract string GetCacheKey(T o);

        protected virtual void ClearObjectCache(T o)
        {
            // var cacheKey = GetCacheKey(o);
            //o.Site.ObjectCache().Remove(cacheKey);
            //为保证页面缓存的及时更新，一旦任何数据修改都清空整站缓存。
            o.Site.ClearCache();
        }

        public virtual void Add(T item)
        {
            ClearObjectCache(item);

            innerRepository.Add(item);

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
