#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Services.Catalog.Templates
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProductTypeTemplateGenerator
    {
        string GenerateCustomeFieldsEditForm(ProductType productType);
        string GenerateVariantGridForm(ProductType productType);
        string GenerateVariantCreateForm(ProductType productType);
        string GenerateVariantEditForm(ProductType productType);
    }
}
