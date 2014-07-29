#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.SiteFlow.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.SiteFlow
{
    /// <summary>
    /// 站点请求事件
    /// </summary>
    public interface ISiteRequestFlowEvents
    {
        /// <summary>
        /// 进入Kooboo CMS请求通道事件
        /// </summary>
        void BeginSiteRequest(object sender, BeginSiteRequestEventArgs args);

        /// <summary>
        /// 在查找站点之前触发
        /// </summary>
        void PreMapSite(object sender, PreMapSiteEventArgs args);

        /// <summary>
        /// 在查找站点之后触发
        /// 适用场景：A/B site test
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostMapSiteEventArgs"/> instance containing the event data.</param>
        void PostMapSite(object sender, PostMapSiteEventArgs args);

        /// <summary>
        /// 查找Handler之前触发
        /// 适用场景：Url Redirect, Robots.txt
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PreMapRequestHandlerEventArgs"/> instance containing the event data.</param>
        void PreMapRequestHandler(object sender, PreMapRequestHandlerEventArgs args);

        /// <summary>
        /// 查找到Handler之后触发
        /// </summary>
        void PostMapRequestHandler(object sender, PostMapRequestHandlerEventArgs args);

        /// <summary>
        /// 执行Handler之前触发
        /// </summary>
        /// <param name="send">The send.</param>
        /// <param name="args">The <see cref="PreExecuteRequestHandlerEventArgs"/> instance containing the event data.</param>
        void PreExecuteRequestHandler(object sender, PreExecuteRequestHandlerEventArgs args);

        /// <summary>
        /// 执行Handler之后触发
        /// </summary>
        void PostExecuteRequestHandler(object sender, PostExecuteRequestHandlerEventArgs args);

        /// <summary>
        /// 异常发生时触发
        /// 适用场景： 记录异常日志，友好异常也没重定向
        /// </summary>
        void Error(object sender, ErrorEventArgs args);

        /// <summary>
        /// EndSiteRequest执行之后触发
        /// 为了与HttpApplication的EndRequest区分，这个事件可以在Mvc的ActionResult被执行的时候触发
        /// 如果有缓存页面的话，应该在缓存内容被输出后触发
        /// 有这个事件可以
        /// </summary>
        void EndSiteRequest(object sender, EndSiteRequestEventArgs args);
    }
}
