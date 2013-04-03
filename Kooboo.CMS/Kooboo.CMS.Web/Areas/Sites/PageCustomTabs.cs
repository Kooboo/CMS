using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Models;
using System.IO;

namespace Kooboo.CMS.Web.Areas.Sites
{
    public class TabInfo
    {
        public string Title { get; set; }
        public string VirualPath { get; set; }
    }
    public static class PageCustomTabs
    {
        static string PageTabsDir = "PageTabs";
        public static IEnumerable<TabInfo> Tabs(string layoutName)
        {
            var site = Site.Current;
            return SystemTabs().Concat(SiteTabs(site)).Concat(LayoutTabs(site, layoutName));
        }

        public static IEnumerable<TabInfo> SystemTabs()
        {
            string tabPath = Path.Combine(Kooboo.Settings.BaseDirectory, "Cms_Data", PageTabsDir);
            return GetTabs(tabPath);

        }
        private static IEnumerable<TabInfo> LayoutTabs(Site site, string layoutName)
        {
            var layout = new Layout(site, layoutName);
            string tabPath = Path.Combine(layout.PhysicalPath, PageTabsDir);
            return GetTabs(tabPath);
        }
        public static IEnumerable<TabInfo> SiteTabs(Site site)
        {
            var tabs = Enumerable.Empty<TabInfo>();
            string tabPath = String.Empty;
            var parentSite = site.Parent;
            while(parentSite != null)
            {
                tabPath = Path.Combine(parentSite.PhysicalPath, PageTabsDir);
                tabs = GetTabs(tabPath).Concat(tabs);
                parentSite = parentSite.Parent;
            }

            tabPath = Path.Combine(site.PhysicalPath, PageTabsDir);
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