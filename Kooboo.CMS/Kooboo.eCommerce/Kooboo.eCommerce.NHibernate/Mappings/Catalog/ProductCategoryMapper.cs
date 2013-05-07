#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Kooboo.CMS.eCommerce.Models.Catalog;

namespace Kooboo.eCommerce.NHibernate.Mappings.Catalog
{
    /// <summary>
    /// Mapping ProductCategory
    /// </summary>
    public class ProductCategoryMapper : ClassMapping<ProductCategory>
    {
        /// <summary>
        /// Mapping
        /// </summary>
        public ProductCategoryMapper()
        {
            Id(pc=>pc.Id,map=>map.Generator(Generators.Identity));
            Property(pc => pc.Deleted);
            Property(pc=>pc.IsFeaturedProduct);
            Property(pc=>pc.DisplayOrder);
            
            ManyToOne(pc=>pc.Category,map=>map.Column("CategoryId"));
            ManyToOne(pc=>pc.Product,map=>map.Column("ProductId"));
        }
    }
}
