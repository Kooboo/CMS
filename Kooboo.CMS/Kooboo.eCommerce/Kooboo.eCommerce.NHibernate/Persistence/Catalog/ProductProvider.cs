#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.eCommerce.Persistence;
using Kooboo.CMS.eCommerce.Persistence.Catalog;

namespace Kooboo.eCommerce.NHibernate.Persistence.Catalog
{
    [Dependency(typeof(IProductProvider))]
    public class ProductProvider : ProviderBase<Product>, IProductProvider
    {
        public ProductProvider(IDbContextFactory dbContextFactory)
            : base(dbContextFactory)
        { 
        
        }
    }
}
