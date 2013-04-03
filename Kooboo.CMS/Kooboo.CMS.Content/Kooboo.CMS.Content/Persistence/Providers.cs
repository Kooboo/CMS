using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence
{
    public static class Providers
    {
        static Providers()
        {
            EnableCaching = true;
            DefaultProviderFactory = new Default.ProviderFactory();
        }
        public static bool EnableCaching { get; set; }

        private static IProviderFactory defaultProviderFactory;
        public static IProviderFactory DefaultProviderFactory
        {
            get
            {
                return defaultProviderFactory;
            }
            set
            {
                if (EnableCaching)
                {
                    defaultProviderFactory = new Caching.CacheProviderFactory(value);
                }
                else
                {
                    defaultProviderFactory = value;
                }
            }
        }

        public static IRepositoryProvider RepositoryProvider
        {
            get
            {
                return DefaultProviderFactory.GetProvider<IRepositoryProvider>();
            }
        }
    }
}
