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
using Kooboo.CMS.eCommerce;
using Kooboo.eCommerce.NHibernate.Persistence.Catalog;
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.Common;
using System.Collections.Generic;

namespace Kooboo.eCommerce.NHibernate.Tests.Persistence.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]   
    public class ProductTypeProviderTests
    {
        ProductTypeProvider provider = new ProductTypeProvider(new CommerceDataDir(new BaseDir()));
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Test_Add_Get_Update_Remove_ProductTypeProvider()
        {
            var productType1 = new ProductType
            {
                UUID = "1",
                Name = "PT1"
            };
            var productType2 = new ProductType
            {
                UUID = "2",
                Name = "PT2"
            };
            foreach (ProductType item in provider.All())
            {
                provider.Remove(item);
            }
            //Add
            provider.Add(productType1);
            provider.Add(productType2);
            Console.WriteLine("AddOK!");

            //All
            IEnumerable<ProductType> productTypes = provider.All();
            Assert.AreEqual(2, productTypes.Count());

            //Get
            var productTypeActual1 = provider.Get(productType1);
            Assert.IsNotNull(productTypeActual1);
            Console.WriteLine("GetOK!");

            //Update
            var productType3 = new ProductType
            {
                UUID = "2",
                Name = "PT3"
            };
            provider.Update(@productType3, productType2);
            var productTypeActual2 = provider.Get(productType2);
            Assert.IsNull(productTypeActual2);
            var productTypeActual3 = provider.Get(productType3);
            Assert.IsNotNull(productTypeActual3);
            Console.WriteLine("Update OK!");

            //Remove
            provider.Remove(productType1);
            var productTypeActual4 = provider.Get(productType1);
            Assert.IsNull(productTypeActual4);
            Console.WriteLine("Remove OK!");

        }
    }
}
