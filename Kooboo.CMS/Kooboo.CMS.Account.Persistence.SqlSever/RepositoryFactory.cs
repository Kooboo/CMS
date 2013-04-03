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
            providers.Add(typeof(IUserRepository), new UserRepository());
            providers.Add(typeof(IRoleRepository), new RoleRepository());
            providers.Add(typeof(ISettingRepository), new Kooboo.CMS.Account.Persistence.FileSystem.SettingRepository());
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
