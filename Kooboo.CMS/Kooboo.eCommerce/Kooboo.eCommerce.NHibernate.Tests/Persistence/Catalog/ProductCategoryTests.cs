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
using Moq;
using Kooboo.CMS.eCommerce.Persistence;

namespace Kooboo.eCommerce.NHibernate.Tests.Persistence.Catalog
{
    [TestClass]
    public class ProductCategoryTests : BaseTest
    {
        ProductCategoryProvider provider = new ProductCategoryProvider(new NoDI_NHibernateContextFactory());
        /// <summary>
        /// Test_CreateQuery_ProductCategoryProvider
        /// </summary>
        [TestMethod]
        public void Test_CreateQuery_ProductCategoryProvider()
        {
            var query = provider.CreateQuery();

            Assert.IsNotNull(query);
            Console.WriteLine("CreateQuery OK!");
        }

        /// <summary>
        /// Test_Add_QueryById_Update_Delete_ProductCategoryProvider
        /// </summary>
        [TestMethod]
        public void Test_Add_QueryById_Update_Delete_ProductCategoryProvider()
        {
            var category = new Category
            {
                Name = "catagory1",
                Description = "Hello description!",
                Image = "",
                PageSize = 10,
                Published = true,
                Deleted = false,
                DisplayOrder = 1,
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            var product = new Product
            {
                Name = "Product1",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            var productCategory = new ProductCategory
            {
                IsFeaturedProduct = false,
                DisplayOrder = 2,
                Product = product,
                Category = category
            };

            Mock<IEntityFileProvider> entityFileProvider = new Mock<IEntityFileProvider>();
            var categoryProvider = new CategoryProvider(new NoDI_NHibernateContextFactory(), entityFileProvider.Object);
            //Add brand
            new ProductProvider(new NoDI_NHibernateContextFactory()).Add(product);
            categoryProvider.Add(category);
            provider.Add(productCategory);
            Console.WriteLine("Add OK!");

            //QueryBy
            var productCategoryQueryBy = provider.QueryById(productCategory.Id);
            Assert.IsNotNull(productCategoryQueryBy);
            Console.WriteLine("QueryBy OK!");

            //update
            productCategory.DisplayOrder = 5;
            provider.Update(productCategory);
            var productCategoryUpdate = provider.QueryById(productCategory.Id);
            Assert.AreEqual(5, productCategoryUpdate.DisplayOrder);
            Console.WriteLine("Update OK!");

            //Delete
            provider.Delete(productCategory);
            var productCategoryDelete = provider.QueryById(productCategory.Id);
            Assert.IsNull(productCategoryDelete);
            Console.WriteLine("Delete OK!");

        }
    }
}
