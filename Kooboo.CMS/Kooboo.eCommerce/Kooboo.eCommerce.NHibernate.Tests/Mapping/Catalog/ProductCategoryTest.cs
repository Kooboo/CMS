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
    /// Test ProductCategory mapping
    /// </summary>
    [TestClass]
    public class ProductCategoryTest : BaseTest 
    {
        /// <summary>
        /// Test simple mapping
        /// </summary>
        [TestMethod]
        public void Test_Simple_Mapping_ProductCategory()
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
            var productCategory = new ProductCategory { 
                IsFeaturedProduct=false,
                DisplayOrder=2,
                Product=product,
                Category=category
            };
            using (ISession session = MySessionFactory.OpenSession())
            {
                ITransaction transaction = session.BeginTransaction();
                session.Save(product);
                session.Save(category);
                session.Save(productCategory);
                transaction.Commit();
            }

            var productCategory1 = MySessionFactory.OpenSession().Query<ProductCategory>()
                .Where(it => it.Id == productCategory.Id)
                .FirstOrDefault();
            Assert.AreEqual(productCategory.Id, productCategory1.Id);
            Assert.AreEqual(product.Id, productCategory1.Product.Id);
            Assert.AreEqual(category.Id, productCategory1.Category.Id);

            Console.WriteLine(productCategory.Id);
            Console.WriteLine(product.Id);
            Console.WriteLine(category.Id);

            var product1 = MySessionFactory.OpenSession().Query<Product>()
                .Where(it => it.Id == product.Id)
                .FirstOrDefault();
            Assert.AreEqual(1,product1.ProductCategories.Count);
        }
    }
}
