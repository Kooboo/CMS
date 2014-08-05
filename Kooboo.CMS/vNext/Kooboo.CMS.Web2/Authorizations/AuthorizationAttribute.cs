#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Security.Principal;
using System.Web.Routing;

namespace Kooboo.CMS.Web2.Authorizations
{  
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizationAttribute : RequiredLogOnAttribute
    {
        protected override bool AuthorizeCore(RequestContext requestContext)
        {
            var authorized = base.AuthorizeCore(requestContext);
            if (authorized)
            {
                var permission = new Kooboo.CMS.Account.Models.Permission() { AreaName = this.AreaName, Group = this.Group, Name = this.Name };

                return requestContext.Authorize(permission);
            }
            else
            {
                return authorized;
            }
        }

        public string AreaName { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
    }

    public class LargeFileAuthorizationAttribute : Kooboo.CMS.Web2.Authorizations.AuthorizationAttribute
    {
        protected override bool AuthorizeCore(System.Web.Routing.RequestContext requestContext)
        {
            if (requestContext.GetRequestValue("Action").ToLower() == "largefile")
            {
                return true;
            }
            return base.AuthorizeCore(requestContext);
        }
    }
}
