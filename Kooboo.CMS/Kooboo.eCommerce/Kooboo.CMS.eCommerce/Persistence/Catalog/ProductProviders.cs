#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Persistence.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductProviders
    {
        /// <summary>
        /// Gets the category provider.
        /// </summary>
        /// <value>
        /// The category provider.
        /// </value>
        public ICategoryProvider CategoryProvider { get; private set; }

        /// <summary>
        /// Gets the brand provider.
        /// </summary>
        /// <value>
        /// The brand provider.
        /// </value>
        public IProductBrandProvider BrandProvider { get; private set; }

        /// <summary>
        /// Gets the product category provider.
        /// </summary>
        /// <value>
        /// The product category provider.
        /// </value>
        public IProductCategoryProvider ProductCategoryProvider { get; private set; }

        /// <summary>
        /// Gets the product image provider.
        /// </summary>
        /// <value>
        /// The product image provider.
        /// </value>
        public IProductImageProvider ProductImageProvider { get; private set; }

        /// <summary>
        /// Gets the product query provider.
        /// </summary>
        /// <value>
        /// The product query provider.
        /// </value>
        public IProductQueryProvider ProductQueryProvider { get; private set; }


        /// <summary>
        /// Gets the product review provider.
        /// </summary>
        /// <value>
        /// The product review provider.
        /// </value>
        public IProductReviewProvider ProductReviewProvider { get; private set; }

        /// <summary>
        /// Gets or sets the product variant provider.
        /// </summary>
        public IProductVariantProvider ProductVariantProvider { get; private set; }

        /// <summary>
        /// Gets the related product provider.
        /// </summary>
        /// <value>
        /// The related product provider.
        /// </value>
        public IRelatedProductProvider RelatedProductProvider { get; private set; }

    }
}
