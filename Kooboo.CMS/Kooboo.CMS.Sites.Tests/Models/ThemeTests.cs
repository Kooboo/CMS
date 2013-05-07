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
    /// Summary description for ThemeTests
    /// </summary>
    [TestClass]
    
    public class ThemeTests
    {
        [TestMethod]
        public void TestPhysicalPath()
        {
            string themeName = "theme1";
            var site = new Site("Site1");
            var theme = new Theme(site, themeName);

            string expected1 = Path.Combine(site.PhysicalPath, "themes", themeName);

            Assert.AreEqual(expected1, theme.PhysicalPath, true);


        }
        [TestMethod]
        public void TestVirtualPath()
        {
            string themeName = "theme1";
            var site = new Site("Site1");
            var theme = new Theme(site, themeName);


            string expected1 = Kooboo.Web.Url.UrlUtility.Combine(site.VirtualPath, "themes", themeName);

            Assert.AreEqual(expected1, theme.VirtualPath, true);

        }
        [TestMethod]
        public void TestParseFromPhysicalPath()
        {
            string siteName = "site1";
            string themeName = "theme1";
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites", siteName, "themes", themeName);

            var theme = new Theme(physicalPath);

            Assert.AreEqual(themeName, theme.Name);
            
            Assert.IsTrue(theme.Site.IsDummy);
            Assert.AreEqual(siteName, theme.Site.Name);
        }
    }
}
