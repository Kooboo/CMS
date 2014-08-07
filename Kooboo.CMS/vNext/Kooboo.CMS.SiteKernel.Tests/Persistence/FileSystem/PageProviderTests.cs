#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.CMS.SiteKernel.Persistence.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Kooboo.CMS.SiteKernel.Tests.Persistence.FileSystem
{
    [TestClass]
    public class PageProviderTests
    {
        [TestInitialize]
        public void TestInitializer()
        {
            Kooboo.Common.ObjectContainer.EngineContext.DefaultTypeFinder = new Kooboo.Common.Web.ObjectContainer.WebAppTypeFinder();
        }
        [TestMethod]
        public void Test_All()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");
            var pages = pageProvider.All(site);

            Assert.AreEqual(4, pages.Count());

            Assert.AreEqual("about", pages.First().FullName);
            Assert.AreEqual("Search", pages.Last().FullName);
        }
    }
}
