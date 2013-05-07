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
using System.IO;
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;

namespace Kooboo.CMS.Content.Tests.Persistence
{
    /// <summary>
    /// Summary description for ContentRepositoryPath
    /// </summary>
    [TestClass]
    public class ContentRepositoryPathTests
    {
        [TestMethod]
        public void TestPhysicalPath()
        {
            var name = "repository1";
            var expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cms_Data", "Contents", name);

            RepositoryPath path = new RepositoryPath(new Repository(name));

            Assert.AreEqual(expected, path.PhysicalPath, true);
        }
    }
}
