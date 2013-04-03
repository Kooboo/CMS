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
    /// Summary description for LayoutTemplateTests
    /// </summary>
    [TestClass]

    public class LayoutTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Kooboo.CMS.Sites.View.TemplateEngines.RegisterEngine(new Kooboo.CMS.Sites.TemplateEngines.WebForm.WebFormTemplateEngine());
        }
        [TestMethod]
        public void TestPhysicalPath()
        {
            string templateName = "template1";
            var site = new Site("Site1");
            var template = new Layout(site, templateName);

            string expected1 = Path.Combine(site.PhysicalPath, "templates", "layouts", templateName);

            Assert.AreEqual(expected1, template.PhysicalPath, true);

            string expected2 = Path.Combine(expected1, "template.aspx");

            Assert.AreEqual(expected2, template.PhysicalTemplateFileName, true);

        }
        [TestMethod]
        public void TestVirtualPath()
        {
            string templateName = "template1";
            var site = new Site("Site1");
            var template = new Layout(site, templateName);


            string expected1 = Kooboo.Web.Url.UrlUtility.Combine(site.VirtualPath, "templates", "layouts", templateName);

            Assert.AreEqual(expected1, template.VirtualPath, true);

            string expected2 = Kooboo.Web.Url.UrlUtility.Combine(expected1, "template.aspx");

            Assert.AreEqual(expected2, template.TemplateFileVirutalPath, true);
        }
        [TestMethod]
        public void TestParseFromPhysicalPath()
        {
            string siteName = "site1";
            string templateName = "template1";
            string physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sites", siteName, "templates", "layouts", templateName);

            var template = new Layout(physicalPath);

            Assert.AreEqual(templateName, template.Name);
            Assert.IsTrue(template.IsDummy);

            Assert.IsTrue(template.Site.IsDummy);
            Assert.AreEqual(siteName, template.Site.Name);
        }
    }
}
