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

namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// 什么类型的内容
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown,
        /// <summary>
        /// 文本内容
        /// </summary>
        Text,
        /// <summary>
        /// media二进制文件
        /// </summary>
        Media
    }
}
