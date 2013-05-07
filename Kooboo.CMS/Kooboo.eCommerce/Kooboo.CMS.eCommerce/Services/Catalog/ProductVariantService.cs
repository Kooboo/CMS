#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.EventBus;
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Services.Catalog
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProductVariantService))]
    public class ProductVariantService : Non_RelationalServiceBase<ProductVariant, IProductVariantProvider>, IProductVariantService
    {
        #region .ctor
        public ProductVariantService(IProductVariantProvider provider, IEventPublisher eventPublisher)
            : base(provider, eventPublisher)
        {
        }
        #endregion
    }
}
