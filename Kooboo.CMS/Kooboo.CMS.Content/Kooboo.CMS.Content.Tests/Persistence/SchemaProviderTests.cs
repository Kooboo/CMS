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
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;

using Kooboo.CMS.Form;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Content.Tests.Persistence
{
    /// <summary>
    /// Summary description for ContentRepository_RepositoryTests
    /// </summary>
    [TestClass]
    public class SchemaProviderTests
    {
        string repositoryName = "SchemaProviderTests";
        Repository contentRepository;
        public SchemaProviderTests()
        {
            contentRepository = new Repository(repositoryName) { DisplayName = "DisplayName" };
        }

        [TestMethod]
        public void Test1()
        {
            SchemaProvider repository = new SchemaProvider();
            var schemaName = "schema1";
            List<Column> columns = new List<Column>()
            {
                new Column(){
                    Name="Title",
                    DataType=DataType.String,
                    ControlType="Text",
                    AllowNull=false,
                    Validations = new []{new RequiredValidation()}
                }
            };
            var schema = new Schema(contentRepository, schemaName, columns);

            repository.Add(schema);

            var all = repository.All(contentRepository);

            Assert.AreEqual(1, all.Count());

            var item = repository.Get(all.First());

            Assert.AreEqual(schema.Name, item.Name);
            Assert.AreEqual(schema.Repository, item.Repository);
            Assert.AreEqual(schema.Columns.Count(), item.Columns.Count());
            Assert.AreEqual(schema.Columns.First().Name, item.Columns.First().Name);            

            repository.Remove(item);
        }
    }
}
