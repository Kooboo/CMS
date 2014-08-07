using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Areas.Sites.Buttons.Home.Cluster
{
    public class Delete_SiteNode : Kooboo.Common.Web.Button.IButtonPlugin
    {
        System.Web.Mvc.ActionResult Kooboo.Common.Web.Button.IButtonPlugin.Execute(Kooboo.Common.Web.Button.ButtonPluginContext context)
        {
            throw new NotImplementedException();
        }

        Kooboo.Common.Web.MvcRoute Kooboo.Common.Web.Button.IButtonPlugin.GetMvcRoute(System.Web.Mvc.ControllerContext controllerContext)
        {
            return new Kooboo.Common.Web.MvcRoute()
            {
                Area = "Sites",
                Controller = "Site",
                Action = "Delete"
            };
        }

        string Kooboo.Common.Web.Button.IButtonPlugin.GroupName
        {
            get { return null; }
        }

        Type Kooboo.Common.Web.Button.IButtonPlugin.OptionModelType
        {
            get { throw new NotImplementedException(); }
        }

        string Kooboo.Common.Web.Button.IButton.DisplayText
        {
            get { return "Delete"; }
        }

        IDictionary<string, object> Kooboo.Common.Web.Button.IButton.HtmlAttributes(System.Web.Mvc.ControllerContext controllerContext)
        {
            return new Dictionary<string, object>();
        }

        string Kooboo.Common.Web.Button.IButton.IconClass
        {
            get { return null; }
        }

        bool Kooboo.Common.Web.Button.IButton.IsVisibleFor(object dataItem)
        {
            throw new NotImplementedException();
        }

        string Kooboo.Common.Web.Button.IButton.Name
        {
            get { return "Delete_Site"; }
        }

        int Kooboo.Common.Web.Button.IButton.Order
        {
            get { return 4; }
        }

        IEnumerable<Kooboo.Common.Web.MvcRoute> Kooboo.Common.Web.IApplyTo.ApplyTo
        {
            get { return new[] { Kooboo.CMS.SiteKernel.Extension.Site.SiteExtensionPoints.SiteCluster }; }
        }

        string Kooboo.Common.Web.IApplyTo.Position
        {
            get { return Kooboo.CMS.SiteKernel.Extension.Site.SiteExtensionPoints.SiteNodeButton; }
        }
    }
}