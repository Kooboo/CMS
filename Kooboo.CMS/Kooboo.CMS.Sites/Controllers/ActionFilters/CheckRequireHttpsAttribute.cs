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
using System.Threading.Tasks;
using System.Web.Mvc;
using Kooboo.Common.Globalization;
using Kooboo.CMS.Sites.View;
namespace Kooboo.CMS.Sites.Controllers.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class CheckRequireHttpsAttribute : ActionFilterAttribute
    {
        // Methods
        protected virtual void HandleNonHttpsRequest(ActionExecutingContext filterContext)
        {
            if (Page_Context.Current.Initialized)
            {
                if (Page_Context.Current.PageRequestContext.Page.RequireHttps)
                {
                    if (!string.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("The requested resource can only be accessed via SSL.".Localize());
                    }
                    string url = "https://" + filterContext.HttpContext.Request.Url.Host + filterContext.HttpContext.Request.RawUrl;
                    filterContext.Result = new RedirectResult(url);
                }
            }
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (!filterContext.HttpContext.Request.IsSecureConnection)
            {
                this.HandleNonHttpsRequest(filterContext);
            }
        }
    }

}
