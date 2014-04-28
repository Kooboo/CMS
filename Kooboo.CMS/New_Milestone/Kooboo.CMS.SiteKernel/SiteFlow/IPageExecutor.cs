#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.SiteKernel.SiteFlow
{
    /// <summary>
    /// 封装页面的执行流程
    /// 默认实现的执行顺序可以根据不同的需要，通过重写ExecutePage达到修改的目的。
    /// 也可以根据需要加入自己需要执行的动作。
    /// </summary>
    public interface IPageExecutor
    {
        void Execute(HttpContextBase httpContext);
        void ExecutePlugins();
        void ExecuteDataRules();
        void InitializeHtmlMeta();
        void ExecuteModuleAction();
    }
}
