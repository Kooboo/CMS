#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.SiteFlow
{
    /// <summary>
    /// 封装页面的执行流程
    /// 默认实现的执行顺序可以根据不同的需要，通过重写ExecutePage达到修改的目的。
    /// 也可以根据需要加入自己需要执行的动作。
    /// </summary>
    public interface IPageRequestFlow
    {
        /// <summary>
        /// 查找页面
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="site">The site.</param>
        /// <returns></returns>
        Page MapPage(ControllerContext controllerContext, Site site);

        /// <summary>
        /// Creates the page context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="site">The site.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        Page_Context CreatePageContext(ControllerContext controllerContext, Site site, Page page);

        /// <summary>
        /// 执行页面逻辑
        /// </summary>
        ActionResult ExecutePage(Page_Context page_Context);
        /// <summary>
        /// 渲染输出页面
        /// </summary>
        void RenderPage(Page_Context page_Context, ActionResult actionResult);

        //void ExecutePlugins();
        //void ExecuteDataRules();
        //void InitializeHtmlMeta();
        //void ExecuteModuleAction();
    }
}
