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
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Web;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
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

            PageRequestModuleExecutor.OnResolvingPage(controllerContext);

            var pageRequestContext = CreatePageRequestContext(controllerContext);

            PageRequestModuleExecutor.OnResolvedPage(controllerContext, pageRequestContext);

            Page_Context.Current.InitContext(pageRequestContext, controllerContext);

#if Page_Trace
            stopwatch.Stop();
            filterContext.HttpContext.Response.Write(string.Format("PageExecutionFilterAttribute.OnActionExecuting, {0}ms.</br>", stopwatch.ElapsedMilliseconds));
#endif

        }

        private PageRequestContext CreatePageRequestContext(ControllerContext controllerContext)
        {
            FrontControllerBase frontController = (FrontControllerBase)controllerContext.Controller;
            string pageUrl = frontController.RequestUrl;

#if Page_Trace
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            //pageUrl: product/detail/product1
            var rawPage = Services.ServiceFactory.PageManager.GetPageByUrl(frontController.Site, pageUrl);
            if (rawPage == null)
            {
                throw new HttpException(0x194, string.Format(SR.GetString("Path_not_found"), new object[] { frontController.Request.Path }));
            }
            var requestUrl = "";
            if (!string.IsNullOrEmpty(pageUrl))
            {
                requestUrl = pageUrl.Substring(rawPage.VirtualPath.TrimStart('/').Length).TrimStart('/');
            }
            var page = Services.ServiceFactory.PageVisitRuleManager.MatchRule(frontController.Site, rawPage, controllerContext.HttpContext);

#if Page_Trace
            stopwatch.Stop();
            controllerContext.HttpContext.Response.Write(string.Format("GetPageByUrl, {0}ms.</br>", stopwatch.ElapsedMilliseconds));
#endif
            var draft = controllerContext.RequestContext.GetRequestValue("_draft_");
            if (!string.IsNullOrEmpty(draft) && draft.ToLower() == "true")
            {
                page = Services.ServiceFactory.PageManager.Provider.GetDraft(page);
                frontController.FrontHttpRequest.RequestChannel = FrontRequestChannel.Draft;
            }
            else
            {
                EnsurePagePublished(frontController, page);
            }

            return new PageRequestContext(controllerContext, frontController.FrontHttpRequest.RawSite, frontController.Site, rawPage, page, frontController.FrontHttpRequest.RequestChannel, requestUrl);
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
}
