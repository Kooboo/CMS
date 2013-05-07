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
    public interface ISiteObject
    {
        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        string Site { get; set; }
    }
}
