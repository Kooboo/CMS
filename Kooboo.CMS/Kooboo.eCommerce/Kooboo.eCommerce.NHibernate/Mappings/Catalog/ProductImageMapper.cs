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
    /// Mapping ProductImage
    /// </summary>
    public class ProductImageMapper : ClassMapping<ProductImage>
    {
        /// <summary>
        /// Mapping
        /// </summary>
        public ProductImageMapper()
        {
            Id(pi => pi.Id, map => map.Generator(Generators.Identity));
            Property(pi => pi.Deleted);
            Property(pi => pi.ImageType, map => map.Length(255));
            Property(pi => pi.ImageUrl, map => map.Length(255));

            ManyToOne(pi => pi.Product, map => map.Column("ProductId"));
        }
    }
    
}
