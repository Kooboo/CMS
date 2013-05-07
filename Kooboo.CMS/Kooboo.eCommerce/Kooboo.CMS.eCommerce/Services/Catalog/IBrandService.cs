#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Services.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBrandService
    {
        IBrandProvider Provider { get; }
        void Add(Brand brand);
        void Update(Brand brand);
        void Delete(Brand brand);
    }
}
