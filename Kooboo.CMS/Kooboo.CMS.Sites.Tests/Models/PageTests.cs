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
    /// Summary description for PageTests
    /// </summary>
    [TestClass]

    public class PageTests
    {
        [TestMethod]
        public void TestPhysicalPath()
        {
            string pageName = "page1";
            var site = new Site("Site1");
            var page = new Page(site, new string[] { pageName });

            string expected1 = Path.Combine(site.PhysicalPath, "pages", pageName);

            Assert.AreEqual(expected1, page.PhysicalPath, true);
        }
      
        [TestMethod]
        public void TestParseFromPhysicalPath()
        {
            string siteName = "site1";
            string pageName = "page1";
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites", siteName, "pages", pageName);

            var page = new Page(physicalPath);

            Assert.AreEqual(pageName, page.Name);
            Assert.IsTrue(page.IsDummy);
        }


        [TestMethod]
        public void TestChildPhysicalPath()
        {
            string parentPageName = "page1";
            var site = new Site("Site1");
            var parent = new Page(site, new string[] { parentPageName });
            string childPageName = "childPage";
            var child = new Page(parent, childPageName);

            string expected1 = Path.Combine(site.PhysicalPath, "pages", parentPageName, childPageName);

            Assert.AreEqual(expected1, child.PhysicalPath, true);
        }

        [TestMethod]
        public void TestParseChildFromPhysicalPath()
        {
            string siteName = "site1";
            string parentPageName = "page1";
            string childPageName = "childPage";
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites", siteName, "pages", parentPageName, childPageName);

            var page = new Page(physicalPath);

            Assert.AreEqual(childPageName, page.Name);
            Assert.AreEqual(parentPageName, page.Parent.Name);
            Assert.AreEqual(siteName, page.Site.Name);
            Assert.IsTrue(page.IsDummy);
        }
        [TestMethod]
        public void CreateChildPageByPageNamePaths()
        {
            string parentPageName = "page1";
            var site = new Site("Site1");
            string childPageName = "childPage";
            var child = new Page(site, new string[] { parentPageName, childPageName });

            string expected1 = Path.Combine(site.PhysicalPath, "pages", parentPageName, childPageName);

            Assert.AreEqual(childPageName, child.Name, true);
            Assert.AreEqual(expected1, child.PhysicalPath, true);
        }

    }
}
