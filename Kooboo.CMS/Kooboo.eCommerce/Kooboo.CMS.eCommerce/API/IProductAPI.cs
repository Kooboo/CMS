#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.eCommerce.Models.Catalog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.API
{
    #region IProductAPI
    /// <summary>
    /// 
    /// </summary>
    public interface IProductAPI
    {
        /// <summary>
        /// Sets the specified product.
        /// Change the product context.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        IProductAPI Set(Product product);

        #region ProductVariant management

        /// <summary>
        /// Gets the product variant.
        /// </summary>
        /// <returns></returns>
        ProductVariant[] GetProductVariants();

        /// <summary>
        /// Sets the product variant.
        /// </summary>
        /// <param name="productVariant">The product variant.</param>
        void AddProductVariant(ProductVariant productVariant);

        /// <summary>
        /// Deletes the product variant.
        /// </summary>
        void DeleteProductVariant(string variantUUID);
        #endregion

        #region Category management

        /// <summary>
        /// Set(Add/Update) the category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="isFeaturedProduct">if set to <c>true</c> [is featured product].</param>
        /// <param name="displayOrder">The display order.</param>
        void SetCategory(int categoryId, bool isFeaturedProduct, int displayOrder);

        /// <summary>
        /// Removes the category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        void RemoveCategory(int categoryId);
        #endregion

        #region Brand management
        /// <summary>
        /// Set(Add/Update) the brand.
        /// </summary>
        /// <param name="brandId">The brand id.</param>
        /// <param name="isFeaturedProduct">if set to <c>true</c> [is featured product].</param>
        /// <param name="displayOrder">The display order.</param>
        void SetBrand(int brandId, bool isFeaturedProduct, int displayOrder);

        /// <summary>
        /// Removes the brand.
        /// </summary>
        /// <param name="brandId">The brand id.</param>
        void RemoveBrand(int brandId);
        #endregion

        #region ProductImage management
        /// <summary>
        /// Sets(Add/Update) the product image.
        /// </summary>
        /// <param name="imageType">Type of the image.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileStream">The file stream.</param>
        void SetProductImage(string imageType, string fileName, Stream fileStream);
        /// <summary>
        /// Removes the product image.
        /// </summary>
        /// <param name="imageType">Type of the image.</param>
        void RemoveProductImage(string imageType);
        #endregion

        #region ProductReview management

        /// <summary>
        /// Adds the product review.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="reviewText">The review text.</param>
        /// <param name="rating">The rating.</param>
        void AddProductReview(string title, string reviewText, string rating);

        /// <summary>
        /// Removes the product review.
        /// </summary>
        /// <param name="reviewId">The review id.</param>
        void RemoveProductReview(int reviewId);
        #endregion

    }
    #endregion

    #region ProductAPI
    /// <summary>
    /// 
    /// </summary>
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProductAPI), ComponentLifeStyle.Transient)]
    public class ProductAPI : IProductAPI
    {
        private Product _product;
        /// <summary>
        /// Sets the specified product.
        /// Change the product context.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IProductAPI Set(Product product)
        {
            this._product = product;
            return this;
        }

        #region Variants
        /// <summary>
        /// Gets the product variants.
        /// </summary>
        /// <returns></returns>
        public ProductVariant[] GetProductVariants()
        {
            return new ProductVariant[0];
        }


        public void AddProductVariant(ProductVariant productVariant)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the product variants.
        /// </summary>        
        public void DeleteProductVariant(string variantUUID)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Set(Add/Update) the category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="isFeaturedProduct">if set to <c>true</c> [is featured product].</param>
        /// <param name="displayOrder">The display order.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SetCategory(int categoryId, bool isFeaturedProduct, int displayOrder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set(Add/Update) the brand.
        /// </summary>
        /// <param name="brandId">The brand id.</param>
        /// <param name="isFeaturedProduct">if set to <c>true</c> [is featured product].</param>
        /// <param name="displayOrder">The display order.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SetBrand(int brandId, bool isFeaturedProduct, int displayOrder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the brand.
        /// </summary>
        /// <param name="brandId">The brand id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveBrand(int brandId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets(Add/Update) the product image.
        /// </summary>
        /// <param name="imageType">Type of the image.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileStream">The file stream.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SetProductImage(string imageType, string fileName, Stream fileStream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the product image.
        /// </summary>
        /// <param name="imageType">Type of the image.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveProductImage(string imageType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the product review.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="reviewText">The review text.</param>
        /// <param name="rating">The rating.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void AddProductReview(string title, string reviewText, string rating)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the product review.
        /// </summary>
        /// <param name="reviewId">The review id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveProductReview(int reviewId)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region ProductAPIExtension
    /// <summary>
    /// 
    /// </summary>
    public static class ProductAPIExtension
    {
        /// <summary>
        /// APIs the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        public static IProductAPI API(this Product product)
        {
            return Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IProductAPI>().Set(product);
        }

        /// <summary>
        /// Gets the type of the product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        public static ProductType GetProductType(this Product product)
        {
            return new ProductType(product.ProductType);
        }
    }
    #endregion
}
