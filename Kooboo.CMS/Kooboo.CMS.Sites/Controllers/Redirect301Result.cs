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
    public class Redirect301Result : RedirectResult
    {
        public Redirect301Result(string redirectUrl)
            : base(redirectUrl)
        {
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            var request = (FrontHttpRequestWrapper)httpContext.Request;

            context.HttpContext.Response.StatusCode = 301;
            context.HttpContext.Response.RedirectLocation = Url;
            context.HttpContext.Response.End();

        }
    }
}
