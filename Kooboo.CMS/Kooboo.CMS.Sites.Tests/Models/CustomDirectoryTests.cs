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
    /// Summary description for CustomDirectory
    /// </summary>
    [TestClass]
    public class CustomDirectoryTests
    {
        [TestMethod]
        public void TestPhysicalPath()
        {
            string dirName = "dir1";
            var site = new Site("Site1");
            var dir = new CustomDirectory(site, new string[] { dirName });

            string expected1 = Path.Combine(site.PhysicalPath, "files", dirName);

            Assert.AreEqual(expected1, dir.PhysicalPath, true);
        }
        [TestMethod]
        public void TestVirtualPath()
        {
            string dirName = "page1";
            var site = new Site("Site1");
            var dir = new CustomDirectory(site, new string[] { dirName });


            string expected1 = Kooboo.Web.Url.UrlUtility.Combine(site.VirtualPath, "files", dirName);

            Assert.AreEqual(expected1, dir.VirtualPath, true);
        }
        [TestMethod]
        public void TestParseFromPhysicalPath()
        {
            string siteName = "site1";
            string dirName = "dir1";
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites", siteName, "files", dirName);

            var page = new CustomDirectory(physicalPath);

            Assert.AreEqual(dirName, page.Name);
            Assert.AreEqual(siteName, page.Site.Name);
        }


        [TestMethod]
        public void TestChildPhysicalPath()
        {
            string parentDir = "dir1";
            var site = new Site("Site1");
            var parent = new CustomDirectory(site, new string[] { parentDir });
            string dir2 = "dir2";
            var child = new CustomDirectory(parent, dir2);

            string expected1 = Path.Combine(site.PhysicalPath, "files", parentDir, dir2);

            Assert.AreEqual(expected1, child.PhysicalPath, true);
        }

        [TestMethod]
        public void TestParseChildFromPhysicalPath()
        {
            string siteName = "site1";
            string dirName = "dir1";
            string dir2 = "dir2";
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites", siteName, "files", dirName, dir2);

            var dir = new CustomDirectory(physicalPath);

            Assert.AreEqual(dir2, dir.Name);
            Assert.AreEqual(dirName, dir.Parent.Name);
            Assert.AreEqual(siteName, dir.Site.Name);
        }

    }
}
