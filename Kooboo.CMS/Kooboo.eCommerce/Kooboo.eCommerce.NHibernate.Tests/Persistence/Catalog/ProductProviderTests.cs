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
    public class ProductProviderTests : BaseTest
    {
        ProductProvider provider = new ProductProvider(new NoDI_NHibernateContextFactory());
        /// <summary>
        /// Test_CreateQuery_ProductProvider
        /// </summary>
        [TestMethod]
        public void Test_CreateQuery_BrandProvider()
        {
            var query = provider.CreateQuery();

            Assert.IsNotNull(query);
            Console.WriteLine("CreateQuery OK!");
        }

        /// <summary>
        /// Test_Add_QueryById_Update_Delete_ProductProvider
        /// </summary>
        [TestMethod]
        public void Test_Add_QueryById_Update_Delete_ProductProvider()
        {
            var product = new Product
            {
                Name = "Product1",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };

            //Add brand
            provider.Add(product);
            Console.WriteLine("Add OK!");

            //QueryBy
            var productQueryBy = provider.QueryById(product.Id);
            Assert.IsNotNull(productQueryBy);
            Console.WriteLine("QueryBy OK!");

            //update
            product.Name = "newName";
            provider.Update(product);
            var productUpdate = provider.QueryById(product.Id);
            Assert.AreEqual("newName", productUpdate.Name);
            Console.WriteLine("Update OK!");

            //Delete
            provider.Delete(product);
            var productDelete = provider.QueryById(product.Id);
            Assert.IsNull(productDelete);
            Console.WriteLine("Delete OK!");


        }
    }
}
