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
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.eCommerce.NHibernate.Persistence.Catalog;
using Moq;
using Kooboo.CMS.eCommerce.Persistence;

namespace Kooboo.eCommerce.NHibernate.Tests.Persistence.Catalog
{
    [TestClass]
    public class ProductBrandProviderTests : BaseTest
    {
        ProductBrandProvider provider = new ProductBrandProvider(new NoDI_NHibernateContextFactory());
        [TestMethod]
        public void Test_CreateQuery_ProductBrandProvider()
        {
            var query = provider.CreateQuery();

            Assert.IsNotNull(query);
            Console.WriteLine("CreateQuery OK!");
        }

        [TestMethod]
        public void Test_Add_QueryById_Update_Delete_ProductBrandProvider()
        {
            var product = new Product
            {
                Name = "Product1",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            var brand = new Brand
            {
                Name = "secondBrand",
                Description = "Hello description!",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            var productbrand = new ProductBrand
            {
                IsFeaturedProduct = false,
                DisplayOrder = 1,
                Brand = brand,
                Product = product
            };
            //Add
            new ProductProvider(new NoDI_NHibernateContextFactory()).Add(product);

            Mock<IEntityFileProvider> entityFileProvider = new Mock<IEntityFileProvider>();
            new BrandProvider(new NoDI_NHibernateContextFactory(),entityFileProvider.Object).Add(brand);
            provider.Add(productbrand);
            Console.WriteLine("Add Ok!");

            //QueryById
            var productbrandQueryBy = provider.QueryById(productbrand.Id);
            Assert.IsNotNull(productbrandQueryBy);
            Console.WriteLine("QueryBy OK!");

            //Update
            productbrand.DisplayOrder = 2;
            provider.Update(productbrand);
            var productbrandUpdate = provider.QueryById(productbrand.Id);
            Assert.AreEqual(2, productbrandUpdate.DisplayOrder);
            Console.WriteLine("Update OK!");

            //Delete
            provider.Delete(productbrand);
            var productbrandDelete = provider.QueryById(productbrand.Id);
            Assert.IsNull(productbrandDelete);
            Console.WriteLine("Delete OK!");

        }
    }
}
