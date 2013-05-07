#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Web;
using Kooboo.Globalization;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Controllers.ActionFilters
{
    public class SiteOfflinedFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var site = Site.Current;
            var requestChannel = ((FrontHttpRequestWrapper)filterContext.HttpContext.Request).RequestChannel;
            if (requestChannel == FrontRequestChannel.Host || requestChannel == FrontRequestChannel.HostNPath)
            {
                if (!Persistence.Providers.SiteProvider.IsOnline(site))
                {
                    var customError = Services.ServiceFactory.CustomErrorManager.Get(site, HttpErrorStatusCode.SiteOfflined.ToString());
                    if (customError != null)
                    {
                        var redirectUrl = UrlUtility.ResolveUrl(customError.RedirectUrl);

                        if (redirectUrl.TrimStart('/') != filterContext.HttpContext.Request.RawUrl.TrimStart('/'))
                        {
                            filterContext.Result = new RedirectResult(redirectUrl);
                        }
                    }
                    else
                    {
                        // 503 to be used for Site offlined.
                        throw new HttpException(503, "The site has been taken offline.".Localize());
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
