#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.CMS.Common.Persistence;
using System;
using System.Collections.Generic;
namespace Kooboo.CMS.eCommerce.Models.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Product : IRelationEntity, ISiteObject
    {
        #region .ctor
        /// <summary>
        /// 
        /// </summary>
        public Product()
        {
            UtcCreationDate = DateTime.UtcNow;
            UtcUpdateDate = DateTime.UtcNow;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        public virtual string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        public virtual string FullDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the product on home page
        /// </summary>
        public virtual bool ShowOnHomePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product allows customer reviews
        /// </summary>
        public virtual bool AllowCustomerReviews { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public virtual bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public virtual bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the UTC creation date.
        /// </summary>
        /// <value>
        /// The UTC creation date.
        /// </value>
        public virtual DateTime UtcCreationDate { get; set; }

        /// <summary>
        /// Gets or sets the UTC update date.
        /// </summary>
        /// <value>
        /// The UTC update date.
        /// </value>
        public virtual DateTime UtcUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual string Site { get; set; }

        /// <summary>
        /// Gets the type of the product.
        /// </summary>
        /// <value>
        /// The type of the product.
        /// </value>
        public virtual string ProductType { get; set; }
        #endregion

        #region Relations
        private ICollection<ProductCategory> _productCategories;
        /// <summary>
        /// Gets or sets the product categories.
        /// </summary>
        /// <value>
        /// The product categories.
        /// </value>
        public virtual ICollection<ProductCategory> ProductCategories
        {
            get
            {
                return _productCategories ?? (_productCategories = new List<ProductCategory>());
            }
            protected set { _productCategories = value; }
        }


        private ICollection<ProductBrand> _productBrands;
        /// <summary>
        /// Gets or sets the product categories.
        /// </summary>
        /// <value>
        /// The product categories.
        /// </value>
        public virtual ICollection<ProductBrand> ProductBrands
        {
            get
            {
                return _productBrands ?? (_productBrands = new List<ProductBrand>());
            }
            protected set { _productBrands = value; }
        }

        private ICollection<ProductImage> _productImages;

        /// <summary>
        /// Gets or sets the product images.
        /// </summary>
        /// <value>
        /// The product images.
        /// </value>
        public virtual ICollection<ProductImage> ProductImages
        {
            get
            {
                return _productImages ?? (_productImages = new List<ProductImage>());
            }
            protected set { _productImages = value; }
        }

        private ICollection<ProductReview> _productReviews;
        /// <summary>
        /// Gets or sets the product images.
        /// </summary>
        /// <value>
        /// The product images.
        /// </value>
        public virtual ICollection<ProductReview> ProductReviews
        {
            get
            {
                return _productReviews ?? (_productReviews = new List<ProductReview>());
            }
            protected set { _productReviews = value; }
        }
        #endregion
    }
}
