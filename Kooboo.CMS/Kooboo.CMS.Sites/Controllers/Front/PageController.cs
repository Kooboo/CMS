//# define Page_Trace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Web;
using System.Web;
using Kooboo.CMS.Sites.View;
using System.IO;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Sites.Caching;
using System.Diagnostics;
using Kooboo.Web.Mvc;
using Kooboo.Web.Url;
using Kooboo.Globalization;

namespace Kooboo.CMS.Sites.Controllers.Front
{
    public class OutputTextWriterWrapper : StringWriter
    {
        public TextWriter InnerTextWriter { get; set; }

        bool rendered = false;
        public OutputTextWriterWrapper(TextWriter httpOutputWriter)
        {
            InnerTextWriter = httpOutputWriter;
        }
        public virtual TextWriter GetHttpTextWriter()
        {
            TextWriter textWriter = this;
            while (textWriter is OutputTextWriterWrapper && ((OutputTextWriterWrapper)textWriter).InnerTextWriter != null)
            {
                textWriter = ((OutputTextWriterWrapper)textWriter).InnerTextWriter;
            }
            return textWriter;
        }
        public virtual void Render(HttpResponseBase response)
        {
            if (!rendered)
            {
                var originalWriter = GetHttpTextWriter();
                response.Output = originalWriter;
                originalWriter.Write(this.ToString());
                rendered = true;
            }
        }
    }
    public class PageExecutionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
#if Page_Trace
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            base.OnActionExecuting(filterContext);

            var controllerContext = filterContext.Controller.ControllerContext;

            var pageRequestContext = InitPageRequestContext(controllerContext);

            Page_Context.Current.InitContext(pageRequestContext, controllerContext);

#if Page_Trace
            stopwatch.Stop();
            filterContext.HttpContext.Response.Write(string.Format("PageExecutionFilterAttribute.OnActionExecuting, {0}ms.</br>", stopwatch.ElapsedMilliseconds));
#endif

        }

        private PageRequestContext InitPageRequestContext(ControllerContext controllerContext)
        {
            FrontControllerBase frontController = (FrontControllerBase)controllerContext.Controller;
            string pageUrl = frontController.RequestUrl;

#if Page_Trace
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            //pageUrl: product/detail/product1
            var page = Services.ServiceFactory.PageManager.GetPageByUrl(frontController.Site, pageUrl);

#if Page_Trace
            stopwatch.Stop();
            controllerContext.HttpContext.Response.Write(string.Format("GetPageByUrl, {0}ms.</br>", stopwatch.ElapsedMilliseconds));
#endif
            if (page == null)
            {
                throw new HttpException(0x194, string.Format(SR.GetString("Path_not_found"), new object[] { frontController.Request.Path }));
            }

            var draft = controllerContext.RequestContext.GetRequestValue("_draft_");
            if (!string.IsNullOrEmpty(draft) && draft.ToLower() == "true")
            {
                page = Services.ServiceFactory.PageManager.PageProvider.GetDraft(page);
                frontController.FrontHttpRequest.RequestChannel = FrontRequestChannel.Draft;
            }
            else
            {
                EnsurePagePublished(frontController, page);
            }
            var requestUrl = "";
            if (!string.IsNullOrEmpty(pageUrl))
            {
                requestUrl = pageUrl.Substring(page.VirtualPath.TrimStart('/').Length).TrimStart('/');
            }

            return new PageRequestContext(controllerContext, frontController.Site, page, frontController.FrontHttpRequest.RequestChannel, requestUrl);
        }

        protected virtual void EnsurePagePublished(Controller controller, Page page)
        {
            if (page.Published.Value == false)
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    throw new HttpException(0x194, string.Format(SR.GetString("Path_not_found"), new object[] { controller.Request.Path }));
                }
            }

        }
    }
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
                    filterContext.Result = new ContentResult() { Content = outputCache.ToString(), ContentType = "text/html" };
                }
                else if (!(filterContext.HttpContext.Response.Output is OutputTextWriterWrapper))
                {
                    filterContext.HttpContext.Response.Output = new OutputTextWriterWrapper(filterContext.HttpContext.Response.Output);
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
                        if (outputTextWriterWrapper != null)
                        {
                            site.ObjectCache().Add(cacheKey, outputTextWriterWrapper.ToString(), page.AsActual().OutputCache.ToCachePolicy());
                        }

                        outputTextWriterWrapper.Render(filterContext.HttpContext.Response);
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

    public class CustomRedirectFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = (FrontHttpRequestWrapper)filterContext.HttpContext.Request;
            RedirectType redirectType;
            string inputUrl = request.RequestUrl;
            if (string.IsNullOrEmpty(inputUrl))
            {
                inputUrl = "/";
            }
            if (!string.IsNullOrEmpty(request.Url.Query))
            {
                inputUrl = inputUrl + request.Url.Query;
            }
            string redirectUrl;
            if (UrlMapperFactory.Default.Map(Site.Current, inputUrl, out redirectUrl, out redirectType))
            {
                filterContext.Result = RedirectHelper.CreateRedirectResult(Site.Current, request.RequestChannel, redirectUrl, null, null, redirectType);
            }
        }
    }

    public class SiteOfflinedFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var site = Site.Current;
            var requestChannel = ((FrontHttpRequestWrapper)filterContext.HttpContext.Request).RequestChannel;
            if (requestChannel == FrontRequestChannel.Host || requestChannel == FrontRequestChannel.HostNPath)
            {
                if (!Persistence.Providers.SiteProvider.IsOnline(site))
                {
                    var customError = Services.ServiceFactory.CustomErrorManager.Get(site, HttpErrorStatusCode.SiteOfflined.ToString());
                    if (customError != null)
                    {
                        var redirectUrl = UrlUtility.ResolveUrl(customError.RedirectUrl);

                        if (redirectUrl != filterContext.HttpContext.Request.RawUrl)
                        {
                            filterContext.Result = new RedirectResult(redirectUrl);
                        }
                    }
                    else
                    {
                        // 503 to be used for Site offlined.
                        throw new HttpException(503, "The site has been taken offline.".Localize());
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class PageController : FrontControllerBase
    {
        public Page_Context PageContext
        {
            get
            {
                return Page_Context.Current;
            }
        }
        [SiteOfflinedFilter(Order = 0)]
        [CustomRedirectFilter(Order = 5)]
        [PageExecutionFilter(Order = 10)]
        [OutputCacheFilter(Order = 15)]
        public virtual ActionResult Entry()
        {
#if Page_Trace
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            var actionResult = Page_Context.Current.ExecutePlugins();
            if (actionResult != null)
            {
                return actionResult;
            }
#if Page_Trace
            stopwatch.Stop();
            Response.Write(string.Format("ExecutePlugins, {0}ms.</br>", stopwatch.ElapsedMilliseconds));
            stopwatch.Reset();
            stopwatch.Start();
#endif
            Page_Context.Current.ExecuteDataRules();

#if Page_Trace
            stopwatch.Stop();
            Response.Write(string.Format("ExecuteDataRules, {0}ms</br>", stopwatch.ElapsedMilliseconds));
            stopwatch.Reset();
            stopwatch.Start();
#endif
            Page_Context.Current.InitializeTitleHtmlMeta();

#if Page_Trace
            stopwatch.Stop();
            Response.Write(string.Format("InitializeTitleHtmlMeta, {0}ms</br>", stopwatch.ElapsedMilliseconds));
            stopwatch.Reset();
            stopwatch.Start();
#endif
            if (Page_Context.Current.ExecuteModuleControllerAction())
            {
                return ViewPage();
            }
#if Page_Trace
            stopwatch.Stop();
            Response.Write(string.Format("ViewPage, {0}ms</br>", stopwatch.ElapsedMilliseconds));
#endif
            return null;
        }

        protected virtual ActionResult ViewPage()
        {
            var layout = (new Layout(Site, PageContext.PageLayout).LastVersion()).AsActual();

            ViewResult viewResult = new FrontViewResult(ControllerContext, layout.FileExtension.ToLower(), layout.TemplateFileVirutalPath);

            if (viewResult != null)
            {
                viewResult.ViewData = this.ViewData;
                viewResult.TempData = this.TempData;
            }


            return viewResult;
        }

        protected override void OnSiteNotExists()
        {
            RedirectTo404().ExecuteResult(this.ControllerContext);
            //throw new HttpException(0x194, string.Format(SR.System_Web_Resources.GetString("Path_not_found"), new object[] { HttpContext.Request.Path }));
        }
    }
}
