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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.eCommerce.Models.Catalog;
using NHibernate;

namespace Kooboo.eCommerce.NHibernate.Tests.Catalog
{
    /// <summary>
    /// Test Product mapping 
    /// </summary>
    [TestClass]
    public class ProductTest : BaseTest
    {
        /// <summary>
        /// Test simple mapping
        /// </summary>
        [TestMethod]
        public void Test_Simple_Mapping_Product()
        {
            var p = new Product
            {
                Name = "Product1",
                UtcCreationDate = DateTime.UtcNow,
                UtcUpdateDate = DateTime.UtcNow
            };
            using (ISession session = MySessionFactory.OpenSession())
            {
                ITransaction transaction = session.BeginTransaction();
                session.Save("Product", p);
                transaction.Commit();
            }
        }

    }
}
