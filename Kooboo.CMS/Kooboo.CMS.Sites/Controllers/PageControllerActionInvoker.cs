#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Controllers
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IActionInvoker))]
    public class PageControllerActionInvoker : ControllerActionInvoker
    {
        protected override void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
        {
            if (actionResult is FileResult)
            {
                controllerContext.HttpContext.Response.RestoreRawOutput();
            }
            base.InvokeActionResult(controllerContext, actionResult);
        }
    }
}
