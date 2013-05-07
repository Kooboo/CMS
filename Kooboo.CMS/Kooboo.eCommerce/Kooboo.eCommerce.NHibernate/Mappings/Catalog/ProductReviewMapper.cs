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
    /// Mapping ProductReview
    /// </summary>
    public class ProductReviewMapper : ClassMapping<ProductReview>
    {
        /// <summary>
        /// Mapping
        /// </summary>
        public ProductReviewMapper()
        {
            Id(pr => pr.Id, map => map.Generator(Generators.Identity));
            Property(pr => pr.Deleted);
            Property(pr => pr.Title, map => map.Length(255));
            Property(pr => pr.ReviewText);
            Property(pr => pr.Rating);
            Property(pr => pr.HelpfulYesTotal);
            Property(pr => pr.HelpfulNoTotal);
            Property(pr => pr.Site, map => map.Length(255));
            Property(pr => pr.IpAddress);
            Property(pr => pr.IsApproved);
            Property(pr => pr.UtcCreationDate);
            Property(pr => pr.UtcUpdateDate);
            //TODO:
            //ManyToOne(pr => pr.Customer, map => map.Column("CusromerId"));
            ManyToOne(pr => pr.Product, map => map.Column("ProductId"));
        }
    }
}
