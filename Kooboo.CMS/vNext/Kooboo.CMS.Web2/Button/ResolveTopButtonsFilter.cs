using Kooboo.Common.Web.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web2.Button
{
    public class ResolveTopButtonsFilter : ActionFilterAttribute
    {
        [Kooboo.Common.ObjectContainer.Dependency.Inject]
        public IButtonPluginExecutor ButtonPluginExecutor { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            filterContext.Controller.ViewBag.TopButtons = ButtonPluginExecutor.LoadButtons(filterContext.Controller.ControllerContext);
        }
    }
}