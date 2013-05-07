#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Relational;
using Kooboo.CMS.eCommerce.Models.Customers;
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using System;

namespace Kooboo.CMS.eCommerce.Models.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProductReview : CustomerComment, IRelationEntity, ISiteObject
    {
        #region .ctor
        public ProductReview()
        {
            UtcCreationDate = DateTime.UtcNow;
            UtcUpdateDate = DateTime.UtcNow;
        }
        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the review text
        /// </summary>
        public virtual string ReviewText { get; set; }

        /// <summary>
        /// Review rating
        /// </summary>
        public virtual int Rating { get; set; }

        /// <summary>
        /// Review helpful votes total
        /// </summary>
        public virtual int HelpfulYesTotal { get; set; }

        /// <summary>
        /// Review not helpful votes total
        /// </summary>
        public virtual int HelpfulNoTotal { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual string Site { get; set; }
        #endregion

        #region Relation
        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Product Product { get; set; }

        #endregion
    }

    public partial class ProductReview
    {
        public virtual int? ProductId
        {
            get
            {
                return (this.Product == null) ? default(int) : new Nullable<int>(this.Product.Id);
            }
            set
            {
                this.Product = value == null ? null : Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IProductProvider>().QueryById(value.Value);
            }
        }
    }
}
