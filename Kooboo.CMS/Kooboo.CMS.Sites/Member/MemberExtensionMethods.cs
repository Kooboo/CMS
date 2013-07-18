#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Sites.Member
{
    public static class MemberExtensionMethods
    {
        public static MemberAuthentication MemberAuthentication(this HttpContextBase httpContext)
        {
            var site = Site.Current;
            return new MemberAuthentication(site, httpContext);
        }
    }
}
