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
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Common.Extension.TopBar
{
    public interface ITopBarPlugin : IApplyTo
    {
        string Name { get; set; }

        string DisplayText { get; set; }

        Type OptionType { get; set; }

        string IconClass { get; set; }

        string GroupName { get; set; }

        int Order { get; set; }

        /// <summary>
        /// CommandTarget==null 是，托管提交动作
        /// 否则，生成一个提交地址
        /// </summary>
        MvcRoute GetMvcRoute(ControllerContext controllerContext);

        /// <summary>
        /// 获得将要生成的Button的HTML 标签
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        IDictionary<string, object> HtmlAttributes(ControllerContext controllerContext);

        /// <summary>
        /// 某条数据是否显示Button
        /// </summary>
        /// <param name="dataItem"></param>
        /// <returns></returns>
        bool IsVisibleFor(object dataItem);

        /// <summary>
        /// 执行Topbar plugin
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ActionResult Execute(TopBarContext context);
    }
}
