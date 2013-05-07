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

namespace Kooboo.CMS.Content.Tests.Models
{
    /// <summary>
    /// Summary description for SchemaPathTests
    /// </summary>
    [TestClass]
    public class SchemaPathTests
    {
        [TestMethod]
        public void TestPhysicalPath()
        {
            var repositoryName = "repository1";
            var schemaName = "schema1";

            var expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cms_Data","Contents", repositoryName, "schemas", schemaName);

            SchemaPath path = new SchemaPath(new Schema(new Repository(repositoryName), schemaName));

            Assert.AreEqual(expected, path.PhysicalPath, true);

            var settingFile = Path.Combine(expected, "setting.config");
            Assert.AreEqual(settingFile, path.SettingFile, true);
        }
    }
}
