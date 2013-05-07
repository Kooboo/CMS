#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Web;
using Kooboo.Globalization;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Controllers.ActionFilters
{
    public class OutputCacheFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
#if Page_Trace
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            base.OnActionExecuting(filterContext);
            var page = Page_Context.Current.PageRequestContext.Page.AsActual();
            var site = Page_Context.Current.PageRequestContext.Site;
            if ((!filterContext.HttpContext.User.Identity.IsAuthenticated) && page.EnabledCache
                && filterContext.HttpContext.Request.HttpMethod.ToUpper() == "GET")
            {
                SetCache(page, filterContext.HttpContext, page.OutputCache.Duration, "*");

                var cacheKey = GetOutputCacheKey(filterContext.HttpContext, page);
                var outputCache = site.ObjectCache().Get(cacheKey);
                if (outputCache != null)
                {
                    filterContext.Result = new CachedContentResult() { Content = outputCache.ToString(), ContentType = "text/html" };
                }
            }
#if Page_Trace
            stopwatch.Stop();
            filterContext.HttpContext.Response.Write(string.Format("OutputCacheFilterAttribute.OnActionExecuting, {0}ms.</br>", stopwatch.ElapsedMilliseconds));
#endif
        }

        private static string GetOutputCacheKey(HttpContextBase httpContext, Page page)
        {
            var cacheKey = string.Format("Page OutputCache-Full page name:{0};Raw request url:{1}", page.FullName, httpContext.Request.RawUrl);
            return cacheKey;
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
#if Page_Trace
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            if (Page_Context.Current.PageRequestContext != null)
            {

                var site = Page_Context.Current.PageRequestContext.Site;
                var page = Page_Context.Current.PageRequestContext.Page.AsActual();
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated && page.EnabledCache)
                {
                    var outputTextWriterWrapper = filterContext.HttpContext.Response.Output as OutputTextWriterWrapper;
                    if (outputTextWriterWrapper != null
                        && filterContext.Exception == null
                        && filterContext.HttpContext.Response.ContentType.ToLower().Contains("text/html"))
                    {
                        var cacheKey = GetOutputCacheKey(filterContext.HttpContext, page);
                        if (outputTextWriterWrapper != null && site.ObjectCache().Get(cacheKey) == null)
                        {
                            site.ObjectCache().Add(cacheKey, outputTextWriterWrapper.ToString(), page.AsActual().OutputCache.ToCachePolicy());
                        }
                    }
                }

            }

#if Page_Trace
            stopwatch.Stop();
            filterContext.HttpContext.Response.Write(string.Format("OutputCacheFilterAttribute.OnResultExecuted, {0}ms.</br>", stopwatch.ElapsedMilliseconds));
#endif
        }
        protected virtual void SetCache(Page page, HttpContextBase httpContext, int cacheDuration, params string[] varyByParams)
        {
            // Cache
            if (cacheDuration > 0)
            {
                var response = httpContext.Response;

                response.AddHeader("Cache-Control", string.Format("public, max-age={0}", cacheDuration));
                //DateTime timestamp = httpContext.Timestamp;

                //HttpCachePolicyBase cache = response.Cache;
                //int duration = cacheDuration;

                //cache.SetCacheability(HttpCacheability.Public);
                //cache.SetAllowResponseInBrowserHistory(true);
                //cache.SetExpires(timestamp.AddSeconds(duration));
                //cache.SetMaxAge(new TimeSpan(0, 0, duration));
                //cache.SetValidUntilExpires(true);
                //cache.SetLastModified(timestamp);
                //cache.VaryByHeaders["Accept-Encoding"] = true;
                //if (varyByParams != null)
                //{
                //    foreach (var p in varyByParams)
                //    {
                //        cache.VaryByParams[p] = true;
                //    }
                //}

                //cache.SetOmitVaryStar(true);
            }
        }
    }
}
