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
    /// 用于在Content bus事件中判断当前的事件类型
    /// </summary>
    [Flags]
    public enum ContentAction
    {
        /// <summary>
        /// 内容添加成功后
        /// </summary>
        Add = 0x1,
        /// <summary>
        /// 内容修改成功后
        /// </summary>
        Update = 0x2,
        /// <summary>
        /// 内容删除成功后
        /// </summary>
        Delete = 0x4,
        /// <summary>
        /// 内容添加前
        /// </summary>
        PreAdd = 0x8,
        /// <summary>
        /// 内容修改前
        /// </summary>
        PreUpdate = 16,
        /// <summary>
        /// 内容删除前
        /// </summary>
        PreDelete = 32
    }
}
