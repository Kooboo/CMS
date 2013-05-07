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
using NHibernate;
using NHibernate.Linq;

namespace Kooboo.eCommerce.NHibernate.Tests.Catalog
{
    /// <summary>
    /// Test ProductBrand
    /// </summary>
    [TestClass]
    public class ProductBrandTest : BaseTest
    {
        /// <summary>
        /// Test Simple Mapping 
        /// </summary>
        [TestMethod]
        public void Test_Simple_Mapping_ProductBrand()
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
            using (ISession session = MySessionFactory.OpenSession())
            {
                ITransaction transaction = session.BeginTransaction();
                session.Save(product);
                session.Save(brand);
                session.Save(productbrand);
                transaction.Commit();
            }
            var productbrand1 = MySessionFactory.OpenSession().Query<ProductBrand>()
                .Where(it => it.Id == productbrand.Id)
                .FirstOrDefault();
            Assert.AreEqual(productbrand1.Id, productbrand.Id);
                        
            var product1 = MySessionFactory.OpenSession().Query<Product>()
                .Where(it => it.Id == product.Id)
                .FirstOrDefault();
                        
            Assert.AreEqual(1, product1.ProductBrands.Count);
        }
    }
}
