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
using Kooboo.CMS.eCommerce;
using Kooboo.CMS.Common;

namespace Kooboo.eCommerce.NHibernate.Tests.Persistence.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ProductVariantProviderTests
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Test_Add_Get_Update_Remove_ProductVariantProvider()
        {
            ProductVariantProvider provider = new ProductVariantProvider(new CommerceDataDir(new BaseDir()));
            var productVariant1 = new ProductVariant
            {
                UUID = "111",
                ProductId = 111
            };
            foreach(ProductVariant item in provider.All())
            {
                provider.Remove(item);
            }
            //Add Get
            provider.Add(productVariant1);
            var productVariantActual = provider.Get(productVariant1);
            Assert.IsNotNull(productVariantActual);
            Console.WriteLine("Add ok!");
            Console.WriteLine("Get ok!");

            //All
            var productVariantAll = provider.All();
            Assert.AreEqual(1, productVariantAll.Count());
            Console.WriteLine("All ok!");

            //Update
            var productVariant2 = new ProductVariant
            {
                UUID = "222",
                ProductId = 111,
            };
            provider.Update(@productVariant2, productVariant1);
            var productVariantActual1 = provider.Get(productVariant1);
            Assert.IsNull(productVariantActual1);
            var productVariantActual2 = provider.Get(productVariant2);
            Assert.IsNotNull(productVariantActual2);
            Console.WriteLine("Update OK!");

            //Remove
            provider.Remove(productVariant2);
            var productVariantActual3 = provider.Get(productVariant2);
            Assert.IsNull(productVariantActual3);
            Console.WriteLine("Remove OK!");
        }
    }
}
