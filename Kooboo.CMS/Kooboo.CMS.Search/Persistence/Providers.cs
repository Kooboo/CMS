using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Search.Persistence
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
                defaultProviderFactory = value;

                if (EnableCaching)
                {
                    defaultProviderFactory = new Caching.ProviderFactory(value);
                }
                else
                {
                    defaultProviderFactory = value;
                }
            }
        }

        public static ISearchSettingProvider SearchSettingProvider
        {
            get
            {
                return DefaultProviderFactory.GetProvider<ISearchSettingProvider>();
            }
        }
        public static ILastActionProvider LastActionProvider
        {
            get
            {
                return DefaultProviderFactory.GetProvider<ILastActionProvider>();
            }
        }
    }
}
