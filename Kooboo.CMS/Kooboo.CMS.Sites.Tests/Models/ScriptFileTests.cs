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
    /// Summary description for ScriptFileTests
    /// </summary>
    [TestClass]
    
    public class ScriptFileTests
    {
        [TestMethod]
        public void TestPhysicalPath()
        {
            var site = new Site("Site1");
            var scriptFile = new ScriptFile(site, "script1.js");

            string expected1 = Path.Combine(site.PhysicalPath, "scripts", "script1.js");

            Assert.AreEqual(expected1, scriptFile.PhysicalPath, true);
        }
        [TestMethod]
        public void TestVirtualPath()
        {
            var site = new Site("Site1");
            var scriptFile = new ScriptFile(site, "script1.js");

            string expected1 = Kooboo.Web.Url.UrlUtility.Combine(site.VirtualPath, "scripts", "script1.js");

            Assert.AreEqual(expected1, scriptFile.VirtualPath, true);

        }
        [TestMethod]
        public void TestParseFromPhysicalPath()
        {
            string siteName = "site1";
            string fileName = "script1";
            string extension = ".js";
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites", siteName, "scripts", fileName + extension);

            var scriptFile = new ScriptFile(physicalPath);

            Assert.AreEqual(fileName, scriptFile.Name);
            Assert.AreEqual(extension, scriptFile.FileExtension);


            Assert.IsTrue(scriptFile.Site.IsDummy);
            Assert.AreEqual(siteName, scriptFile.Site.Name);
        }
    }
}
