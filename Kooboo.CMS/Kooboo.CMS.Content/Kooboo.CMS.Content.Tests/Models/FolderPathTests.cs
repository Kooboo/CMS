using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Content.Persistence.Default;
using System.IO;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;

namespace Kooboo.CMS.Content.Tests.Models
{
    /// <summary>
    /// Summary description for FolderPathTests
    /// </summary>
    [TestClass]
    public class FolderPathTests
    {
        [TestMethod]
        public void TestPhysicalPath()
        {
            var repositoryName = "repository1";
            var folder = "folder1";

            var expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cms_Data", "Contents", repositoryName, "Media", folder);

            FolderPath path = new FolderPath(new Folder(new Repository(repositoryName), folder));

            Assert.AreEqual(expected, path.PhysicalPath, true);

            expected = Path.Combine(expected, "setting.config");
            Assert.AreEqual(expected, path.SettingFile, true);
        }
    }
}
