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
using Kooboo.CMS.Account.Persistence;
using System.Collections;

namespace Kooboo.CMS.Account.Persistence.SqlSever
{
    public class RepositoryFactory : IRepositoryFactory
    {
        static Hashtable providers = new Hashtable();
        static RepositoryFactory()
        {
            providers.Add(typeof(IUserProvider), new UserProvider());
            providers.Add(typeof(IRoleProvider), new RoleProvider());
        }
        public T GetRepository<T>() where T : class
        {
            foreach (var p in providers.Values)
            {
                if (p is T)
                {
                    return (T)p;
                }
            }
            return null;
        }
    }
}
