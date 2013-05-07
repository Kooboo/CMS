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
    public class BrandProviderTests : BaseTest
    {
        BrandProvider provider;
        public BrandProviderTests()
        {
            Mock<IEntityFileProvider> entityFileProvider = new Mock<IEntityFileProvider>();
            provider = new BrandProvider(new NoDI_NHibernateContextFactory(), entityFileProvider.Object);
        }
        /// <summary>
        /// Test_CreateQuery_BrandProvider
        /// </summary>
        [TestMethod]
        public void Test_CreateQuery_BrandProvider()
        {
            var query = provider.CreateQuery();

            Assert.IsNotNull(query);
            Console.WriteLine("CreateQuery OK!");
        }

        /// <summary>
        /// Test_Add_QueryById_Update_Delete_BrandProvider
        /// </summary>
        [TestMethod]
        public void Test_Add_QueryById_Update_Delete_BrandProvider()
        {
            var brand = new Brand
            {
                Name = "secondBrand",
                Description = "Hello description!",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };

            //Add brand
            provider.Add(brand);
            Console.WriteLine("Add OK!");

            //QueryBy
            var brandQueryBy = provider.QueryById(brand.Id);
            Assert.IsNotNull(brandQueryBy);
            Console.WriteLine("QueryBy OK!");

            //update
            brand.Description = "New description";
            provider.Update(brand);
            var brandUpdate = provider.QueryById(brand.Id);
            Assert.AreEqual("New description", brandUpdate.Description);
            Console.WriteLine("Update OK!");

            //Delete
            provider.Delete(brand);
            var brandDelete = provider.QueryById(brand.Id);
            Assert.IsNull(brandDelete);
            Console.WriteLine("Delete OK!");


        }
    }
}
