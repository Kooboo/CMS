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
using Kooboo.CMS.Content.Query;
using Kooboo.Data;

namespace Kooboo.CMS.Content.Persistence.Sqlce.Tests
{
    /// <summary>
    /// Summary description for SimpleQueryTests
    /// </summary>
    [TestClass]
    public class SimpleQueryTests
    {
        static Repository repository = null;
        static Schema schema;
        static TextFolder folder;
        static TextContentProvider textContentProvider = new TextContentProvider();
        static SimpleQueryTests()
        {
            repository = new Repository("SimpleQueryTests");
            schema = new Schema(repository, "news");
            schema.AddColumn(new Column()
            {
                Name = "Title",
                DataType = DataType.String,
                Length = 256
            });
            schema.AddColumn(new Column()
            {
                Name = "Comments",
                DataType = DataType.Int
            });

            folder = new TextFolder(repository, "news");
            folder.SchemaName = schema.Name;

            ProviderFactory providerFactory = new ProviderFactory();
            Providers.DefaultProviderFactory = providerFactory;
            providerFactory.GetProvider<IRepositoryProvider>().Add(repository);
            providerFactory.GetProvider<ISchemaProvider>().Add(schema);
            providerFactory.GetProvider<ITextFolderProvider>().Add(folder);

            InitializeData();
        }
        [TestMethod]
        public void TestSimpleQuery()
        {

            var query = folder.CreateQuery();

            var result = textContentProvider.Execute(query) as IEnumerable<TextContent>;

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Count());

            query = folder.CreateQuery().WhereEquals("UserKey", "userkeY1");

            result = textContentProvider.Execute(query) as IEnumerable<TextContent>;

            Assert.AreEqual(1, result.Count());

            query = folder.CreateQuery().WhereBetweenOrEqual("UtcCreationDate", DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);

            result = textContentProvider.Execute(query) as IEnumerable<TextContent>;

            Assert.AreEqual(2, result.Count());

            query = folder.CreateQuery().WhereEquals("Published", true);

            result = textContentProvider.Execute(query) as IEnumerable<TextContent>;

            Assert.AreEqual(2, result.Count());

            query = folder.CreateQuery().WhereNotEquals("UserKey", "userkeY1");

            result = textContentProvider.Execute(query) as IEnumerable<TextContent>;

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void TestCount()
        {
            var query = folder.CreateQuery();

            int count = query.Count();

            Assert.AreEqual(2, count);
        }
        [TestMethod]
        public void TestFirst()
        {
            var query = folder.CreateQuery();
            var content = query.OrderBy("Id").First();
            Assert.AreEqual("userkey1", content.UserKey);

            query = folder.CreateQuery();
            content = query.OrderByDescending("Id").First();
            Assert.AreEqual("userkey2", content.UserKey);
        }

        [TestMethod]
        public void TestLast()
        {
            var query = folder.CreateQuery();
            var content = query.OrderBy("Id").Last();
            Assert.AreEqual("userkey2", content.UserKey);

            query = folder.CreateQuery();
            content = query.OrderByDescending("Id").Last();
            Assert.AreEqual("userkey1", content.UserKey);
        }

        private static void InitializeData()
        {
            TextContent textContent1 = new TextContent();
            textContent1.FolderName = folder.FullName;
            textContent1.SchemaName = schema.Name;
            textContent1.Repository = repository.Name;
            textContent1.UtcCreationDate = DateTime.UtcNow;
            textContent1.UtcLastModificationDate = DateTime.UtcNow;
            textContent1.UserKey = "userkey1";


            textContent1["Title"] = "news1";
            textContent1["Comments"] = 100;
            textContent1.Published = true;
            textContentProvider.Add(textContent1);


            TextContent textContent2 = new TextContent();
            textContent2.FolderName = folder.FullName;
            textContent2.SchemaName = schema.Name;
            textContent2.Repository = repository.Name;
            textContent2.UtcCreationDate = DateTime.UtcNow;
            textContent2.UtcLastModificationDate = DateTime.UtcNow;
            textContent2.UserKey = "userkey2";


            textContent2["Title"] = "news2";
            textContent2["Comments"] = 102;
            textContent2.Published = true;

            textContentProvider.Add(textContent2);
        }
    }
}
