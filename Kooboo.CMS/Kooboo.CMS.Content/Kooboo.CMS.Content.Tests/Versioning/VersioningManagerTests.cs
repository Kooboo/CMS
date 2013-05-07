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
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Versioning;
namespace Kooboo.CMS.Content.Tests.Versioning
{
    /// <summary>
    /// Summary description for VersioningManagerTest
    /// </summary>
    [TestClass]
    public class VersioningManagerTests
    {
        [TestMethod]
        public void Test1()
        {
            TextContent content = new TextContent();
            content.Repository = "repository1";
            content.FolderName = "news";
            content.UtcCreationDate = DateTime.Now;
            content.UtcLastModificationDate = DateTime.Now;
            content["title"] = "title1";

            VersionManager.LogVersion(content);

            Assert.AreEqual(1, VersionManager.AllVersions(content).First());

            var version1 = VersionManager.GetVersion(content, 1);

            Assert.AreEqual(content["title"], version1.TextContent["title"]);
            Assert.AreEqual(content.UtcLastModificationDate, version1.UtcCommitDate);

            //content["title"] = "title2";
            //content.UtcLastModificationDate = DateTime.Now;

            //VersionManager.LogVersion(content);

            //Assert.AreEqual(2, VersionManager.AllVersions(content).Last());

            //var version2 = VersionManager.GetVersion(content, 2);

            //Assert.AreEqual(content["title"], version2.TextContent["title"]);
            //Assert.AreEqual(content.UtcLastModificationDate, version2.UtcCommitDate);
        }
    }
}
