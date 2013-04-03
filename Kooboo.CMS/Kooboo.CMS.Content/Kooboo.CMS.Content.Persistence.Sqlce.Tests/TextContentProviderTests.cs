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
    /// Summary description for TextContentProviderTests
    /// </summary>
    [TestClass]
    public class TextContentProviderTests
    {
        static Repository repository = null;
        static Schema newsSchema;
        static TextFolder newsFolder;
        static Schema categorySchema;
        static TextFolder categoryFolder;
        static TextContentProvider textContentProvider = new TextContentProvider();

        static TextContentProviderTests()
        {
            repository = new Repository("TextContentProviderTests");

            categorySchema = new Schema(repository, "category");
            categorySchema.AddColumn(new Column() { Name = "title", DataType = DataType.String, Length = 256 });

            categoryFolder = new TextFolder(repository, "category") { SchemaName = categorySchema.Name };

            newsSchema = new Schema(repository, "news");
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

            newsFolder = new TextFolder(repository, "news") { SchemaName = newsSchema.Name, CategoryFolders = new[] { categoryFolder.Name } };

            ProviderFactory providerFactory = new ProviderFactory();
            Providers.DefaultProviderFactory = providerFactory;
            providerFactory.GetProvider<IRepositoryProvider>().Add(repository);
            providerFactory.GetProvider<ISchemaProvider>().Add(categorySchema);
            providerFactory.GetProvider<ISchemaProvider>().Add(newsSchema);
            providerFactory.GetProvider<ITextFolderProvider>().Add(newsFolder);
            providerFactory.GetProvider<ITextFolderProvider>().Add(categoryFolder);
        }
        [TestMethod]
        public void TestContentCUD()
        {
            TextContent textContent = new TextContent();
            textContent.FolderName = newsFolder.FullName;
            textContent.SchemaName = newsSchema.Name;
            textContent.Repository = repository.Name;
            textContent.UtcCreationDate = DateTime.UtcNow;
            textContent.UtcLastModificationDate = DateTime.UtcNow;
            textContent.UserKey = "userkey1";


            textContent["Title"] = "news1";
            textContent["Comments"] = 100;

            textContentProvider.Add(textContent);

            textContent["Title"] = "news1-updated";
            textContent["Comments"] = 101;
            textContentProvider.Update(textContent, textContent);

            textContentProvider.Delete(textContent);

        }
        [TestMethod]
        public void TestAddCategory()
        {
            dynamic category1 = new TextContent(repository.Name, categorySchema.Name, categoryFolder.Name)
            {
                UserKey = "category1"
            };
            category1.Title = "category1";

            textContentProvider.Add(category1);

            dynamic category2 = new TextContent(repository.Name, categorySchema.Name, categoryFolder.Name)
            {
                UserKey = "category2"
            };
            category2.Title = "category2";

            textContentProvider.Add(category2);

            dynamic news1 = new TextContent(repository.Name, newsSchema.Name, newsFolder.Name)
            {
                UserKey = "news1"
            };
            news1.title = "news1";
            textContentProvider.Add(news1);

            textContentProvider.AddCategories(news1, new Category() { ContentUUID = news1.uuid, CategoryFolder = categoryFolder.FullName, CategoryUUID = (string)(category1.UUID) },
                 new Category() { ContentUUID = news1.uuid, CategoryFolder = categoryFolder.FullName, CategoryUUID = (string)(category2.UUID) });

            textContentProvider.DeleteCategories(news1, new Category() { ContentUUID = news1.uuid, CategoryFolder = categoryFolder.FullName, CategoryUUID = (string)(category2.UUID) });

        }


        [TestMethod]
        public void TestClearCategory()
        {
            var contentProvider = new TextContentProvider();
            dynamic category1 = new TextContent(repository.Name, categorySchema.Name, categoryFolder.Name);
            category1.UserKey = "TestClearCategory-category1";
            category1.Title = "TestClearCategory-category1";
            contentProvider.Add(category1);
            dynamic content1 = new TextContent(repository.Name, newsSchema.Name, newsFolder.Name);
            content1.UserKey = "TestClearCategory-news1";
            content1.Title = "TestClearCategory-news1";
            contentProvider.Add(content1);
            contentProvider.AddCategories(content1, new Category() { ContentUUID = content1.uuid, CategoryFolder = categoryFolder.Name, CategoryUUID = category1.UUID });

            contentProvider.ClearCategories(content1);
        }

        [TestMethod]
        public void TestUserKey()
        {
            dynamic category1 = new TextContent(repository.Name, categorySchema.Name, categoryFolder.Name)
            {
                UserKey = "userkey1"
            };
            category1.Title = "category1";

            textContentProvider.Add(category1);

            dynamic category2 = new TextContent(repository.Name, categorySchema.Name, categoryFolder.Name)
            {
                UserKey = "userkey1"
            };
            category2.Title = "category2";

            textContentProvider.Add(category2);

            Assert.AreEqual("userkey1-1", category2.UserKey);

        }


        [TestMethod]
        public void TestExport()
        {
            var news = textContentProvider.ExportSchemaData(newsSchema).ToArray();
            var categoryData = textContentProvider.ExportCategoryData(repository).ToArray();
        }
        [TestMethod]
        public void TestImport()
        {
            var category = new TextContent(categorySchema.Repository.Name, categorySchema.Name, categoryFolder.FullName);
            category.UserKey = "import-category";
            category["Title"] = "import-category";


            var news1 = new TextContent(newsSchema.Repository.Name, newsSchema.Name, newsFolder.FullName);
            news1.UserKey = "import-news";
            news1["Title"] = "import-news";
            news1["Body"] = "import-news";

            textContentProvider.ImportSchemaData(categorySchema, new[] { category });
            textContentProvider.ImportSchemaData(newsSchema, new[] { news1 });
            textContentProvider.ImportCategoryData(repository, new[] { new Category() { ContentUUID = news1.UUID, CategoryFolder = category.FolderName, CategoryUUID = category.UUID } });

        }


    }
}
