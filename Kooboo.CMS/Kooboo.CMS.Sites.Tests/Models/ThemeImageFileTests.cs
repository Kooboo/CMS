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
    /// Summary description for ThemeFileTests
    /// </summary>
    [TestClass]

    public class ThemeImageFileTests
    {
        [TestMethod]
        public void TestPhysicalPath()
        {
            string themeName = "theme1";
            var site = new Site("Site1");
            var theme = new Theme(site, themeName);
            var themeImageFile = new ThemeImageFile(theme, "image1.jpg");

            string expected1 = Path.Combine(site.PhysicalPath, "themes", themeName, "Images", "image1.jpg");

            Assert.AreEqual(expected1, themeImageFile.PhysicalPath, true);
        }
        [TestMethod]
        public void TestVirtualPath()
        {
            string themeName = "theme1";
            var site = new Site("Site1");
            var theme = new Theme(site, themeName);
            var themeImageFile = new ThemeImageFile(theme, "image1.jpg");

            string expected1 = Kooboo.Web.Url.UrlUtility.Combine(site.VirtualPath, "themes", themeName, "Images", "image1.jpg");

            Assert.AreEqual(expected1, themeImageFile.VirtualPath, true);

        }
        [TestMethod]
        public void TestParseFromPhysicalPath()
        {
            string siteName = "site1";
            string themeName = "theme1";
            string fileName = "image1";
            string extension = ".jpg";
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites", siteName, "themes", themeName, "Images", fileName + extension);

            var themeImageFile = new ThemeFile(physicalPath);

            Assert.AreEqual(fileName, themeImageFile.Name);
            Assert.AreEqual(extension, themeImageFile.FileExtension);

            Assert.AreEqual(themeName, themeImageFile.Theme.Name);

            Assert.IsTrue(themeImageFile.Site.IsDummy);
            Assert.AreEqual(siteName, themeImageFile.Site.Name);
        }
    }
}
