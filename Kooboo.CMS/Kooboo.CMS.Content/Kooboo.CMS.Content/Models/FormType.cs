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

namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// 模板类型
    /// </summary>
    [Flags]
    public enum FormType
    {
        /// <summary>
        /// 列表
        /// </summary>
        Grid = 1,
        /// <summary>
        /// 新建
        /// </summary>
        Create = 2,
        /// <summary>
        /// 修改
        /// </summary>
        Update = 4,
        /// <summary>
        /// 类别选择
        /// </summary>
        Selectable = 8,
        /// <summary>
        /// 用于前台的列表显示
        /// </summary>
        List = 16,
        /// <summary>
        /// 用于前台的内容详细显示
        /// </summary>
        Detail = 32
    }
}
