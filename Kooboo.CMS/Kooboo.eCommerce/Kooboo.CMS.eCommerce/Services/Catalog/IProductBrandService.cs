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
    public interface IProductBrandService
    {
        IProductBrandProvider Provider { get; }
        void Add(ProductBrand productBrand);
        void Update(ProductBrand productBrand);
        void Delete(ProductBrand productBrand);
    }
}
