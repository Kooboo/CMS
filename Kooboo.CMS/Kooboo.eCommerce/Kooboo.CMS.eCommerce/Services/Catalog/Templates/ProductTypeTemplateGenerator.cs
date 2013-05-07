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
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProductTypeTemplateGenerator))]
    public class ProductTypeTemplateGenerator : IProductTypeTemplateGenerator
    {
        #region Methods
        public virtual string GenerateCustomeFieldsEditForm(ProductType productType)
        {
            throw new NotImplementedException();
        }

        public virtual string GenerateVariantGridForm(ProductType productType)
        {
            throw new NotImplementedException();
        }

        public virtual string GenerateVariantCreateForm(ProductType productType)
        {
            throw new NotImplementedException();
        }

        public virtual string GenerateVariantEditForm(ProductType productType)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
