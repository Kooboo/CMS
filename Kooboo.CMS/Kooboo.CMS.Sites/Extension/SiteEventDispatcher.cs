using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using System.Web;
using Kooboo.CMS.Sites.Services;

namespace Kooboo.CMS.Sites.Extension
{
    public static class SiteEventDispatcher
    {
        static IDictionary<string, IEnumerable<ISiteEvents>> siteEvents = new Dictionary<string, IEnumerable<ISiteEvents>>(StringComparer.CurrentCultureIgnoreCase);
        public static void OnPreSiteRequestExecute(Site site, HttpContextBase httpContext)
        {
            ServiceFactory.AssemblyManager.EnsureAssembliesExistsInBin(site);
            var observers = GetObservers(site);

            foreach (var item in observers)
            {
                item.OnPreSiteRequestExecute(site, httpContext);
            }
        }
        private static IEnumerable<ISiteEvents> GetObservers(Site site)
        {
            if (!siteEvents.ContainsKey(site.Name))
            {
                lock (siteEvents)
                {
                    if (!siteEvents.ContainsKey(site.Name))
                    {
                        var observers = ServiceFactory.AssemblyManager.GetTypeInstances(site, typeof(ISiteEvents)).OfType<ISiteEvents>().ToArray();
                        foreach (var item in observers)
                        {
                            item.OnSiteStart(site);
                        }
                        siteEvents[site.Name] = observers;
                    }
                }
            }
            return siteEvents[site.Name];
        }
        public static void OnPostSiteRequestExecute(Site site, HttpContextBase httpContext)
        {
            var observers = GetObservers(site);
            foreach (var item in observers)
            {
                item.OnPostSiteRequestExecute(site, httpContext);
            }
        }
    }
}
