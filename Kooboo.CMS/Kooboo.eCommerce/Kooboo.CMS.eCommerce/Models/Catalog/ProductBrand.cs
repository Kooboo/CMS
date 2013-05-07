#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.CMS.eCommerce.Persistence.Catalog;
using System;
namespace Kooboo.CMS.eCommerce.Models.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProductBrand : IRelationEntity
    {
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
        /// Get or set whenever the productbrand has been delete
        /// </summary>
        public virtual bool Deleted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the product is featured
        /// </summary>
        public virtual bool IsFeaturedProduct { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public virtual int DisplayOrder { get; set; }
        #endregion

        #region Relation
        /// <summary>
        /// Gets or sets the brand.
        /// </summary>
        /// <value>
        /// The brand.
        /// </value>
        public virtual Brand Brand { get; set; }

        /// <summary>
        /// Gets or sets the product
        /// </summary>
        public virtual Product Product { get; set; }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProductBrand
    {
        /// <summary>
        /// Get or set the BrandId
        /// </summary>
        public virtual int? BrandId
        {
            get
            {
                return (this.Brand == null) ? default(int) : new Nullable<int>(this.Brand.Id);
            }
            set
            {
                this.Brand = value == null ? null : Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IBrandProvider>().QueryById(value.Value);
            }
        }

        /// <summary>
        /// Get or set the ProductId
        /// </summary>
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
