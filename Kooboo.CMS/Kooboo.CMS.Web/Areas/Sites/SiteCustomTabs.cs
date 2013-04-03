using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Models;
using System.IO;

namespace Kooboo.CMS.Web.Areas.Sites
{
    public static class SiteCustomTabs
    {
        static string SiteTabsDir = "SiteTabs";
        public static IEnumerable<TabInfo> Tabs()
        {
            var site = Site.Current;
            return SystemTabs().Concat(SiteTabs(site));
        }

        public static IEnumerable<TabInfo> SystemTabs()
        {
            string tabPath = Path.Combine(Kooboo.Settings.BaseDirectory, "Cms_Data", SiteTabsDir);
            return GetTabs(tabPath);

        }
       
        public static IEnumerable<TabInfo> SiteTabs(Site site)
        {
            var tabs = Enumerable.Empty<TabInfo>();
            string tabPath = String.Empty;
            var parentSite = site.Parent;
            while (parentSite != null)
            {
                tabPath = Path.Combine(parentSite.PhysicalPath, SiteTabsDir);
                tabs = GetTabs(tabPath).Concat(tabs);
                parentSite = parentSite.Parent;
            }

            tabPath = Path.Combine(site.PhysicalPath, SiteTabsDir);
            tabs = tabs.Concat(GetTabs(tabPath));
            return tabs;
        }
        private static IEnumerable<TabInfo> GetTabs(string tabPath)
        {
            if (Directory.Exists(tabPath))
            {
                foreach (var file in Directory.EnumerateFiles(tabPath, "*.cshtml"))
                {
                    var tabInfo = new TabInfo();
                    tabInfo.Title = Path.GetFileNameWithoutExtension(file);
                    tabInfo.VirualPath = file.Replace(Kooboo.Settings.BaseDirectory, "~/");
                    yield return tabInfo;
                }
            }
        }

    }
}