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

            if (inner is IUserProvider)
            {
                return (T)(object)(new UserProvider((IUserProvider)inner));
            }
      
            return inner;

        }
    }
}
