#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.CMS.SiteKernel.Persistence.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Tests.Persistence.FileSystem
{
    [TestClass]
    public class SiteProviderTests
    {
        [TestInitialize]
        public void TestInitializer()
        {
            Kooboo.Common.ObjectContainer.EngineContext.DefaultTypeFinder = new Kooboo.Common.Web.ObjectContainer.WebAppTypeFinder();
        }

        [TestMethod]
        public void Test_All()
        {
            var siteProvider = new SiteProvider(new BaseDir());

            var sites = siteProvider.All();

            Assert.AreEqual(1, sites.Count());
        }
        [TestMethod]
        public void Test_ChildSites()
        {
            var siteProvider = new SiteProvider(new BaseDir());
            var site = new Site("SampleSite");

            var childSites = siteProvider.ChildSites(site);

            Assert.AreEqual(3, childSites.Count());
        }
    }
}
