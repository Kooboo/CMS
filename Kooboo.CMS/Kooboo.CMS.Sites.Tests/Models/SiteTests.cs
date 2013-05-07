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
    /// Summary description for SiteTests
    /// </summary>
    [TestClass]
    public class SiteTests
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void TestPhysicalPath()
        {
            Site site = new Site("site1");
            string expected = Path.Combine(Kooboo.Settings.BaseDirectory, "Cms_Data", "sites", "site1");
            Assert.AreEqual(expected, site.PhysicalPath, true);
        }
        [TestMethod]
        public void TestVirtualPath()
        {
            Site site = new Site("site1");
            string expected = Kooboo.Web.Url.UrlUtility.Combine("~/","Cms_Data", "sites", "site1");
            Assert.AreEqual(expected, site.VirtualPath, true);
        }
        [TestMethod]
        public void TestParseFromPhysicalPath()
        {
            string siteName = "site1";
            string physicalPath = Path.Combine(Kooboo.Settings.BaseDirectory, "sites", siteName);
            Site site = Site.ParseSiteFromPhysicalPath(physicalPath);
            Assert.AreEqual(siteName, site.Name);
        }

        [TestMethod]
        public void TestParseFromRelativePath()
        {
            string siteName = "site1";
            Site site = Site.ParseSiteFromRelativePath(new string[] { siteName });
            Assert.AreEqual(siteName, site.Name);
        }

        [TestMethod]
        public void TestParseWithParentSite()
        {
            string parent = "parent";
            string siteName = "subSite";

            string physicalPath = Path.Combine(Kooboo.Settings.BaseDirectory, "sites", parent, Site.PATH_NAME, siteName);
            Site site = Site.ParseSiteFromPhysicalPath(physicalPath);
            Assert.AreEqual(siteName, site.Name);
            Assert.IsNotNull(site.Parent);
            Assert.AreEqual(parent, site.Parent.Name);
        }

        [TestMethod]
        public void TestPhysicalPathWithParentSite()
        {
            Site parent = new Site("parent");
            Site sub = new Site(parent, "sub");
            string expected = Path.Combine(Kooboo.Settings.BaseDirectory, "Cms_Data", "sites", "parent", Site.PATH_NAME, "sub");
            Assert.AreEqual(expected, sub.PhysicalPath, true);
        }
        [TestMethod]
        public void TestRelativePaths()
        {
            Site parent = new Site("parent");

            Assert.AreEqual("parent", parent.RelativePaths.First());
        }
        [TestMethod]
        public void TestRelativePathsWithParentSite()
        {
            Site parent = new Site("parent");
            Site child = new Site(parent, "child");
            Assert.AreEqual(2, child.RelativePaths.Count());
            Assert.AreEqual("parent", child.RelativePaths.First());
            Assert.AreEqual("child", child.RelativePaths.Last());
        }
    }
}
