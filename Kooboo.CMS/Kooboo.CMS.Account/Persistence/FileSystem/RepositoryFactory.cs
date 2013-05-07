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

namespace Kooboo.CMS.Account.Persistence.FileSystem
{
    [Obsolete("这个类在V4中不再使用，不删除的原因是避免外部如果继承它而出现不兼容的错误。")]
    public class RepositoryFactory : IRepositoryFactory
    {
        static Hashtable providers = new Hashtable();
        static RepositoryFactory()
        {
            providers.Add(typeof(IRoleProvider), new RoleRepository());
            providers.Add(typeof(IUserProvider), new UserRepository());
            providers.Add(typeof(ISettingProvider), new SettingRepository());
        }
        public T GetRepository<T>() where T : class
        {
            foreach (var item in providers.Values)
            {
                if (item is T)
                {
                    return (T)item;
                }
            }
            return null;
        }
    }
}
