#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;

using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using Kooboo.Common.ObjectContainer;
using System.IO;
using Kooboo.CMS.Sites.Services;
using Kooboo.Common;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Sites.Extension.UI.Tabs
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ITabProvider), Key = "PageCustomTabProvider")]
    public class PageCustomTabProvider : ITabProvider
    {
        public MvcRoute[] ApplyTo
        {
            get
            {
                return new[]{
                     new MvcRoute()
                {
                    Area = "Sites",
                    Controller = "Page",
                    Action = "Draft"
                },
                    new MvcRoute()
                {
                    Area = "Sites",
                    Controller = "Page",
                    Action = "Edit"
                }
                };
            }
        }

        #region GetTabs
        public IEnumerable<TabInfo> GetTabs(System.Web.Routing.RequestContext requestContext)
        {
            var site = GetSite(requestContext);
            if (site == null)
            {
                return new TabInfo[0];
            }
            var page = GetPage(site, requestContext);
            if (page == null)
            {
                return new TabInfo[0];
            }

            return SystemTabs().Concat(ModuleTabs(site)).Concat(SiteTabs(site)).Concat(LayoutTabs(site, page.Layout));
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
        private Page GetPage(Site site, RequestContext requestContext)
        {
            var pageName = requestContext.GetRequestValue("UUID");
            if (!string.IsNullOrEmpty(pageName))
            {
                var page = new Page(site, pageName).AsActual();
                return page;
            }
            return null;
        }

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
        #endregion

        #region DirName
        static string PageTabsDir = "PageTabs";
        #endregion

        #region SystemTabs
        public static IEnumerable<TabInfo> SystemTabs()
        {
            var baseDir = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<IBaseDir>();
            string tabPath = Path.Combine(baseDir.Cms_DataPhysicalPath, PageTabsDir);
            return GetTabs(tabPath);

        }
        #endregion

        #region LayoutTabs
        private static IEnumerable<TabInfo> LayoutTabs(Site site, string layoutName)
        {
            var layout = new Layout(site, layoutName).LastVersion();
            string tabPath = Path.Combine(layout.PhysicalPath, PageTabsDir);
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
                tabPath = Path.Combine(parentSite.PhysicalPath, PageTabsDir);
                tabs = GetTabs(tabPath).Concat(tabs);
                parentSite = parentSite.Parent;
            }

            tabPath = Path.Combine(site.PhysicalPath, PageTabsDir);
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

                string tabPath = Path.Combine(Settings.BaseDirectory, "Areas", moduleName, PageTabsDir);

                tabs = GetTabs(tabPath).Concat(tabs);

            }

            return tabs;
        }
        #endregion
    }
}
