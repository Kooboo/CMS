#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Web;
using Kooboo.Common.Globalization;


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Controllers.ActionFilters
{
    public class CacheToDiskAttribute : ActionFilterAttribute
    {
        //[Inject]
        public PageCachingManager PageCachingManager { get; set; }
        public CacheToDiskAttribute()
        {
            PageCachingManager = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<PageCachingManager>();
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var page = Page_Context.Current.PageRequestContext.Page.AsActual();
            var site = Page_Context.Current.PageRequestContext.Site;
            if (!filterContext.HttpContext.Request.IsAjaxRequest() && (!filterContext.HttpContext.User.Identity.IsAuthenticated) && page.CacheToDisk
                && filterContext.HttpContext.Request.HttpMethod.ToUpper() == "GET")
            {
                string html = PageCachingManager.GetCaching(filterContext.HttpContext, page);

                if (html != null)
                {
                    filterContext.Result = new CachedContentResult() { Content = html.ToString(), ContentType = "text/html" };
                }
            }
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            if (Page_Context.Current.PageRequestContext != null)
            {
                var site = Page_Context.Current.PageRequestContext.Site;
                var page = Page_Context.Current.PageRequestContext.Page.AsActual();
                if (!filterContext.HttpContext.Request.IsAjaxRequest() && !filterContext.HttpContext.User.Identity.IsAuthenticated && page.CacheToDisk)
                {
                    var outputTextWriterWrapper = filterContext.HttpContext.Response.Output as OutputTextWriterWrapper;
                    if (outputTextWriterWrapper != null
                        && filterContext.Exception == null
                        && filterContext.HttpContext.Response.ContentType.ToLower().Contains("text/html"))
                    {
                        try
                        {
                            var html = outputTextWriterWrapper.ToString();
                            PageCachingManager.AddCaching(filterContext.HttpContext, page, html);
                        }
                        catch (Exception e)
                        {
                           Kooboo.Common.Logging.Logger.Error(e.Message, e);
                        }
                    }
                }
            }
        }
    }
}
