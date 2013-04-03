using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Kooboo.CMS.Account.Persistence.Caching
{
    public class CacheProviderFactory : IRepositoryFactory
    {
        public static System.Runtime.Caching.CacheItemPolicy DefaultCacheItemPolicy = new System.Runtime.Caching.CacheItemPolicy()
        {
            SlidingExpiration = TimeSpan.Parse("00:30:00")
        };
        private IRepositoryFactory innerRepositoryFactory;
        public CacheProviderFactory(IRepositoryFactory innerFactory)
        {
            innerRepositoryFactory = innerFactory;
        }

        public T GetRepository<T>()
             where T : class
        {
            var inner = innerRepositoryFactory.GetRepository<T>();

            if (inner is IUserRepository)
            {
                return (T)(object)(new UserRepository((IUserRepository)inner));
            }
      
            return inner;

        }
    }
}
