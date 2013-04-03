using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.CMS.Content.Persistence.Sqlce.AssemblyInitializer), "Initialize")]
namespace Kooboo.CMS.Content.Persistence.Sqlce
{
    public static class AssemblyInitializer
    {
        public static void Initialize()
        {
            ApplicationInitialization.RegisterInitializerMethod(delegate()
            {
                Kooboo.CMS.Content.Persistence.Providers.DefaultProviderFactory = new Sqlce.ProviderFactory();
            }, 0);
        }
    }
}
