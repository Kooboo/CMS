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
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using Kooboo.CMS.eCommerce.EventBus;
using Kooboo.CMS.eCommerce.Models.Catalog;

namespace Kooboo.CMS.eCommerce.Services.Catalog
{
    [Dependency(typeof(IProductCategoryService))]
    public class ProductCategoryService : RelationalServiceBase<ProductCategory,IProductCategoryProvider>,IProductCategoryService
    {
        #region .ctor
        public ProductCategoryService(IProductCategoryProvider provider, IEventPublisher eventPublisher)
            : base(provider, eventPublisher)
        { 
        }
        #endregion
    }
}
