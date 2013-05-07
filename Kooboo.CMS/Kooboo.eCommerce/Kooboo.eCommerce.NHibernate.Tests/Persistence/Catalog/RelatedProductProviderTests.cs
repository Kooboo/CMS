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
    public class RelatedProductProviderTests : BaseTest
    {
        RelatedProductProvider provider = new RelatedProductProvider(new NoDI_NHibernateContextFactory());
        /// <summary>
        /// Test_CreateQuery_RelatedProductProvider
        /// </summary>
        [TestMethod]
        public void Test_CreateQuery_RelatedProductProvider()
        {
            var query = provider.CreateQuery();

            Assert.IsNotNull(query);
            Console.WriteLine("CreateQuery OK!");
        }

        /// <summary>
        /// Test_Add_QueryById_Update_Delete_RelatedProductProvider
        /// </summary>
        [TestMethod]
        public void Test_Add_QueryById_Update_Delete_RelatedProductProvider()
        {
            var product1 = new Product
            {
                Name = "Product1",
                ShortDescription = "this is product1",
                FullDescription = "full",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            var product2 = new Product
            {
                Name = "Product2",
                ShortDescription = "this is product2",
                FullDescription = "full",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            var relatedProduct = new RelatedProduct
            {
                DisplayOrder = 1,
                Product1 = product1,
                Product2 = product2
            };

            //Add brand
            ProductProvider productProcider = new ProductProvider(new NoDI_NHibernateContextFactory());
            productProcider.Add(product1);
            productProcider.Add(product2);
            provider.Add(relatedProduct);
            Console.WriteLine("Add OK!");

            //QueryBy
            var relatedProductQueryBy = provider.QueryById(relatedProduct.Id);
            Assert.IsNotNull(relatedProductQueryBy);
            Console.WriteLine("QueryBy OK!");

            //update
            relatedProduct.DisplayOrder = 5;
            provider.Update(relatedProduct);
            var brandUpdate = provider.QueryById(relatedProduct.Id);
            Assert.AreEqual(5, relatedProduct.DisplayOrder);
            Console.WriteLine("Update OK!");

            //Delete
            provider.Delete(relatedProduct);
            var relatedProductDelete = provider.QueryById(relatedProduct.Id);
            Assert.IsNull(relatedProductDelete);
            Console.WriteLine("Delete OK!");

        }
    }
}
