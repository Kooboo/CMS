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
    public class ChildrenQueryTests
    {
        static Repository repository = null;
        static Schema newsSchema;
        static TextFolder newsFolder;
        static Schema commentSchema;

        static TextContentProvider textContentProvider = new TextContentProvider();
        static ChildrenQueryTests()
        {
            repository = new Repository("ChildrenQueryTests");

            commentSchema = new Schema(repository, "comments");
            commentSchema.AddColumn(new Column() { Name = "title", DataType = DataType.String, Length = 256 });


            newsSchema = new Schema(repository, "news") { ChildSchemas = new[] { commentSchema.Name } };
            newsSchema.AddColumn(new Column()
            {
                Name = "Title",
                DataType = DataType.String,
                Length = 256
            });
            newsSchema.AddColumn(new Column()
            {
                Name = "Comments",
                DataType = DataType.Int
            });

            newsFolder = new TextFolder(repository, "news") { SchemaName = newsSchema.Name };

            ProviderFactory providerFactory = new ProviderFactory();
            Providers.DefaultProviderFactory = providerFactory;
            providerFactory.GetProvider<IRepositoryProvider>().Add(repository);
            providerFactory.GetProvider<ISchemaProvider>().Add(commentSchema);
            providerFactory.GetProvider<ISchemaProvider>().Add(newsSchema);
            providerFactory.GetProvider<ITextFolderProvider>().Add(newsFolder);
            InitializeData();
        }
        [TestMethod]
        public void TestQuery()
        {
            var query = newsSchema.CreateQuery().WhereEquals("title", "news1").Children(commentSchema.Name);

            Assert.AreEqual(2, query.Count());
         
            var first = query.OrderBy("id").First();
            Assert.AreEqual("comment1", first["title"]);
        }


        private static void InitializeData()
        {
            dynamic news1 = new TextContent(repository.Name, newsSchema.Name, newsFolder.Name)
            {
                UserKey = "news1"
            };
            news1.title = "news1";
            textContentProvider.Add(news1);


            dynamic comment1 = new TextContent(repository.Name, commentSchema.Name, null)
            {
                UserKey = "comment1",
                ParentUUID = news1.UUID
            };
            comment1.Title = "comment1";

            textContentProvider.Add(comment1);

            dynamic comment2 = new TextContent(repository.Name, commentSchema.Name, null)
            {
                UserKey = "comment2",
                ParentUUID = news1.UUID
            };
            comment2.Title = "comment2";

            textContentProvider.Add(comment2);
        }
    }
}
