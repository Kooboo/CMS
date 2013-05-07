#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Catalog;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace Kooboo.eCommerce.NHibernate.Mappings.Catalog
{
    /// <summary>
    /// Mapping Brand
    /// </summary>
    public class BrandMapper : ClassMapping<Brand>
    {
        /// <summary>
        /// Mapping
        /// </summary>
        public BrandMapper()
        {
            Id(brand => brand.Id, map => map.Generator(Generators.Native));
            Property(brand => brand.Name, map => map.Length(255));
            Property(brand => brand.Description);
            Property(brand => brand.Image, map => map.Length(255));
            Property(brand => brand.PageSize);
            Property(brand => brand.Published);
            Property(brand => brand.Deleted);
            Property(brand => brand.DisplayOrder);
            Property(brand => brand.UtcCreationDate);
            Property(brand => brand.UtcUpdateDate);
            Property(brand => brand.Site, map => map.Length(255));
        }
    }
}
