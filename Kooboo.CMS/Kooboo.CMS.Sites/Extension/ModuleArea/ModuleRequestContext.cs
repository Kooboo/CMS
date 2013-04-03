using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModuleRequestContext : RequestContext
    {
        public ModuleRequestContext(HttpContextBase httpContext, RouteData routeData, ModuleContext moduleContext)
            : base(httpContext, routeData)
        {
            this.ModuleContext = moduleContext;

        }
        public ModuleContext ModuleContext { get; private set; }
        public ControllerContext PageControllerContext { get; set; }
    }
}
