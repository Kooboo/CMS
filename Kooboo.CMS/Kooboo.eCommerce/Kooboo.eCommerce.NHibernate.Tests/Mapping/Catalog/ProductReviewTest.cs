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
    /// Test ProductReview mapping
    /// </summary>
    [TestClass]
    public class ProductReviewTest : BaseTest
    {
        /// <summary>
        /// Test simple mapping
        /// </summary>
        [TestMethod]
        public void Test_Simple_Mapping_ProductReview()
        {
            var product = new Product
            {
                Name = "Product1",
                ShortDescription = "show",
                FullDescription = "full",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            var productReview = new ProductReview
            {
                Title = "review1",
                Product = product,
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            using (ISession session = MySessionFactory.OpenSession())
            {
                ITransaction transaction = session.BeginTransaction();
                session.Save(product);
                session.Save(productReview);
                transaction.Commit();
            }
            var productReview1 = MySessionFactory.OpenSession().Query<ProductReview>()
                .Where(it => it.Id == productReview.Id)
                .FirstOrDefault();
            Assert.AreEqual(productReview.Id, productReview1.Id);
            Assert.AreEqual(product.Id, productReview1.Product.Id);

            var product1 = MySessionFactory.OpenSession().Query<Product>()
                .Where(it => it.Id == product.Id)
                .FirstOrDefault();
            Assert.AreEqual(1, product1.ProductReviews.Count);
        }
    }
}
