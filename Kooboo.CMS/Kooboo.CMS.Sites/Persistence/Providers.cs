using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence
{
    public static class Providers
    {
        static Providers()
        {
            EnableCaching = true;
            ProviderFactory = new FileSystem.ProviderFactory();
        }
        public static bool EnableCaching { get; set; }
        private static IProviderFactory providerFactory;
        public static IProviderFactory ProviderFactory
        {
            get
            {
                return providerFactory;
            }
            set
            {
                if (EnableCaching)
                {
                    providerFactory = new Caching.CacheProviderFactory(value);
                }
                else
                    providerFactory = value;
            }
        }

        public static ISiteProvider SiteProvider
        {
            get
            {
                return ProviderFactory.GetRepository<ISiteProvider>();
            }
        }

        public static IPageProvider PageProvider
        {
            get
            {
                return ProviderFactory.GetRepository<IPageProvider>();
            }
        }

        public static ILayoutProvider LayoutProvider
        {
            get
            {
                return ProviderFactory.GetRepository<ILayoutProvider>();
            }
        }

        public static IViewProvider ViewProvider
        {
            get
            {
                return ProviderFactory.GetRepository<IViewProvider>();
            }
        }

        public static IScriptProvider ScriptsProvider
        {
            get
            {
                return ProviderFactory.GetRepository<IScriptProvider>();
            }
        }

        public static IThemeProvider ThemeProvider
        {
            get
            {
                return ProviderFactory.GetRepository<IThemeProvider>();
            }
        }

        public static ICustomFileProvider ImagesProvider
        {
            get
            {
                return ProviderFactory.GetRepository<ICustomFileProvider>();
            }
        }

        public static ICustomErrorProvider CustomErrorProvider
        {
            get
            {
                return ProviderFactory.GetRepository<ICustomErrorProvider>();
            }
        }

        public static IUrlRedirectProvider UrlRedirectProvider
        {
            get
            {
                return ProviderFactory.GetRepository<IUrlRedirectProvider>();
            }
        }
        public static IUrlKeyMapProvider UrlKeyMapProvider
        {
            get
            {
                return ProviderFactory.GetRepository<IUrlKeyMapProvider>();
            }
        }
        public static IPagePublishingQueueProvider PagePublishingProvider
        {
            get
            {
                return ProviderFactory.GetRepository<IPagePublishingQueueProvider>();
            }
        }
        public static IHtmlBlockProvider HtmlBlockProvider
        {
            get
            {
                return providerFactory.GetRepository<IHtmlBlockProvider>();
            }
        }
    }
}
