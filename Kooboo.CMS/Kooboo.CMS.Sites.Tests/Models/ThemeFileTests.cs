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
    
    public class ThemeFileTests
    {
        [TestMethod]
        public void TestPhysicalPath()
        {
            string themeName = "theme1";
            var site = new Site("Site1");
            var theme = new Theme(site, themeName);
            var themeFile = new ThemeFile(theme, "style1.css");

            string expected1 = Path.Combine(site.PhysicalPath, "themes", themeName, "style1.css");

            Assert.AreEqual(expected1, themeFile.PhysicalPath, true);
        }
        [TestMethod]
        public void TestVirtualPath()
        {
            string themeName = "theme1";
            var site = new Site("Site1");
            var theme = new Theme(site, themeName);
            var themeFile = new ThemeFile(theme, "style1.css");

            string expected1 = Kooboo.Web.Url.UrlUtility.Combine(site.VirtualPath, "themes", themeName, "style1.css");

            Assert.AreEqual(expected1, themeFile.VirtualPath, true);

        }
        [TestMethod]
        public void TestParseFromPhysicalPath()
        {
            string siteName = "site1";
            string themeName = "theme1";
            string fileName = "style1";
            string extension = ".css";
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites", siteName, "themes", themeName, fileName + extension);

            var themeFile = new ThemeFile(physicalPath);

            Assert.AreEqual(fileName, themeFile.Name);
            Assert.AreEqual(extension, themeFile.FileExtension);

            Assert.AreEqual(themeName, themeFile.Theme.Name);            

            Assert.IsTrue(themeFile.Site.IsDummy);
            Assert.AreEqual(siteName, themeFile.Site.Name);
        }
    }
}
