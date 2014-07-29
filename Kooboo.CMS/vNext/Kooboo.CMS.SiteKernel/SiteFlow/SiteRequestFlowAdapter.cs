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

namespace Kooboo.CMS.SiteKernel.SiteFlow
{
    public class SiteRequestFlowAdapter
    {
        #region .ctor
        ISiteRequestFlow _siteRequestFlow;
        ISiteRequestFlowEvents[] _events;
        public SiteRequestFlowAdapter(ISiteRequestFlow siteRequestFlow, ISiteRequestFlowEvents[] events)
        {
            Contract.Requires(siteRequestFlow != null);
            _siteRequestFlow = siteRequestFlow;
            _events = events ?? new ISiteRequestFlowEvents[0];
        }
        #endregion

        #region Properties
        public ISiteRequestFlow SiteRequestFlow
        {
            get { return _siteRequestFlow; }
        }
        public ISiteRequestFlowEvents[] EventHandlers
        {
            get { return _events; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// 根据需要创建一个自定义的HttpContext
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        public HttpContextBase RenewHttpContext(HttpContextBase httpContext)
        {
            Contract.Requires(httpContext != null);
            foreach (var item in _events)
            {
                item.BeginSiteRequest(this, new BeginSiteRequestEventArgs(httpContext));
            }
            var newHttpContext = _siteRequestFlow.RenewHttpContext(httpContext);
            return newHttpContext;
        }

        /// <summary>
        /// 根据当前的请求地址，查找对应的站点。
        /// 1. 预览模式   dev~ + 站点名称
        /// 2. 正式访问   域名 + site path
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        public Site MapSite(HttpContextBase httpContext)
        {
            Contract.Requires(httpContext != null);
            foreach (var item in _events)
            {
                item.PreMapSite(this, new PreMapSiteEventArgs(httpContext));
            }

            var site = _siteRequestFlow.MapSite(httpContext);

            foreach (var item in _events)
            {
                var args = new PostMapSiteEventArgs(httpContext, site);
                item.PostMapSite(this, args);
                //替换事件中可能重新查找的站点。
                site = args.Site;
            }

            return site;
        }

        /// <summary>
        /// 查找处理请求的Handler
        /// 比如：PageRequestHandler
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        public IRequestHandler MapRequestHandler(HttpContextBase httpContext, Site site)
        {
            Contract.Requires(httpContext != null);
            Contract.Requires(site != null);

            foreach (var item in _events)
            {
                item.PreMapRequestHandler(this, new PreMapRequestHandlerEventArgs(httpContext, site));
            }

            var requestHandler = _siteRequestFlow.MapRequestHandler(httpContext, site);

            foreach (var item in _events)
            {
                item.PostMapRequestHandler(this, new PostMapRequestHandlerEventArgs(httpContext, site, requestHandler));
            }
            return requestHandler;
        }

        /// <summary>
        /// 执行Handler
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public void ExecuteRequestHandler(HttpContextBase httpContext, Site site, IRequestHandler requestHandler)
        {
            Contract.Requires(httpContext != null);
            Contract.Requires(site != null);
            Contract.Requires(requestHandler != null);

            foreach (var item in _events)
            {
                item.PreExecuteRequestHandler(this, new PreExecuteRequestHandlerEventArgs(httpContext, site, requestHandler));
            }

            _siteRequestFlow.ExecuteRequestHandler(httpContext, site, requestHandler);

            foreach (var item in _events)
            {
                item.PostExecuteRequestHandler(this, new PostExecuteRequestHandlerEventArgs(httpContext, site, requestHandler));
            }
        }

        /// <summary>
        /// 站点请求结束
        /// </summary>
        /// <param name="httpContext"></param>
        public void EndSiteRequest(HttpContextBase httpContext, Site site)
        {
            Contract.Requires(httpContext != null);
            Contract.Requires(site != null);

            foreach (var item in _events)
            {
                item.EndSiteRequest(this, new EndSiteRequestEventArgs(httpContext, site));
            }

            _siteRequestFlow.EndSiteRequest(httpContext, site);
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
