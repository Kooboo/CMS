#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Sites.Models;

using System.IO;

namespace Kooboo.CMS.Sites.Tests.Models
{
    /// <summary>
    /// Summary description for ImageFileTests
    /// </summary>
    [TestClass]

    public class CustomFileTests
    {
        [TestMethod]
        public void TestPhysicalPath()
        {
            var site = new Site("Site1");
            string imageName = "image1.jpg";
            var image = new CustomFile(site, imageName);

            string expected1 = Path.Combine(site.PhysicalPath, "files", imageName);

            Assert.AreEqual(expected1, image.PhysicalPath, true);
        }
        [TestMethod]
        public void TestVirtualPath()
        {
            var site = new Site("Site1");
            string imageName = "image1.jpg";
            var image = new CustomFile(site, imageName);

            string expected1 = Kooboo.Web.Url.UrlUtility.Combine(site.VirtualPath, "files", imageName);

            Assert.AreEqual(expected1, image.VirtualPath, true);

        }
        [TestMethod]
        public void TestParseFromPhysicalPath()
        {
            string siteName = "site1";
            string imageName = "image1";
            string extension = ".jpg";
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites", siteName, "files", imageName + extension);

            var imageFile = new CustomFile(physicalPath);

            Assert.AreEqual(imageName, imageFile.Name);
            Assert.AreEqual(extension, imageFile.FileExtension);

            Assert.AreEqual(siteName, imageFile.Site.Name);
        }

        [TestMethod]
        public void TestParseWithDirFromPhysicalPath()
        {
            string siteName = "site1";
            string imageName = "image1";
            string extension = ".jpg";
            string dir = "dir1";
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites", siteName, "files", dir, imageName + extension);

            var imageFile = new CustomFile(physicalPath);

            Assert.AreEqual(imageName, imageFile.Name);
            Assert.AreEqual(extension, imageFile.FileExtension);


            Assert.AreEqual(dir, imageFile.Directory.Name);
        }
    }
}
