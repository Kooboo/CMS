#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Web.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Extension
{
    public abstract class ManagedButtonPlugin : IButtonPlugin
    {
        public abstract string Name { get; }
        public abstract string DisplayText { get; }

        public abstract string GroupName { get; }

        public abstract string IconClass { get; }

        public abstract Type OptionModelType { get; }

        public abstract int Order { get; }

        public abstract IEnumerable<Kooboo.Common.Web.MvcRoute> ApplyTo { get; }

        /// <summary>
        /// 托管的Button不需要实现GetMvcRoute
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        Kooboo.Common.Web.MvcRoute IButtonPlugin.GetMvcRoute(System.Web.Mvc.ControllerContext controllerContext)
        {
            return null;
        }

        public abstract IDictionary<string, object> HtmlAttributes(System.Web.Mvc.ControllerContext controllerContext);

        public abstract bool IsVisibleFor(object dataItem);

        public abstract System.Web.Mvc.ActionResult Execute(ButtonPluginContext context);

    }
}
