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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Sites.Membership
{
    public static class MembershipExtensionMethods
    {
        public static IMembershipAuthentication Membership(this HttpContextBase httpContext)
        {
            if (Page_Context.Current.Initialized)
            {
                if (Page_Context.Current.PageRequestContext.RequestChannel == Web.FrontRequestChannel.Design)
                {
                    return new NullMembershipAuthentication();
                }
            }

            var site = Site.Current;
            var membership = site.GetMembership();
            if (membership == null)
            {
                return new NullMembershipAuthentication();
            }
            else
            {
                return new MembershipAuthentication(site, membership, httpContext);
            }
        }
    }
}
