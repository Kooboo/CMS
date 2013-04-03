using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.WindowsAzure.ServiceRuntime;
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.CMS.Caching.AzureSync.AssemblyInitializer), "Initialize")]
namespace Kooboo.CMS.Caching.AzureSync
{
    public static class AssemblyInitializer
    {
        public static void Initialize()
        {
            ApplicationInitialization.RegisterInitializerMethod(delegate()
            {
                Kooboo.CMS.Caching.CacheExpiredNotification.Notifiactions.Add(new AzureInstancesCachingNotification());
            }, 0);
        }

        public static void WriteLine(string message)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "instances.txt");
            File.AppendAllLines(path, new[] { message });
        }
    }
}
