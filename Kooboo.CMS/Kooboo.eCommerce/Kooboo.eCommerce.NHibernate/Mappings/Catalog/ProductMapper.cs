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
    /// Mapping Product
    /// </summary>
    public class ProductMapper : ClassMapping<Product>
    {
        /// <summary>
        /// Mapping
        /// </summary>
        public ProductMapper()
        {
            Id(product => product.Id, map => map.Generator(Generators.Identity));
            Property(product => product.Name, map => map.Length(255));
            Property(product => product.ShortDescription);
            Property(product => product.FullDescription);
            Property(product => product.ShowOnHomePage);
            Property(product => product.AllowCustomerReviews);
            Property(product => product.Published);
            Property(product => product.Deleted);
            Property(product => product.UtcCreationDate);
            Property(product => product.UtcUpdateDate);
            Property(product => product.Site, map => map.Length(255));
            Property(product => product.ProductType, map => map.Length(255));

            Bag(product => product.ProductCategories, bag =>
            {
                bag.Inverse(true);
                bag.Key(k => k.Column("ProductId"));
            }, a => a.OneToMany());

            Bag(product => product.ProductBrands, bag =>
            {
                bag.Inverse(true);
                bag.Key(k => k.Column("ProductId"));
            }, a => a.OneToMany());

            Bag(product => product.ProductImages, bag =>
            {
                bag.Inverse(true);
                bag.Key(k => k.Column("ProductId"));
            }, a => a.OneToMany());

            Bag(product => product.ProductReviews, bag =>
            {
                bag.Inverse(true);
                bag.Key(k => k.Column("ProductId"));
            }, a => a.OneToMany());
        }
    }
}
