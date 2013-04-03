using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Tests.Models
{
    /// <summary>
    /// Summary description for FolderTests
    /// </summary>
    [TestClass]
    public class FolderTests
    {
        [TestMethod]
        public void TestCreateFolder()
        {
            var repositoryName = "FolderTests";
            var repository = new Repository(repositoryName);
            Folder folder1 = new Folder(repository, new[] { "folder1" });

            Assert.AreEqual("folder1", folder1.Name);

            Folder folder2 = new Folder(repository, new[] { "folder1", "folder2" });

            Assert.AreEqual("folder2", folder2.Name);
            Assert.AreEqual("folder1", folder2.Parent.Name);
        }
    }
}
