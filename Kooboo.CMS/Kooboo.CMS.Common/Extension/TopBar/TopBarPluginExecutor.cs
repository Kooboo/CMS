#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Kooboo.CMS.Common.Extension.TopBar
{
    [Dependency(typeof(ITopBarPluginExecutor))]
    public class TopBarPluginExecutor : ITopBarPluginExecutor
    {
        #region .ctor
        IEnumerable<ITopBarPlugin> topbarPlugins;
        IApplyToMatcher applyToMatcher;
        public TopBarPluginExecutor(IEnumerable<ITopBarPlugin> tabPlugins, IApplyToMatcher applyToMatcher)
        {
            this.topbarPlugins = tabPlugins;
            this.applyToMatcher = applyToMatcher;
        }
        #endregion

        #region MatchTopBarPlugins
        protected virtual IEnumerable<ITopBarPlugin> MatchTopBarPlugins(RouteData route)
        {
            return this.applyToMatcher.Match(this.topbarPlugins, route);
        }
        #endregion

        #region Execute
        public System.Web.Mvc.ActionResult Execute(System.Web.Mvc.ControllerContext controllerContext, string pluginName)
        {
            var executingPlugin = MatchTopBarPlugins(controllerContext.RequestContext.RouteData).Where(it => it.Name.EqualsOrNullEmpty(pluginName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (executingPlugin != null)
            {
                object optionModel = null;
                if (executingPlugin.OptionType != null)
                {
                    optionModel = ModelBindHelper.BindModel(executingPlugin.OptionType, controllerContext);
                }

                return executingPlugin.Execute(new TopBarContext(controllerContext, optionModel, null));
            }
            else
            {
                throw new Exception("The top bar plugin can not be found.");
            }
        }
        #endregion

        #region LoadTopBarPlugins
        public IEnumerable<ITopBarPlugin> LoadTopBarPlugins(System.Web.Mvc.ControllerContext controllerContext)
        {
            var topBarPlugins = MatchTopBarPlugins(controllerContext.RouteData);

            return topbarPlugins;
        }
        #endregion
    }
}
