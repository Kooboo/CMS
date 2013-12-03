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
    /// <summary>
    /// get the custom tabs for the site settings.
    /// 1. System level，locate "Cms_Data\SiteTabs"，all the site  will have these tabs.
    /// 2. Site level，locate "Cms_Data\SiteName\SiteTabs"，private tabs for the site.
    /// 3. Module level, locate "{ModulePath}\SiteTabs", the sites which include the moule will have these tabs.
    /// </summary>
    public static class SiteCustomTabs
    {
        static string SiteTabsDir = "SiteTabs";
        public static IEnumerable<TabInfo> Tabs()
        {
            var site = Site.Current;
            return SystemTabs().Concat(ModuleTabs(site)).Concat(SiteTabs(site));
        }

        public static IEnumerable<TabInfo> SystemTabs()
        {
            var baseDir = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IBaseDir>();
            string tabPath = Path.Combine(baseDir.Cms_DataPhysicalPath, SiteTabsDir);
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


        public static IEnumerable<TabInfo> ModuleTabs(Site site)
        {

            var tabs = Enumerable.Empty<TabInfo>();

            var modules = ServiceFactory.ModuleManager.AllModulesForSite(site.FullName);

            foreach (var moduleName in modules)
            {

                string tabPath = Path.Combine(Kooboo.Settings.BaseDirectory, "Areas", moduleName, SiteTabsDir);

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