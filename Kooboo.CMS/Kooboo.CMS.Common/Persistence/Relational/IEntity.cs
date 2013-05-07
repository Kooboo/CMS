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

namespace Kooboo.CMS.Common.Persistence.Relational
{
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>        
        int Id { get; set; }
    }
}
