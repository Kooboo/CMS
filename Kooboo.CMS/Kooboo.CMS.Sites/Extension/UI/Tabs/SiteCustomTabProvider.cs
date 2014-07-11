#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;

using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Routing;
using Kooboo.Common;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Sites.Extension.UI.Tabs
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ITabProvider), Key = "SiteCustomTabProvider")]
    public class SiteCustomTabProvider : ITabProvider
    {
        public MvcRoute[] ApplyTo
        {
            get
            {
                return new[]{
                    new MvcRoute()
                {
                    Area = "Sites",
                    Controller = "Site",
                    Action = "Settings"
                }
                };
            }
        }

        #region DirName
        static string SiteTabsDir = "SiteTabs";
        #endregion

        #region SystemTabs
        public static IEnumerable<TabInfo> SystemTabs()
        {
            var baseDir = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<IBaseDir>();
            string tabPath = Path.Combine(baseDir.Cms_DataPhysicalPath, SiteTabsDir);
            return GetTabs(tabPath);

        }
        #endregion

        #region SiteTabs
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
        #endregion

        #region ModuleTabs
        public static IEnumerable<TabInfo> ModuleTabs(Site site)
        {

            var tabs = Enumerable.Empty<TabInfo>();

            var modules = ServiceFactory.ModuleManager.AllModulesForSite(site.FullName);

            foreach (var moduleName in modules)
            {

                string tabPath = Path.Combine(Settings.BaseDirectory, "Areas", moduleName, SiteTabsDir);

                tabs = GetTabs(tabPath).Concat(tabs);

            }

            return tabs;
        }
        #endregion

        #region GetTabs
        private static IEnumerable<TabInfo> GetTabs(string tabPath)
        {
            if (Directory.Exists(tabPath))
            {
                foreach (var file in Directory.EnumerateFiles(tabPath, "*.cshtml"))
                {
                    var tabInfo = new TabInfo();
                    var name = Path.GetFileNameWithoutExtension(file);
                    tabInfo.Name = name;
                    tabInfo.DisplayText = name;
                    tabInfo.VirtualPath = file.Replace(Settings.BaseDirectory, "~/");
                    yield return tabInfo;
                }
            }
        }
        public IEnumerable<TabInfo> GetTabs(RequestContext requestContext)
        {
            var site = GetSite(requestContext);
            if (site == null)
            {
                return new TabInfo[0];
            }

            var tabs = SystemTabs().Concat(ModuleTabs(site)).Concat(SiteTabs(site));

            return tabs;
        }

        private Site GetSite(RequestContext requestContext)
        {
            var siteName = requestContext.GetRequestValue("siteName");
            if (!string.IsNullOrEmpty(siteName))
            {
                var site = new Site(siteName).AsActual();
                return site;
            }
            return null;
        }
        #endregion
    }
}
