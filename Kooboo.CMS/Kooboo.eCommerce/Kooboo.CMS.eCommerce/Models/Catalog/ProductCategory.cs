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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Models.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProductCategory : IRelationEntity
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
        /// Get or set whenever the productcategory has been delete
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

        /// <summary>
        /// Gets the category
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Product Product { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProductCategory
    {
        /// <summary>
        /// Get or set the CategoryId
        /// </summary>
        public virtual int? CategoryId
        {
            get
            {
                return (this.Category == null) ? default(int) : new Nullable<int>(this.Category.Id);
            }
            set
            {
                this.Category = value == null ? null : Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<ICategoryProvider>().QueryById(value.Value);
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
