#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.CMS.SiteKernel.SiteFlow.Args;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.SiteFlow
{
    public class PageRequestFlowAdapter
    {
        #region .ctor
        PageRequestFlow _pageRequestFlow;
        PageRequestFlowEvents[] _events;
        public PageRequestFlowAdapter(PageRequestFlow siteRequestFlow, PageRequestFlowEvents[] events)
        {
            Contract.Requires(siteRequestFlow != null);
            _pageRequestFlow = siteRequestFlow;
            _events = events ?? new PageRequestFlowEvents[0];
        }
        #endregion

        #region Properties
        public PageRequestFlow SiteRequestFlow
        {
            get { return _pageRequestFlow; }
        }
        public PageRequestFlowEvents[] EventHandlers
        {
            get { return _events; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// 根据需要创建一个自定义的HttpContext
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="site">The site.</param>
        /// <returns></returns>
        public Page MapPage(ControllerContext controllerContext, Site site)
        {
            Contract.Requires(controllerContext != null);
            Contract.Requires(site != null);
            foreach (var item in _events)
            {
                item.PreMapPage(this, new PreMapPageEventArgs(controllerContext, site));
            }
            var page = _pageRequestFlow.MapPage(controllerContext, site);
            foreach (var item in _events)
            {
                var args = new PostMapPageEventArgs(controllerContext, site, page);
                item.PostMapPage(this, args);
                page = args.Page;
            }

            return page;
        }


        /// <summary>
        /// 创建Page_Context
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="site"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Page_Context CreatePageContext(ControllerContext controllerContext, Site site, Page page)
        {
            Contract.Requires(controllerContext != null);
            Contract.Requires(site != null);
            Contract.Requires(page != null);

            foreach (var item in _events)
            {
                item.PreCreatePageContext(this, new PreCreatePageContextEventArgs(controllerContext, site, page));
            }
            var page_context = _pageRequestFlow.CreatePageContext(controllerContext, site, page);
            foreach (var item in _events)
            {
                var args = new PostCreatePageContextEventArgs(page_context);
                item.PostCreatePageContext(this, args);
                page_context = args.Page_Context;
            }

            return page_context;
        }

        /// <summary>
        /// 执行页面逻辑
        /// </summary>
        /// <param name="page_context"></param>
        /// <returns></returns>
        public ActionResult ExecutePage(Page_Context page_context)
        {
            Contract.Requires(page_context != null);
            foreach (var item in _events)
            {
                item.PreExecutePage(this, new PreExecutePageEventArgs(page_context));
            }

            var actionResult = _pageRequestFlow.ExecutePage(page_context);

            foreach (var item in _events)
            {
                var args = new PostExecutePageEventArgs(page_context, actionResult);
                item.PostExecutePage(this, args);
                actionResult = args.ActionResult;
            }

            return actionResult;
        }

        /// <summary>
        /// 渲染输出页面
        /// </summary>
        /// <param name="page_context"></param>
        /// <param name="actionResult"></param>
        public void RenderPage(Page_Context page_context, ActionResult actionResult)
        {
            Contract.Requires(page_context != null);
            Contract.Requires(actionResult != null);
            foreach (var item in _events)
            {
                item.PreRenderPage(this, new PreRenderPageEventArgs(page_context, actionResult));
            }

            _pageRequestFlow.ExecutePage(page_context);

            foreach (var item in _events)
            {
                var args = new PostRenderPageEventArgs(page_context, actionResult);
                item.PostRenderPage(this, args);

            }
        }

        /// <summary>
        /// 结束页面请求
        /// </summary>
        /// <param name="page_Context"></param>
        public void EndPageRequest(Page_Context page_context)
        {
            Contract.Requires(page_context != null);
            foreach (var item in _events)
            {
                item.EndPageRequest(this, new EndPageRequestEvengArgs(page_context));
            }

            _pageRequestFlow.EndPageRequest(page_context);
            
        }

        /// <summary>
        /// Handle the exception
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="site">The site.</param>
        /// <param name="exception">The exception.</param>
        public void Error(HttpContextBase httpContext, Site site, Exception exception)
        {
            Contract.Requires(httpContext != null);
            Contract.Requires(exception != null);

            foreach (var item in _events)
            {
                var args = new ErrorEventArgs(httpContext, site, exception);
                item.Error(this, args);
                if (args.ExceptionHandled == true)
                {
                    break;
                }
            }
        }
        #endregion
    }
}
