using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Account.Persistence
{
    public interface IRepositoryFactory
    {
        T GetRepository<T>()
            where T : class;
    }
    public static class RepositoryFactory
    {
        public static bool EnableCaching { get; set; }

        static RepositoryFactory()
        {
            EnableCaching = true;
            Factory = new FileSystem.RepositoryFactory();
        }
        static IRepositoryFactory factory;
        public static IRepositoryFactory Factory
        {
            get
            {
                return factory;
            }
            set
            {
                if (EnableCaching)
                {
                    factory = new Caching.CacheProviderFactory(value);
                }
                else
                    factory = value;
            }
        }

        public static IRoleRepository RoleRepository
        {
            get
            {
                return Factory.GetRepository<IRoleRepository>();
            }
        }
        public static IUserRepository UserRepository
        {
            get
            {
                return Factory.GetRepository<IUserRepository>();
            }
        }
        public static ISettingRepository SettingRepository
        {
            get
            {
                return Factory.GetRepository<ISettingRepository>();
            }
        }
    }
}
