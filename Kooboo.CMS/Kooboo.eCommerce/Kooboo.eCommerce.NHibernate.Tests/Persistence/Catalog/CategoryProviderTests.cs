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
namespace Kooboo.eCommerce.NHibernate.Tests.Persistence
{
    /// <summary>
    /// test categoryprovider
    /// </summary>
    [TestClass]
    public class CategoryProviderTests : BaseTest
    {
        CategoryProvider provider;
        public CategoryProviderTests()
        {
            Mock<IEntityFileProvider> entityFileProvider = new Mock<IEntityFileProvider>();
            provider = new CategoryProvider(new NoDI_NHibernateContextFactory(), entityFileProvider.Object);
        }

        /// <summary>
        /// Test_CreateQuery_CategoryProvider
        /// </summary>
        [TestMethod]
        public void Test_CreateQuery_CategoryProvider()
        {
            var query = provider.CreateQuery();

            Assert.IsNotNull(query);
            Console.WriteLine("CreateQuery OK!");
        }

        /// <summary>
        /// Test_Add_QueryById_Update_Delete_CategoryProvider
        /// </summary>
        [TestMethod]
        public void Test_Add_QueryById_Update_Delete_CategoryProvider()
        {
            var category = new Category
            {
                Name = "category1",
                Description = "Hello111",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };

            //Add Catagory
            provider.Add(category);
            Console.WriteLine("Add OK!");

            //QueryBy
            var categoryQueryBy = provider.QueryById(category.Id);
            Assert.IsNotNull(categoryQueryBy);
            Console.WriteLine("QueryBy OK!");

            //update
            category.Description = "New description";
            provider.Update(category);
            var categoryUpdate = provider.QueryById(category.Id);
            Assert.AreEqual("New description", categoryUpdate.Description);
            Console.WriteLine("Update OK!");

            //Delete
            provider.Delete(category);
            var categoryDelete = provider.QueryById(category.Id);
            Assert.IsNull(categoryDelete);
            Console.WriteLine("Delete OK!");


        }
    }
}
