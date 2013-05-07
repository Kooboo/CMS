#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Controllers.ActionFilters
{
    public class CheckSiteExistsActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Site.Current == null)
            {
                OnSiteNotExists(filterContext);
            }

            base.OnActionExecuting(filterContext);
        }

        protected virtual void OnSiteNotExists(ActionExecutingContext filterContext)
        {
            throw new HttpException(0x194, string.Format(SR.GetString("Path_not_found"), new object[] { filterContext.HttpContext.Request.Path }));
        }
    }
}
