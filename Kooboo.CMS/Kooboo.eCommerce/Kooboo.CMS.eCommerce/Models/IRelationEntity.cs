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
    /// 以关系型数据库为存储目标的对象。
    /// </summary>
    public interface IRelationEntity : Kooboo.CMS.Common.Persistence.Relational.IEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IRelationEntity" /> is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if deleted; otherwise, <c>false</c>.
        /// </value>
        bool Deleted { get; set; }
    }
}
