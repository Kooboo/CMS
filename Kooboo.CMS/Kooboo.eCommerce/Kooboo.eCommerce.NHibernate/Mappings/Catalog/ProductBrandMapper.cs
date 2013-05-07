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
    /// Mapping ProductBrand
    /// </summary>
    public class ProductBrandMapper : ClassMapping<ProductBrand>
    {
        /// <summary>
        /// Mapping
        /// </summary>
        public ProductBrandMapper()
        {
            Id(pb => pb.Id, map => map.Generator(Generators.Identity));
            Property(pb => pb.Deleted);
            Property(pb => pb.IsFeaturedProduct);
            Property(pb => pb.DisplayOrder);

            ManyToOne(pb => pb.Brand, map => map.Column("BrandId"));
            ManyToOne(pb => pb.Product, map => map.Column("ProductId"));

        }
    }
}
