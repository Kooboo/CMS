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

namespace Kooboo.CMS.SiteKernel.SiteFlow
{
    /// <summary>
    /// 页面处理流程事件
    /// </summary>
    public interface PageRequestFlowEvents
    {
        /// <summary>
        /// 查找页面之前触发
        /// </summary>
        void PreMapPage(object sender, PreMapPageEventArgs args);

        /// <summary>
        /// 查找页面之后触发
        /// NOTE：
        /// 适用场景：A/B page test.
        /// </summary>
        void PostMapPage(object sender, PostMapPageEventArgs args);

        void PreCreatePageContext(object sender, PreCreatePageContextEventArgs args);

        void PostCreatePageContext(object sender, PostCreatePageContextEventArgs args);

        /// <summary>
        /// 执行页面之前触发
        /// 适用场景：检查是否缓存并输出缓存版本， SSL检查，页面权限验证
        /// </summary>
        void PreExecutePage(object sender, PreExecutePageEventArgs args);

        /// <summary>
        /// 页面执行之后触发
        /// NOTE：如果页面直接输出缓存版本，这个事件可能没有执行。
        /// </summary>
        void PostExecutePage(object sender, PostExecutePageEventArgs args);

        /// <summary>
        /// 页面渲染之前触发
        /// NOTE：如果页面直接输出缓存版本，这个事件可能没有执行。
        /// </summary>
        void PreRenderPage(object sender, PreRenderPageEventArgs args);

        /// <summary>
        /// 页面渲染之后触发
        /// NOTE：如果页面直接输出缓存版本，这个事件可能没有执行。
        /// </summary>
        void PostRenderPage(object sender, PostRenderPageEventArgs args);

        /// <summary>
        /// 异常发生时触发
        /// </summary>
        void Error(object sender, ErrorEventArgs args);

        /// <summary>
        /// 页面请求结束时触发
        /// 适用场景：
        /// * 当页面被缓存的时候，Plugin不会被执行。统计内容的阅读次数就不能通过Plugin来计算，这种情况下就可以用这个事件来做统计。
        /// * 可以用来清空Session, Cookie等临时内容
        /// 
        /// </summary>
        void EndPageRequest(object sender, EndPageRequestEvengArgs args);
    }
}
