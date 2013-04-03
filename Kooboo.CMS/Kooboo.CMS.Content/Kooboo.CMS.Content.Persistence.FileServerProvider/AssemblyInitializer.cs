using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.CMS.Content.Persistence.FileServerProvider.AssemblyInitializer), "Initialize")]
namespace Kooboo.CMS.Content.Persistence.FileServerProvider
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
            var providerFactory = Providers.DefaultProviderFactory;            
            providerFactory.RegisterProvider<IMediaFolderProvider>(new MediaFolderProvider());
            providerFactory.RegisterProvider<IMediaContentProvider>(new MediaContentProvider());
            providerFactory.RegisterProvider<ITextContentFileProvider>(new TextContentFileProvider());
        }
    }
}
