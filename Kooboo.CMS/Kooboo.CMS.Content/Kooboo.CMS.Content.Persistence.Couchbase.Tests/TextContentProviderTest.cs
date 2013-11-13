using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Content.Models;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.CMS.Content.Persistence.Couchbase.Tests
{
    [TestClass]
    public class TextContentProviderTest
    {
        private Repository repository = null;
        private Schema newsSchema = null;
        private TextFolder textFolder = null;
        private TextContent news1 = null;
        private TextContent category1 = null;
        private Category[] categories = null;
        private TextContentProvider provider = new TextContentProvider();
        private SchemaProvider schemaProvider = new SchemaProvider();

        public TextContentProviderTest()
        {
            repository = new Repository("TextContentProviderTest");

            newsSchema = new Schema(repository, "News");
            newsSchema.AddColumn(new Column() { Name = "title", DataType = Common.DataType.String });
            newsSchema.AddColumn(new Column() { Name = "Body", DataType = Common.DataType.String });
            schemaProvider.Initialize(newsSchema);

            textFolder = new TextFolder(repository, "News") { SchemaName = newsSchema.Name };

            news1 = new TextContent(repository.Name, newsSchema.Name, textFolder.FullName);
            news1["Title"] = "title1";
            news1["body"] = "body1";

            category1 = new TextContent(repository.Name, "category", "category");
            category1["Title"] = "category1";

            categories = new Category[] { new Category() { ContentUUID = news1.UUID, CategoryFolder = category1.FolderName, CategoryUUID = category1.UUID } };
        }

        //[TestMethod]
        //public void Test_CreateSchemaViews()
        //{
        //    var flag = repository.CreateSchemaViews(newsSchema);
        //    Assert.AreEqual(true, flag);
        //}

        [TestMethod]
        public void Test_AddCategories()
        {
            provider.Add(news1);
            provider.Add(category1);
            provider.AddCategories(news1, categories);
            provider.ClearCategories(news1);
            provider.AddCategories(news1, categories);
            provider.DeleteCategories(news1, categories);
            provider.Delete(news1);
            provider.Delete(category1);
        }

        [TestMethod]
        public void Test_ExportCategoryData()
        {
            provider.Add(news1);
            provider.Add(category1);
            provider.AddCategories(news1, categories);

            var list = provider.ExportCategoryData(repository);

            provider.DeleteCategories(news1, categories);
            provider.Delete(news1);
            provider.Delete(category1);

            Assert.IsTrue(list.Count() > 0);
        }

        [TestMethod]
        public void Test_ExportSchemaData()
        {
            provider.Add(news1);
            provider.Add(category1);
            provider.AddCategories(news1, categories);

            var list = provider.ExportSchemaData(newsSchema);

            provider.DeleteCategories(news1, categories);
            provider.Delete(news1);
            provider.Delete(category1);

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void Test_ImportCategoryData()
        {
            provider.Add(news1);
            provider.Add(category1);
            provider.ImportCategoryData(repository,categories);

            provider.DeleteCategories(news1, categories);
            provider.Delete(news1);
            provider.Delete(category1);
        }

        [TestMethod]
        public void Test_QueryCategories()
        {
            provider.Add(news1);
            provider.Add(category1);
            provider.AddCategories(news1, categories);

            var ret = provider.QueryCategories(news1);

            provider.DeleteCategories(news1, categories);
            provider.Delete(news1);
            provider.Delete(category1);

            Assert.AreEqual(ret.Count(), categories.Length);
        }

        [TestMethod]
        public void Test_Update()
        {
            provider.Add(news1);
            provider.Add(category1);
            provider.AddCategories(news1, categories);

            var news2=new TextContent(repository.Name, newsSchema.Name, textFolder.FullName);
            news2["Title"] = "title2";
            news2["body"] = "body2";

            provider.Update(news2, news1);

            provider.DeleteCategories(news1, categories);
            provider.Delete(news1);
            provider.Delete(category1);
        }

        //[TestMethod]
        //public void Test_DeleteSchemaViews()
        //{
        //    var flag = repository.DeleteSchemaViews(newsSchema);
        //    repository.CreateSchemaViews(newsSchema);
        //    Assert.AreEqual(true, flag);

        //}
    }
}
