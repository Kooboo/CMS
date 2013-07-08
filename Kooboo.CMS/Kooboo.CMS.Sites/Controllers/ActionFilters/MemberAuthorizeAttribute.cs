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
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Member;
using System.Security.Principal;
using Kooboo.CMS.Sites.View;
using System.Net;
namespace Kooboo.CMS.Sites.Controllers.ActionFilters
{
    public class MemberAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!AuthorizeCore(filterContext.HttpContext))
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }
        protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (!Page_Context.Current.Initialized)
            {
                throw new InvalidOperationException();
            }
            var permission = Page_Context.Current.PageRequestContext.Page.Permission;
            if (permission.RequireMember)
            {
                IPrincipal member = httpContext.MemberAuthentication().GetMember();
                if (!member.Identity.IsAuthenticated)
                {
                    return false;
                }
                var groups = permission.AllowGroups;
                if (groups != null && groups.Length > 0 && !groups.Any<string>(new Func<string, bool>(member.IsInRole)))
                {
                    return false;
                }
            }

            return true;
        }

        protected virtual void HandleUnauthorizedRequest(ActionExecutingContext filterContext)
        {
            throw new HttpException((int)HttpStatusCode.Unauthorized, "The page available for member only.");
        }
    }
}
