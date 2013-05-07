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
using Kooboo.CMS.eCommerce.Persistence;
using Moq;

namespace Kooboo.eCommerce.NHibernate.Tests.Persistence.Catalog
{
    [TestClass]
    public class ProductImageProviderTests : BaseTest
    {
        ProductImageProvider provider;
        public ProductImageProviderTests()
        {
            Mock<IEntityFileProvider> entityFileProvider = new Mock<IEntityFileProvider>();
            provider = new ProductImageProvider(new NoDI_NHibernateContextFactory(), entityFileProvider.Object);
        }
        /// <summary>
        /// Test_CreateQuery_ProductImageProvider
        /// </summary>
        [TestMethod]
        public void Test_CreateQuery_BrandProvider()
        {
            var query = provider.CreateQuery();

            Assert.IsNotNull(query);
            Console.WriteLine("CreateQuery OK!");
        }

        /// <summary>
        /// Test_Add_QueryById_Update_Delete_ProductImageProvider
        /// </summary>
        [TestMethod]
        public void Test_Add_QueryById_Update_Delete_BrandProvider()
        {
            var product = new Product
            {
                Name = "Product1",
                ShortDescription = "show",
                FullDescription = "full",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            var productImage = new ProductImage
            {
                ImageType = "jpg",
                ImageUrl = "image/1.jpg",
                Product = product
            };

            //Add brand
            new ProductProvider(new NoDI_NHibernateContextFactory()).Add(product);
            provider.Add(productImage);
            Console.WriteLine("Add OK!");

            //QueryBy
            var productImageQueryBy = provider.QueryById(productImage.Id);
            Assert.IsNotNull(productImageQueryBy);
            Console.WriteLine("QueryBy OK!");

            //update
            productImage.ImageType = "type";
            provider.Update(productImage);
            var productImageUpdate = provider.QueryById(productImage.Id);
            Assert.AreEqual("type", productImageUpdate.ImageType);
            Console.WriteLine("Update OK!");

            //Delete
            provider.Delete(productImage);
            var productImageDelete = provider.QueryById(productImage.Id);
            Assert.IsNull(productImageDelete);
            Console.WriteLine("Delete OK!");

        }
    }
}
