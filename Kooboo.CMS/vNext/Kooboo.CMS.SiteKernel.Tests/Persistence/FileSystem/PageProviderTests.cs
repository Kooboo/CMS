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
using System.IO;
using System.Collections.Generic;

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
            var childPages = pageProvider.ChildPages(pages.First());

            Assert.AreEqual(4, pages.Count());

            Assert.AreEqual("about", pages.First().FullName);
            Assert.AreEqual("Search", pages.Last().FullName);
            Assert.AreEqual("Search", childPages.First().FullName);
        }

        [TestMethod]
        public void Test_RootPages()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");
            var rootPages = pageProvider.RootPages(site);

            Assert.AreEqual(4, rootPages.Count());
            Assert.AreEqual("about", rootPages.First().FullName);
            Assert.AreEqual("Search", rootPages.Last().FullName);
            Assert.AreEqual("SampleSite", rootPages.Last().Site.AbsoluteName);
        }

        [TestMethod]
        public void Test_ChildPages()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");
            var page = new Page(site, "Member");

            var childPages = pageProvider.ChildPages(page);

            Assert.AreEqual(8, childPages.Count());
        }

        [TestMethod]
        public void Test_Get()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");

            var o = pageProvider.Get(pageProvider.RootPages(site).First());

            Assert.AreEqual("about", o.FullName);
        }

        [TestMethod]
        public void Test_Add()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");
            var newPage = new Page(site, "newPage");

            pageProvider.Add(newPage);

            var rootPages = pageProvider.RootPages(site);

            Assert.AreEqual(5, rootPages.Count());
        }

        [TestMethod]
        public void Test_Update()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");
            var oldPage = new Page(site, "oldPage");
            var newPage = new Page(site, "newPage");

            pageProvider.Add(oldPage);
            pageProvider.Update(newPage, oldPage);

            var o = pageProvider.Get(newPage);

            Assert.AreEqual("newPage", o.FullName);
        }

        [TestMethod]
        public void Test_Remove()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");
            var oldPage = new Page(site, "oldPage");

            pageProvider.Add(oldPage);
            pageProvider.Remove(oldPage);

            var rootPages = pageProvider.RootPages(site);

            Assert.AreEqual(4, rootPages.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Test_GetDraft()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");
            var page = new Page(site, "about");

            pageProvider.GetDraft(page);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Test_SaveAsDraft()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");
            var page = new Page(site, "about");

            pageProvider.SaveAsDraft(page);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Test_RemoveDraft()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");
            var page = new Page(site, "about");

            pageProvider.RemoveDraft(page);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Test_Import()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");
            var page = new Page(site, "about");

            pageProvider.Import(page, new MemoryStream(), new Dictionary<string, object>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Test_Export()
        {
            var pageProvider = new PageProvider();
            var site = new Site("SampleSite");
            var rootPages = pageProvider.RootPages(site);

            pageProvider.Export(rootPages, new Dictionary<string, object>());
        }
    }
}
