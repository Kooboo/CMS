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
    /// 样式或者脚本文件
    /// </summary>
    public class File
    {
        public string FileName { get; set; }

        /// <summary>
        /// 文件在站点内的绝对路径，比如样式文件在站点内的相对目录就是：
        /// Styles/Style1.css
        /// Styles/folder1/form.css
        /// 站点内的脚本文件就是：
        /// Scripts/script1.js
        /// Scripts/folder1/script2.js
        /// 
        /// </summary>
        public string AbsoluteName { get; set; }

        /// <summary>
        /// 文件内容
        /// 并不会每次都读取文件内容出来。只有当获取单个文件或者列表查询时使用 includeContent=true 时才会读取文件内容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 文件所在的目录名，比如：
        /// /
        /// /folder1
        /// /folder1/folder2
        /// </summary>
        public string Directory { get; set; }
    }
}
