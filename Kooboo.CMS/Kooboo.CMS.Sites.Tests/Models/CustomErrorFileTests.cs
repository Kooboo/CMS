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
    /// Summary description for CustomErrorFileTest
    /// </summary>
    [TestClass]
    public class CustomErrorFileTests
    {
        [TestMethod]
        public void TestPhysicalPath()
        {
            var site = new Site("Site1");

            string expected1 = Path.Combine(site.PhysicalPath, "CustomErrors.config");

            CustomErrorsFile customErrorFile = new CustomErrorsFile(site);
            Assert.AreEqual(expected1, customErrorFile.PhysicalPath, true);
        }
    }
}
