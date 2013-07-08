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
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Controllers
{
    public static class RedirectHelper
    {
        public static ActionResult CreateRedirectResult(Site site, FrontRequestChannel channel, string url, string rawUrl, int? statusCode, RedirectType redirectType, bool appendErrorPath = true)
        {
            var redirectUrl = url;
            if (!UrlUtility.IsAbsoluteUrl(redirectUrl))
            {
                redirectUrl = UrlUtility.ResolveUrl(redirectUrl);
                //WrapperUrl will cause endless loop if site host by ASP.NET development server when transfer redirect.
                if (redirectType != RedirectType.Transfer || Settings.IsHostByIIS)
                {
                    redirectUrl = FrontUrlHelper.WrapperUrl(redirectUrl, site, channel).ToString();
                }
            }
            if (appendErrorPath == true)
            {
                if (!string.IsNullOrEmpty(rawUrl))
                {
                    redirectUrl = redirectUrl.AddQueryParam("returnUrl", rawUrl);
                }
                //if (statusCode != null)
                //{
                //    redirectUrl = redirectUrl.AddQueryParam("statusCode", statusCode.ToString());
                //}
            }
            switch (redirectType)
            {
                case RedirectType.Moved_Permanently_301:
                    return new Redirect301Result(redirectUrl);

                case RedirectType.Transfer:
                    return new TransferResult(redirectUrl, statusCode ?? 200);
                case RedirectType.Found_Redirect_302:
                default:
                    return new RedirectResult(redirectUrl);
            }

        }
    }
}
