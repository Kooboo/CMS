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

namespace Kooboo.CMS.eCommerce.Models
{
    /// <summary>
    /// 以非关系型数据库为存储目标的对象。
    /// </summary>
    public interface INonRelationEntity
    {
        /// <summary>
        /// Gets or sets the UUID. 
        /// 本地生成的UUID作为主键
        /// </summary>
        /// <value>
        /// The UUID.
        /// </value>
        string UUID { get; set; }
    }
}
