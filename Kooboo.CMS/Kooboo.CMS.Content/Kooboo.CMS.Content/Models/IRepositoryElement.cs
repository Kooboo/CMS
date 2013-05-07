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
    /// 属于内容数据库的对象接口
    /// </summary>
    public interface IRepositoryElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        Repository Repository { get; set; }
    }
}
