using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.CMS.Caching.NotifyRemote.AssemblyInitializer), "Initialize")]
namespace Kooboo.CMS.Caching.NotifyRemote
{
    public static class AssemblyInitializer
    {
        public static void Initialize()
        {
            ApplicationInitialization.RegisterInitializerMethod(delegate()
            {
                Kooboo.CMS.Caching.CacheExpiredNotification.Notifiactions.Add(new NotifyRemoteServer());
            }, 0);
        }
    }
}
