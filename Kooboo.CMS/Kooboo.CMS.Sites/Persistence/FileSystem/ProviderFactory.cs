using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public class ProviderFactory : IProviderFactory
    {
        static Hashtable providers = new Hashtable();
        static ProviderFactory()
        {
            providers.Add(typeof(ISiteProvider), new SiteProvider());
            providers.Add(typeof(IPageProvider), new PageProvider());
            providers.Add(typeof(ILayoutProvider), new LayoutProvider());
            providers.Add(typeof(IViewProvider), new ViewProvider());
            providers.Add(typeof(IScriptProvider), new ScriptProvider());
            providers.Add(typeof(IThemeProvider), new ThemeProvider());
            providers.Add(typeof(ICustomFileProvider), new CustomFileProvider());
            providers.Add(typeof(ICustomErrorProvider), new CustomErrorProvider());
            providers.Add(typeof(IUrlRedirectProvider), new UrlRedirectProvider());
            providers.Add(typeof(IUrlKeyMapProvider), new UrlKeyMapProvider());
            providers.Add(typeof(IUserProvider), new UserProvider());
            providers.Add(typeof(IPagePublishingQueueProvider), new PagePublishingQueueProvider());
            providers.Add(typeof(IHtmlBlockProvider), new HtmlBlockProvider());
        }

        #region IRepositoryFactory Members


        public T GetRepository<T>()
        {
            foreach (var item in providers.Values)
            {
                if (item is T)
                {
                    return (T)item;
                }
            }
            return default(T);
        }

        #endregion


        public void RegisterProvider<ServiceType>(ServiceType provider)
        {
            providers[typeof(ServiceType)] = provider;
        }
    }
}
