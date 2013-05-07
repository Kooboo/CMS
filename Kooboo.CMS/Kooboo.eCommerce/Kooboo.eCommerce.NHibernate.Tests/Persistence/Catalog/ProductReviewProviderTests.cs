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
using Kooboo.eCommerce.NHibernate.Persistence.Catalog;
using Kooboo.CMS.eCommerce.Models.Catalog;

namespace Kooboo.eCommerce.NHibernate.Tests.Persistence.Catalog
{
    [TestClass]
    public class ProductReviewProviderTests : BaseTest
    {
        ProductReviewProvider provider = new ProductReviewProvider(new NoDI_NHibernateContextFactory());
        /// <summary>
        /// Test_CreateQuery_ProductReviewProvider
        /// </summary>
        [TestMethod]
        public void Test_CreateQuery_ProductReviewProvider()
        {
            var query = provider.CreateQuery();

            Assert.IsNotNull(query);
            Console.WriteLine("CreateQuery OK!");
        }

        /// <summary>
        /// Test_Add_QueryById_Update_Delete_ProductReviewProvider
        /// </summary>
        [TestMethod]
        public void Test_Add_QueryById_Update_Delete_ProductReviewProvider()
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
                UtcCreationDate=DateTime.UtcNow,
                UtcUpdateDate=DateTime.UtcNow
            };

            //Add brand
            new ProductProvider(new NoDI_NHibernateContextFactory()).Add(product);
            provider.Add(productReview);
            Console.WriteLine("Add OK!");

            //QueryBy
            var productReviewQueryBy = provider.QueryById(productReview.Id);
            Assert.IsNotNull(productReviewQueryBy);
            Console.WriteLine("QueryBy OK!");

            //update
            productReview.Title = "review";
            provider.Update(productReview);
            var productReviewUpdate = provider.QueryById(productReview.Id);
            Assert.AreEqual("review", productReviewUpdate.Title);
            Console.WriteLine("Update OK!");

            //Delete
            provider.Delete(productReview);
            var productReviewDelete = provider.QueryById(productReview.Id);
            Assert.IsNull(productReviewDelete);
            Console.WriteLine("Delete OK!");

        }
    }
}
