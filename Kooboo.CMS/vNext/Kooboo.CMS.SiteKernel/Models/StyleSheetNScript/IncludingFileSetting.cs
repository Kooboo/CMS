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

namespace Kooboo.CMS.SiteKernel.Models
{
    /// <summary>
    /// 被引用到站点或者页面的set/单个文件的设置
    /// </summary>
    public class IncludingFileSetting
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        public string SetName { get; set; }
        /// <summary>
        /// 绝对文件名
        /// </summary>
        public string AbsoluteFileName { get; set; }
        /// <summary>
        /// HTML标签的属性
        /// </summary>
        public Dictionary<string, string> HtmlAttributes { get; set; }
        /// <summary>
        /// Async属性值
        /// </summary>
        public bool Async { get; set; }
        /// <summary>
        /// w3c 的defer属性定义
        /// </summary>
        public bool Defer { get; set; }
    }
}
