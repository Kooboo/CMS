using Kooboo.CMS.SiteKernel.Extension.Site;
using Kooboo.Common.Web.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Areas.Sites.Buttons.Home.Cluster
{
    public class CreateANewSite : IButtonPlugin
    {
        System.Web.Mvc.ActionResult IButtonPlugin.Execute(ButtonPluginContext context)
        {
            throw new NotImplementedException();
        }

        public Kooboo.Common.Web.MvcRoute GetMvcRoute(System.Web.Mvc.ControllerContext controllerContext)
        {

            return new Kooboo.Common.Web.MvcRoute()
            {
                Controller = "Site",
                Action = "Create"
            };
        }

        public string GroupName
        {
            get { return "Create"; }
        }

        public Type OptionModelType
        {
            get { throw new NotImplementedException(); }
        }

        public string DisplayText
        {
            get { return "A new site"; }
        }

        public IDictionary<string, object> HtmlAttributes(System.Web.Mvc.ControllerContext controllerContext)
        {
            return new Dictionary<string, object>();
        }

        public string IconClass
        {
            get { return null; }
        }

        public bool IsVisibleFor(object dataItem)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { return "CreateANewSite"; }
        }

        public int Order
        {
            get { return 1; }
        }

        public IEnumerable<Kooboo.Common.Web.MvcRoute> ApplyTo
        {
            get { return new[] { SiteExtensionPoints.SiteCluster }; }
        }
    }
}