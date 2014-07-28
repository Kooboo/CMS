#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Kooboo.CMS.Common.Extension.Tab
{
    public class TabPluginLoader : ITabPluginLoader
    {
        #region .ctor
        IEnumerable<ITabPlugin> tabPlugins;
        IApplyToMatcher applyToMatcher;
        public TabPluginLoader(IEnumerable<ITabPlugin> tabPlugins, IApplyToMatcher applyToMatcher)
        {
            this.tabPlugins = tabPlugins;
            this.applyToMatcher = applyToMatcher;
        }
        #endregion

        #region LoadTabPlugins
        public IEnumerable<LoadedTabPlugin> LoadTabPlugins(System.Web.Mvc.ControllerContext controllerContext)
        {
            var matchedTabs = MatchTabPlugins(controllerContext.RequestContext.RouteData).OrderBy(it => it.Order);

            var loadedPlugins = new List<LoadedTabPlugin>();

            foreach (var tab in matchedTabs)
            {
                var tabLoadContext = new TabContext(controllerContext, new System.Web.Mvc.ViewDataDictionary(controllerContext.Controller.ViewData));
                tab.LoadData(tabLoadContext);
                loadedPlugins.Add(new LoadedTabPlugin(tab, tabLoadContext));
            }
            return loadedPlugins;
        }

        #endregion

        protected virtual IEnumerable<ITabPlugin> MatchTabPlugins(RouteData route)
        {
            return this.applyToMatcher.Match(this.tabPlugins, route);
        }

        #region SubmitToTabPlugins
        public IEnumerable<LoadedTabPlugin> SubmitToTabPlugins(System.Web.Mvc.ControllerContext controllerContext)
        {
            var matchedTabs = MatchTabPlugins(controllerContext.RequestContext.RouteData).OrderBy(it => it.Order);

            var loadedPlugins = new List<LoadedTabPlugin>();

            foreach (var tab in matchedTabs)
            {
                object model = null;
                if (tab.ModelType != null)
                {
                    model = ModelBindHelper.BindModel(tab.ModelType, controllerContext);
                }
                TabContext tabContext = new TabContext(controllerContext, new System.Web.Mvc.ViewDataDictionary(controllerContext.Controller.ViewData) { Model = model });
                tab.Submit(tabContext);
                loadedPlugins.Add(new LoadedTabPlugin(tab, tabContext));
            }
            return loadedPlugins;
        }
        #endregion
    }
}