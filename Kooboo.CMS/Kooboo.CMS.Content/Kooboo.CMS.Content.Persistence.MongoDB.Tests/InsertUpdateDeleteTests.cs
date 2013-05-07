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
using Kooboo.CMS.Content.Persistence.MongoDB;
namespace Kooboo.CMS.Content.Persistence.MongoDB.Tests
{
    public class EmptyUserKeyGenerator : UserKeyGenerator
    {
        public override string Generate(ContentBase content)
        {
            return content.UserKey;
        }
    }
    /// <summary>
    /// Summary description for TextContentProviderTests
    /// </summary>
    [TestClass]
    public class InsertUpdateDeleteTests
    {
        Repository repository = null;
        Schema newsSchema = null;
        TextFolder textFolder = null;
        TextContentProvider provider = new TextContentProvider();
        public InsertUpdateDeleteTests()
        {
            repository = new Repository("InsertUpdateDeleteTests");
            newsSchema = new Schema(repository, "News");
            newsSchema.AddColumn(new Column() { Name = "title", DataType =  Common.DataType.String });
            newsSchema.AddColumn(new Column() { Name = "Body", DataType = Common.DataType.String });
            textFolder = new TextFolder(repository, "News") { SchemaName = newsSchema.Name };

            EmptyUserKeyGenerator.DefaultGenerator = new EmptyUserKeyGenerator();
        }
        [TestMethod]
        public void Insert()
        {
            TextContent news1 = new TextContent(repository.Name, newsSchema.Name, textFolder.FullName);
            news1["Title"] = "title1";
            news1["body"] = "body1";
            provider.Add(news1);
        }
        [TestMethod]
        public void Update()
        {
            TextContent news1 = new TextContent(repository.Name, newsSchema.Name, textFolder.FullName);
            news1["Title"] = "title1";
            news1["body"] = "body1";
            provider.Add(news1);

            news1["title"] = "title1=update";

            provider.Update(news1, news1);
        }
        [TestMethod]
        public void Delete()
        {
            TextContent news1 = new TextContent(repository.Name, newsSchema.Name, textFolder.FullName);
            news1["Title"] = "title1";
            news1["body"] = "body1";
            provider.Add(news1);

            provider.Delete(news1);
        }
        [TestMethod]
        public void Insert_Delete_Category()
        {
            TextContent news1 = new TextContent(repository.Name, newsSchema.Name, textFolder.FullName);
            news1["Title"] = "title1";
            news1["body"] = "body1";
            provider.Add(news1);

            TextContent category1 = new TextContent(repository.Name, "category", "category");
            category1["Title"] = "category1";
            provider.Add(category1);

            var categories = new Category[] { new Category() { ContentUUID = news1.UUID, CategoryFolder = category1.FolderName, CategoryUUID = category1.UUID } };

            provider.AddCategories(news1, categories);
            provider.DeleteCategories(news1, categories);
            provider.AddCategories(news1, categories);
            provider.ClearCategories(news1);

            provider.ClearCategories(news1);

        }
        [TestMethod]
        public void Export_Schema_Data()
        {
            TextContent news1 = new TextContent(repository.Name, newsSchema.Name, textFolder.FullName);
            news1["Title"] = "title1";
            news1["body"] = "body1";
            provider.Add(news1);

            var news = provider.ExportSchemaData(newsSchema);


            Assert.IsTrue(news.Count() > 1);
        }
        [TestMethod]
        public void Export_Categories_Data()
        {
            TextContent news1 = new TextContent(repository.Name, newsSchema.Name, textFolder.FullName);
            news1["Title"] = "title1";
            news1["body"] = "body1";
            provider.Add(news1);

            TextContent category1 = new TextContent(repository.Name, "category", "category");
            category1["Title"] = "category1";
            provider.Add(category1);

            var categories = new Category[] { new Category() { ContentUUID = news1.UUID, CategoryFolder = category1.FolderName, CategoryUUID = category1.UUID } };

            provider.AddCategories(news1, categories);

            var exportedCategories = provider.ExportCategoryData(repository);

            Assert.IsTrue(exportedCategories.Count() > 1);
        }
    }
}
