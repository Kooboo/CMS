#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.eCommerce.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Services.Catalog.Templates
{
    public static class ProductTypeTemplateExtensions
    {
        #region GetTemplatePath
        public static ProductTypeTemplatePath GetTemplatePath(this ProductType productType)
        {
            var commerceDataDir = EngineContext.Current.Resolve<ICommerceDataDir>();
            return new ProductTypeTemplatePath(commerceDataDir, productType);
        }
        #endregion
    }
}
