using Kooboo.CMS.SiteKernel.Extension.Site;
using Kooboo.Common.Web.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Button
{
    public class CreateGroup : IButtonGroup
    {
        public string DisplayText
        {
            get { return "Create"; }
        }

        public IDictionary<string, object> HtmlAttributes(System.Web.Mvc.ControllerContext controllerContext)
        {
            // throw new NotImplementedException();

            return new Dictionary<string, object>();
        }

        public string IconClass
        {
            get { return "add"; }
        }

        public bool IsVisibleFor(object dataItem)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { return "Create"; }
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