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
using Kooboo.CMS.eCommerce.Persistence;
using Kooboo.CMS.eCommerce.Models;
using System.IO;
using Kooboo.Web.Url;

namespace Kooboo.CMS.eCommerce.Tests.Persistence
{
    [TestClass]
    public class EntityFileProviderTest
    {
        ICommerceDataDir commerceDataDir = new CommerceDataDir(new Kooboo.CMS.Common.BaseDir());
        EntityFileProvider entityFileProvider;
        public EntityFileProviderTest()
        {
            entityFileProvider = new EntityFileProvider(commerceDataDir);
        }

        [TestMethod]
        public void Test_Save()
        {
            EntityFile entityFile = new EntityFile()
            {
                FileName = "image1.png",
                Data = new MemoryStream(new byte[] { 0, 1, 2, 3 }),
                ContentType = "image"
            };
            var result = entityFileProvider.Save(entityFile, "Tests");
            var expectedPhysicalPath = Path.Combine(commerceDataDir.DataPhysicalPath, "Tests", entityFile.FileName);
            var expectedVirtualPath = UrlUtility.Combine(commerceDataDir.DataVirutalPath, "Tests", entityFile.FileName);
            Assert.AreEqual(expectedPhysicalPath, result.PhysicalPath);
            Assert.AreEqual(expectedVirtualPath, result.VirtualPath);
        }
        [TestMethod]
        public void Test_DeleteFolder()
        {
            entityFileProvider.DeleteFolder("Tests");
        }
        [TestMethod]
        public void Test_Delete()
        {
            entityFileProvider.Delete(UrlUtility.Combine(commerceDataDir.DataVirutalPath, "Tests", "image1.png"));
        }
    }
}
