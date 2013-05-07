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
    /// Mapping RelatedProduct
    /// </summary>
    public class RelatedProductMapper : ClassMapping<RelatedProduct>
    {
        /// <summary>
        /// Mapping
        /// </summary>
        public RelatedProductMapper()
        {
            Id(rp => rp.Id, map => map.Generator(Generators.Identity));
            Property(rp => rp.Deleted);
            Property(rp => rp.DisplayOrder);

            ManyToOne(rp => rp.Product1, map => map.Column("Product1Id"));
            ManyToOne(rp => rp.Product2, map => map.Column("Product2Id"));
        }
    }
}
