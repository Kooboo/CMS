using Kooboo.CMS.SiteKernel.Extension.Site;
using Kooboo.Common.Web.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Areas.Sites.Buttons.Home.Cluster
{
    public class CreateANewSubSite_Top : IButtonPlugin
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
                Action = "CreateSubSite"
            };
        }

        public virtual string GroupName
        {
            get { return "Create"; }
        }

        public virtual Type OptionModelType
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string DisplayText
        {
            get { return "A new sub site"; }
        }

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
            throw new NotImplementedException();
        }

        public virtual string Name
        {
            get { return "CreateANewSubSite"; }
        }

        public virtual int Order
        {
            get { return 2; }
        }

        public virtual IEnumerable<Kooboo.Common.Web.MvcRoute> ApplyTo
        {
            get { return new[] { SiteExtensionPoints.SiteCluster }; }
        }


        public virtual string Position
        {
            get { return null; }
        }
    }
}