#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Models;
using System.IO;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Web.Areas.Sites
{
    public class TabInfo
    {
        public string Title { get; set; }
        public string VirualPath { get; set; }
    }
    /// <summary>
    /// Get the custom tabs.
    /// 1. System level，locate "Cms_Data\PageTabs"，all the pages in all site will have these tabs.
    /// 2. Site level，locate "Cms_Data\SiteName\PageTabs"， all the pages in the site will have these tabs.
    /// 3. Layout level，locate "{LayoutPage}\PageTabs"， the pages use this layout will have these tabs.
    /// 4. Module level, locate "{ModulePath}\PageTabs", the pages in the site which include this module will have these tabs.
    /// </summary>
    public static class PageCustomTabs
    {
        static string PageTabsDir = "PageTabs";
        public static IEnumerable<TabInfo> Tabs(string layoutName)
        {
            var site = Site.Current;
            return SystemTabs().Concat(ModuleTabs(site)).Concat(SiteTabs(site)).Concat(LayoutTabs(site, layoutName));
        }

        public static IEnumerable<TabInfo> SystemTabs()
        {
            var baseDir = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IBaseDir>();
            string tabPath = Path.Combine(baseDir.Cms_DataPhysicalPath, PageTabsDir);
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
            while (parentSite != null)
            {
                tabPath = Path.Combine(parentSite.PhysicalPath, PageTabsDir);
                tabs = GetTabs(tabPath).Concat(tabs);
                parentSite = parentSite.Parent;
            }

            tabPath = Path.Combine(site.PhysicalPath, PageTabsDir);
            tabs = tabs.Concat(GetTabs(tabPath));
            return tabs;
        }

        public static IEnumerable<TabInfo> ModuleTabs(Site site)
        {

            var tabs = Enumerable.Empty<TabInfo>();

            var modules = ServiceFactory.ModuleManager.AllModulesForSite(site.FullName);

            foreach (var moduleName in modules)
            {

                string tabPath = Path.Combine(Kooboo.Settings.BaseDirectory, "Areas", moduleName, PageTabsDir);

                tabs = GetTabs(tabPath).Concat(tabs);

            }

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