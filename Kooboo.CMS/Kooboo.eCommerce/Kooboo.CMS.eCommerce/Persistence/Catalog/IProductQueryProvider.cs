#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Persistence.Catalog
{
    /// <summary>
    /// 专用于产品搜索功能
    /// </summary>
    public interface IProductQueryProvider
    {
        /// <summary>
        /// Searches the products.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="brandId">The brand id.</param>
        /// <param name="featuredProducts">if set to <c>true</c> [featured products].</param>
        /// <param name="priceMin">The price min.</param>
        /// <param name="priceMax">The price max.</param>
        /// <param name="keywords">The keywords.</param>
        /// <param name="searchDescriptions">if set to <c>true</c> [search descriptions].</param>
        /// <param name="siteName">Name of the site.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="showHidden">if set to <c>true</c> [show hidden].</param>
        /// <returns></returns>
        IPagedList<Product> SearchProducts(int? categoryId, int? brandId, bool? featuredProducts,
           decimal? priceMin, decimal? priceMax, string keywords, bool searchDescriptions, string siteName, ProductSortBy sort,
           int pageIndex, int pageSize, bool showHidden = false);
    }
}
