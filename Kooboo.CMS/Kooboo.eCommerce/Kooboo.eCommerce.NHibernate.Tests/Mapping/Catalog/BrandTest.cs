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

namespace Kooboo.eCommerce.NHibernate.Tests.Tests.CataLog
{
    /// <summary>
    /// The Base Class of Tests
    /// </summary>
    [TestClass]
    public class BrandTest : BaseTest
    {
        /// <summary>
        /// Test Brand simple mapping
        /// </summary>
        [TestMethod]
        public void Test_Simple_Mapping_Brand()
        {
            var brand = new Brand
            {
                Name = "secondBrand",
                Description = "Hello description!",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            using (ISession session = MySessionFactory.OpenSession())
            {
                session.Save(brand);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    transaction.Commit();
                }
            }

        }
    }
}
