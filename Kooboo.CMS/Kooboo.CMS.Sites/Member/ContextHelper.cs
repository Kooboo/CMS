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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Member
{
    public static class ContextHelper
    {
        public static Membership GetMembership()
        {
            if (Site.Current == null)
            {
                throw new InvalidOperationException("The site can not be found.");
            }
            var membership = Site.Current.GetMembership();
            if (membership == null)
            {
                throw new InvalidOperationException("The site does not relate to any membership.");
            }
            return membership;
        }

        public static string GetReturnUrl(ControllerContext controllerContext)
        {
            var redirectUrl = controllerContext.HttpContext.Request.QueryString["ReturnUrl"];
            return redirectUrl;
        }
    }
}
