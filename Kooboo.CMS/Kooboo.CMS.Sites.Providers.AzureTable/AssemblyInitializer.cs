using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.CMS.Sites.Models;
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.CMS.Sites.Providers.AzureTable.AssemblyInitializer), "Initialize")]
namespace Kooboo.CMS.Sites.Providers.AzureTable
{
    public static class AssemblyInitializer
    {
        public static void Initialize()
        {
            ApplicationInitialization.RegisterInitializerMethod(delegate()
            {
                ResetProviders();
            }, 1);
        }

        public static void ResetProviders()
        {
            Kooboo.CMS.Sites.Globalization.DefaultRepositoryFactory.Instance = new LabelProvider.RepositoryFactory();

            var providerFactory = Kooboo.CMS.Sites.Persistence.Providers.ProviderFactory;
            providerFactory.RegisterProvider<IPageProvider>(new PageProvider.PageProvider());
            providerFactory.RegisterProvider<IHtmlBlockProvider>(new HtmlBlockProvider.HtmlBlockProvider());
            providerFactory.RegisterProvider<IUserProvider>(new UserProvider.UserProvider());

            //VersionManager.RegisterVersionLogger<Page>(new PageProvider.PageProvider.PageVersionLogger());
            //VersionManager.RegisterVersionLogger<HtmlBlock>(new HtmlBlockProvider.HtmlBlockProvider.HtmlBlockVersionLogger());


        }
    }
}
