using Kooboo.Common.Web.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Areas.Sites.Buttons.Home.SiteMap
{
    public abstract class SiteMapNodeButtonBase : IButtonPlugin
    {
        System.Web.Mvc.ActionResult IButtonPlugin.Execute(Kooboo.Common.Web.Button.ButtonPluginContext context)
        {
            throw new NotImplementedException();
        }

        public virtual Kooboo.Common.Web.MvcRoute GetMvcRoute(System.Web.Mvc.ControllerContext controllerContext, object dataContext)
        {
            return new Kooboo.Common.Web.MvcRoute();
        }

        public virtual string GroupName
        {
            get { return null; }
        }

        public virtual Type OptionModelType
        {
            get { return null; }
        }

        public abstract string DisplayText { get; }

        public virtual IDictionary<string, object> HtmlAttributes(System.Web.Mvc.ControllerContext controllerContext)
        {
            return new Dictionary<string, object>();
        }

        public virtual string IconClass
        {
            get { return null; }
        }

        public virtual bool IsVisibleFor(object dataItem)
        {
            return true;
        }

        public abstract string Name { get; }

        public abstract int Order { get; }

        public virtual IEnumerable<Kooboo.Common.Web.MvcRoute> ApplyTo
        {
            get { return new[] { Kooboo.CMS.SiteKernel.Extension.Page.PageExtensionPoints.SiteMap }; }
        }

        public virtual string Position
        {
            get { return Kooboo.CMS.SiteKernel.Extension.Page.PageExtensionPoints.SiteMapNodeButton; }
        }
    }
}