#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Member.Models;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.Web.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Member
{
    public static class MemberPluginHelper
    {
        public static Membership GetMembership()
        {
            if (Site.Current == null)
            {
                throw new NullSiteContextException();
            }
            var membership = Site.Current.GetMembership();
            if (membership == null)
            {
                throw new NullMembershipException();
            }
            return membership;
        }

        public static string GetReturnUrl(ControllerContext controllerContext)
        {
            var redirectUrl = controllerContext.HttpContext.Request.QueryString["ReturnUrl"];
            return redirectUrl;
        }

        public static string ResolveSiteUrl(ControllerContext controllerContext, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }
            url = UrlHelper.GenerateContentUrl(url, controllerContext.HttpContext);
            return controllerContext.RequestContext.UrlHelper().FrontUrl().WrapperUrl(url).ToString();
        }
      
    }
}
