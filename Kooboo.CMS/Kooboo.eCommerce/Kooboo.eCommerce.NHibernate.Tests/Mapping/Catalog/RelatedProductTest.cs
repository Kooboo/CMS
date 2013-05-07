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
    /// Test RelatedProduct mapping
    /// </summary>
    [TestClass]
    public class RelatedProductTest : BaseTest
    {
        /// <summary>
        /// Test simple mapping
        /// </summary>
        [TestMethod]
        public void Test_Simple_Mapping_RelatedProduct()
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
            using (ISession session = MySessionFactory.OpenSession())
            {
                ITransaction transcation = session.BeginTransaction();
                session.Save(product1);
                session.Save(product2);
                session.Save(relatedProduct);
                transcation.Commit();
            }
            var relatedProduct1 = MySessionFactory.OpenSession().Query<RelatedProduct>()
                .Where(it => it.Id == relatedProduct.Id)
                .FirstOrDefault();
            Assert.AreEqual(relatedProduct.Id, relatedProduct1.Id);
            Assert.AreEqual(product1.Id, relatedProduct1.Product1.Id);
            Assert.AreEqual(product2.Id, relatedProduct1.Product2.Id);
        }
    }
}
