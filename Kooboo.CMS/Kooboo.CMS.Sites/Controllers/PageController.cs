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
using Kooboo.CMS.Sites.Controllers.ActionFilters;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Web;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Url;
//# define Page_Trace
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Sites.Controllers
{
    [CheckSiteExistsActionFilter(Order = 0)]
    [SiteOfflinedFilter(Order = 2)]
    public class PageController : FrontControllerBase
    {
        #region PageContext
        public Page_Context PageContext
        {
            get
            {
                return Page_Context.Current;
            }
        }
        #endregion

        #region Page Entry
        [CustomRedirectFilter(Order = 5)]
        [PageExecutionFilter(Order = 10)]
        [MemberAuthorize(Order = 15)]
        [CheckRequireHttps(Order = 20)]
        [FormSubmitionFilterAttribute(Order = 22)]
        [CustomOutputTextWriterFilter(Order = 25)]
        [OutputCacheFilter(Order = 30)]
        [CacheToDisk(Order = 35)]
        [PageHtmlParserActionFilter(Order = 40)]
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

            if (layout == null)
            {
                throw new KoobooException(string.Format("The layout does not exists. Layout name:{0}".Localize(), PageContext.PageLayout));
            }
            ViewResult viewResult = new FrontViewResult(ControllerContext, layout.FileExtension.ToLower(), layout.TemplateFileVirutalPath);

            if (viewResult != null)
            {
                viewResult.ViewData = this.ViewData;
                viewResult.TempData = this.TempData;
            }

            return viewResult;
        }

        #endregion

        #region View Entry
        protected class ViewMock : IView, IViewDataContainer
        {
            public void Render(ViewContext viewContext, TextWriter writer)
            {

            }
            public ViewDataDictionary ViewData
            {
                get;
                set;
            }
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public virtual ActionResult ViewEntry(string viewName)
        {
            viewName = Server.UrlDecode(viewName);
            var viewPosition = new ViewPosition()
            {
                LayoutPositionId = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(5),
                ViewName = viewName,
                PagePositionId = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(5)
            };
            //pageUrl: product/detail/product1
            var rawPage = new Page(Site, "____VisitViewPage_____") { IsDummy = false };
            rawPage.PagePositions.Add(viewPosition);

            var requestUrl = "/";

            var pageRequestContext = new PageRequestContext(this.ControllerContext, FrontHttpRequest.RawSite, FrontHttpRequest.Site, rawPage, rawPage,
                FrontHttpRequest.RequestChannel, requestUrl);

            Page_Context.Current.InitContext(pageRequestContext, ControllerContext);

            var actionResult = Page_Context.Current.ExecutePlugins();
            if (actionResult != null)
            {
                return actionResult;
            }

            Page_Context.Current.ExecuteDataRules();

            var viewMock = new ViewMock() { ViewData = new ViewDataDictionary(ViewData) };
            ViewContext viewContext = new ViewContext(this.ControllerContext, viewMock, ViewData, this.TempData, Response.Output);

            HtmlHelper html = new HtmlHelper(viewContext, viewMock);

            return Content(html.FrontHtml().RenderView(viewPosition).ToString());
        }
        #endregion

    }
}
