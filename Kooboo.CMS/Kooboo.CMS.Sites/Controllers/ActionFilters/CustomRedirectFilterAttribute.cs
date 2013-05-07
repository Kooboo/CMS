#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Web;
using Kooboo.Globalization;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Controllers.ActionFilters
{
    public class CustomRedirectFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = (FrontHttpRequestWrapper)filterContext.HttpContext.Request;
            RedirectType redirectType;
            string inputUrl = request.RequestUrl;
            if (string.IsNullOrEmpty(inputUrl))
            {
                inputUrl = "/";
            }
            if (!string.IsNullOrEmpty(request.Url.Query))
            {
                inputUrl = inputUrl + request.Url.Query;
            }
            string redirectUrl;
            if (UrlMapperFactory.Default.Map(Site.Current, inputUrl, out redirectUrl, out redirectType))
            {
                filterContext.Result = RedirectHelper.CreateRedirectResult(Site.Current, request.RequestChannel, redirectUrl, null, null, redirectType);
            }
        }
    }
}
