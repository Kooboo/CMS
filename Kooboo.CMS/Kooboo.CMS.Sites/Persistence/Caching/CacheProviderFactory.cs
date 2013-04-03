using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Persistence;
using System.Collections;

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class CacheProviderFactory : IProviderFactory
    {
        public static System.Runtime.Caching.CacheItemPolicy DefaultCacheItemPolicy = new System.Runtime.Caching.CacheItemPolicy()
        {
            SlidingExpiration = TimeSpan.Parse("00:30:00")
        };
        private IProviderFactory innerRepositoryFactory;
        public CacheProviderFactory(IProviderFactory innerFactory)
        {
            innerRepositoryFactory = innerFactory;
        }

        public T GetRepository<T>()
        {
            var inner = innerRepositoryFactory.GetRepository<T>();

            if (inner is ISiteProvider)
            {
                return (T)(object)(new SiteProvider((ISiteProvider)inner));
            }
            if (inner is IPageProvider)
            {
                return (T)(object)(new PageProvider((IPageProvider)inner));
            }
            if (inner is ILayoutProvider)
            {
                return (T)(object)(new LayoutProvider((ILayoutProvider)inner));
            }
            if (inner is IViewProvider)
            {
                return (T)(object)(new ViewProvider((IViewProvider)inner));
            }
            if (inner is IUrlKeyMapProvider)
            {
                return (T)(object)(new UrlKeyMapProvider((IUrlKeyMapProvider)inner));
            }
            if (inner is IHtmlBlockProvider)
            {
                return (T)(object)(new HtmlBlockProvider((IHtmlBlockProvider)inner));
            }
            return inner;

        }


        public void RegisterProvider<ServiceType>(ServiceType provider)
        {
            innerRepositoryFactory.RegisterProvider<ServiceType>(provider);
        }
    }
}
