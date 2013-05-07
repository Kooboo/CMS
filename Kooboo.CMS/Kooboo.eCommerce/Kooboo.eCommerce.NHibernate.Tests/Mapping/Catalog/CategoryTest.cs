#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.eCommerce.Models.Catalog;
using NHibernate;
using System.Linq;
using NHibernate.Linq;

namespace Kooboo.eCommerce.NHibernate.Tests.Catalog
{
    /// <summary>
    /// Test Category mapping
    /// </summary>
    [TestClass]
    public class CategoryTest : BaseTest
    {
        /// <summary>
        /// Test Category simple mapping
        /// </summary>
        [TestMethod]
        public void Test_Simple_Mapping_Category()
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
            using (ISession session = MySessionFactory.OpenSession())
            {
                ITransaction transaction = session.BeginTransaction();
                session.Save(category);
                session.Flush();
                transaction.Commit();
            }
        }

        /// <summary>
        /// Test Category.Parent mapping
        /// </summary>
        [TestMethod]
        public void Test_Parent_Mapping_Category()
        {
            var session = MySessionFactory.OpenSession();
            var category1 = new Category
            {
                Name = "catagory1",
                Description = "Hello description!",

                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow,
                Parent = null
            };
            var category2 = new Category
            {
                Name = "catagory2",
                Description = "Hello description!",

                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow,

                Parent = category1
            };

            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(category1);
                session.Save(category2);
                transaction.Commit();
            }


            Assert.AreNotEqual(0, category2.Id);

            var actualCategory2 = session.Query<Category>()
                .Where(it => it.Id == category2.Id)
                .FirstOrDefault();

            Assert.AreEqual(category2.Id, actualCategory2.Id);

            session = MySessionFactory.OpenSession();

            var actualCategory1 = session.Query<Category>()
              .Where(it => it.Id == category1.Id)
              .FirstOrDefault();

            using (var transaction = session.BeginTransaction())
            {
                session.Delete(actualCategory1);
                transaction.Commit();
            }

        }

        [TestMethod]
        public void Test_Change_Parent()
        {
            var session = MySessionFactory.OpenSession();
            var parent1 = new Category
            {
                Name = "parent1",
                Description = "Hello description!",

                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow,
                Parent = null
            };
            var child1 = new Category
            {
                Name = "child",
                Description = "Hello description!",

                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow,

                Parent = parent1
            };

            var parent2 = new Category
            {
                Name = "parent2",
                Description = "Hello description!",

                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow,
            };

            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(parent1);
                session.Save(child1);
                session.Save(parent2);
                transaction.Commit();
            }

            session = MySessionFactory.OpenSession();

            var actualChild1 = session.Query<Category>()
                .Where(it => it.Id == child1.Id)
                .First();
            var actualParent2 = session.Query<Category>()
                .Where(it => it.Id == parent2.Id)
                .First();

            actualChild1.Parent = actualParent2;

            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(actualChild1);
                transaction.Commit();
            }
            actualChild1 = session.Query<Category>()
                .Where(it => it.Id == child1.Id)
                .FirstOrDefault();

            Assert.AreEqual(actualParent2.Id, actualChild1.ParentId);

            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(actualParent2);
                transaction.Commit();
            }

            actualChild1 = session.Query<Category>()
                .Where(it => it.Id == child1.Id)
                .FirstOrDefault();
            Assert.IsNull(actualChild1);
        }
    }
}
