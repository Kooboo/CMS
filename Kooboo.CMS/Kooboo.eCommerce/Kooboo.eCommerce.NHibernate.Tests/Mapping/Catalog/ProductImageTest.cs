#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Linq;
using Kooboo.CMS.eCommerce.Models.Catalog;

namespace Kooboo.eCommerce.NHibernate.Tests.Catalog
{
    /// <summary>
    /// Test ProductImage mapping 
    /// </summary>
    [TestClass]
    public class ProductImageTest : BaseTest
    {
        /// <summary>
        /// Test simple mapping
        /// </summary>
        [TestMethod]
        public void Test_Simple_Mapping_ProductImage()
        {
            var product = new Product
            {
                Name = "Product1",
                ShortDescription = "show",
                FullDescription = "full",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            var productImage = new ProductImage {
                ImageType="jpg",
                ImageUrl="image/1.jpg",
                Product=product
            };
            using (ISession session = MySessionFactory.OpenSession())
            {
                ITransaction transaction = session.BeginTransaction();
                session.Save(product);
                session.Save(productImage);
                transaction.Commit();
            }
            var productImage1 = MySessionFactory.OpenSession().Query<ProductImage>()
                .Where(it => it.Id == productImage.Id)
                .FirstOrDefault();
            Assert.AreEqual(productImage.Id, productImage1.Id);
            Assert.AreEqual(product.Id,productImage1.Product.Id);

            var product1 = MySessionFactory.OpenSession().Query<Product>()
                .Where(it => it.Id == product.Id)
                .FirstOrDefault();
            Assert.AreEqual(1, product1.ProductImages.Count);

        }
    }
}
