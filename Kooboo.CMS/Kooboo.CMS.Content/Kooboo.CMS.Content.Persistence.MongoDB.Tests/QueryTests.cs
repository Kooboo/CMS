#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence.MongoDB;
using Kooboo.CMS.Content.Query.Expressions;
namespace Kooboo.CMS.Content.Persistence.MongoDB.Tests
{
    [TestClass]
    public class QueryTests
    {
        Repository repository = null;
        Schema newsSchema = null;
        Schema categorySchema = null;
        Schema commentSchema = null;
        TextFolder newsFolder = null;
        TextFolder categoryFolder = null;

        TextContent newsContent;
        TextContent category1;
        TextContent category2;
        TextContent commenContent;
        TextContent news2;
        TextContent news3;

        TextContentProvider provider = new TextContentProvider();
        public QueryTests()
        {
            EmptyUserKeyGenerator.DefaultGenerator = new EmptyUserKeyGenerator();
            Providers.DefaultProviderFactory = new MongoDB.ProviderFactory();

            repository = new Repository(Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10).ToString());
            Providers.DefaultProviderFactory.GetProvider<IRepositoryProvider>().Add(repository);
            categorySchema = new Schema(repository, "category") { IsDummy = false };
            categorySchema.AddColumn(new Column() { Name = "Title" });
            categoryFolder = new TextFolder(repository, "Category") { SchemaName = categorySchema.Name, IsDummy = false };
            Providers.DefaultProviderFactory.GetProvider<ITextFolderProvider>().Add(categoryFolder);

            newsSchema = new Schema(repository, "News") { IsDummy = false };
            newsSchema.AddColumn(new Column() { Name = "title", DataType = Common.DataType.String });
            newsSchema.AddColumn(new Column() { Name = "Body", DataType = Common.DataType.String });
            newsSchema.AddColumn(new Column() { Name = "Comments", DataType = Common.DataType.Int });
            Providers.DefaultProviderFactory.GetProvider<ISchemaProvider>().Add(newsSchema);

            newsFolder = new TextFolder(repository, "News") { SchemaName = newsSchema.Name, Categories = new List<CategoryFolder>() { new CategoryFolder() { FolderName = categoryFolder.FullName, SingleChoice = false } }, OrderSetting = new OrderSetting() { FieldName = "Sequence", Direction = OrderDirection.Descending } };
            Providers.DefaultProviderFactory.GetProvider<ITextFolderProvider>().Add(newsFolder);

            commentSchema = new Schema(repository, "Comment") { IsDummy = false };
            commentSchema.AddColumn(new Column() { Name = "Title" });
            Providers.DefaultProviderFactory.GetProvider<ISchemaProvider>().Add(commentSchema);

            category1 = new TextContent(repository.Name, categorySchema.Name, categoryFolder.FullName);
            category1["title"] = "category1";
            provider.Add(category1);

            category2 = new TextContent(repository.Name, categorySchema.Name, categoryFolder.FullName);
            category2["title"] = "category2";
            provider.Add(category2);

            newsContent = new TextContent(repository.Name, newsSchema.Name, newsFolder.FullName);
            newsContent["title"] = "news1";
            newsContent["body"] = "body";
            newsContent["comments"] = 1;
            provider.Add(newsContent);

            news2 = new TextContent(repository.Name, newsSchema.Name, newsFolder.FullName);
            news2["title"] = "news2";
            news2["body"] = "body";
            news2["comments"] = 0;
            provider.Add(news2);

            news3 = new TextContent(repository.Name, newsSchema.Name, newsFolder.FullName);
            news3["title"] = "news2";
            news3["body"] = "body";
            news3["comments"] = 5;
            provider.Add(news3);

            provider.AddCategories(newsContent, new Category() { ContentUUID = newsContent.UUID, CategoryUUID = category1.UUID, CategoryFolder = category1.FolderName });
            provider.AddCategories(newsContent, new Category() { ContentUUID = newsContent.UUID, CategoryUUID = category2.UUID, CategoryFolder = category2.FolderName });

            commenContent = new TextContent(repository.Name, commentSchema.Name, "");
            commenContent.ParentFolder = newsContent.FolderName;
            commenContent.ParentUUID = newsContent.UUID;
            commenContent["title"] = "comment1";
            provider.Add(commenContent);

        }
        [TestMethod]
        public void SimpleQuery()
        {
            var content = newsFolder.CreateQuery().WhereEquals("UUID", newsContent.UUID).FirstOrDefault();
            Assert.IsNotNull(content);
            Assert.AreEqual(content.UUID, newsContent.UUID);
        }
        [TestMethod]
        public void Query_Case_Insensitive()
        {
            var content = new TextFolder(repository, "news", null) { SchemaName = newsSchema.Name.ToLower(), IsDummy = false }.CreateQuery().WhereEquals("UUID", newsContent.UUID).FirstOrDefault();
            Assert.IsNotNull(content);
            Assert.AreEqual(content.UUID, newsContent.UUID);

            content = newsFolder.CreateQuery().WhereEquals("UUID", newsContent.UUID.ToUpper()).FirstOrDefault();
            Assert.IsNotNull(content);
        }
        [TestMethod]
        public void Query_By_Category()
        {
            var content = newsFolder.CreateQuery().WhereCategory(categoryFolder.CreateQuery().WhereEquals("UUID", category1.UUID)).FirstOrDefault();
            Assert.IsNotNull(content);
        }
        [TestMethod]
        public void Query_By_Category2()
        {
            var content = newsFolder.CreateQuery()
                .WhereCategory(categoryFolder.CreateQuery().WhereEquals("UUID", category1.UUID))
                .WhereCategory(categoryFolder.CreateQuery().WhereEquals("UUID", category2.UUID))
                .FirstOrDefault();
            Assert.IsNotNull(content);
        }
        [TestMethod]
        public void Query_Category()
        {
            var category = newsFolder.CreateQuery().WhereEquals("UUID", newsContent.UUID).Categories(categoryFolder).FirstOrDefault();
            Assert.IsNotNull(category);
        }

        [TestMethod]
        public void Query_Parent()
        {
            var parent = commentSchema.CreateQuery().WhereEquals("UUID", commenContent.UUID).Parent(newsSchema).FirstOrDefault();
            Assert.IsNotNull(parent);
        }
        [TestMethod]
        public void Query_Children()
        {
            var child = newsSchema.CreateQuery().WhereEquals("UUID", newsContent.UUID).Children(commentSchema);
            Assert.AreEqual(1, child.Count());
        }
        [TestMethod]
        public void Query_True_AndAlso_False()
        {
            var newsQuery = newsFolder.CreateQuery().Where(new FalseExpression());
            Assert.AreEqual(0, newsQuery.Count());

            newsQuery = newsFolder.CreateQuery().Where(new TrueExpression());
            Assert.AreEqual(3, newsQuery.Count());

            newsQuery = newsFolder.CreateQuery().Where(new AndAlsoExpression(new FalseExpression(), new TrueExpression()));
            Assert.AreEqual(0, newsQuery.Count());
        }
        [TestMethod]
        public void Query_WhereLessThan()
        {
            var count = newsFolder.CreateQuery().WhereLessThan("Comments", 1).Count();
            Assert.AreEqual(0, count);
        }
        [TestMethod]
        public void Query_WhereCategory_CategoriesQuery()
        {
            var categories = newsContent.Categories(categoryFolder);
            var newsInCategories = newsFolder.CreateQuery().WhereCategory(categories);
            Assert.AreEqual(1, newsInCategories.Count());
        }

        [TestMethod]
        public void Order_ThenOrder()
        {
            var news = newsFolder.CreateQuery().OrderBy("title").OrderByDescending("comments");
            Assert.AreEqual(newsContent["UUID"], news.First()["UUID"].ToString());
            Assert.AreEqual(news2["UUID"], news.Last()["UUID"].ToString());
            //All field capital will effect the order by.
            news = newsFolder.CreateQuery().OrderBy("comments").OrderBy("title");
            Assert.AreEqual(news2["UUID"], news.First()["UUID"].ToString());
        }
        [TestMethod]
        public void WhereIn()
        {
            var news = newsFolder.CreateQuery().WhereEquals("Published", null).WhereIn("UUID", new[] { newsContent.UUID, news2.UUID });
            Assert.AreEqual(2, news.Count());
        }
        [TestMethod]
        public void WhereNotIn()
        {
            var news = newsFolder.CreateQuery().WhereEquals("Published", null).WhereNotIn("UUID", new[] { newsContent.UUID, news2.UUID });
            Assert.AreEqual(1, news.Count());
        }
        [ExpectedException(typeof(NotSupportedException))]
        [TestMethod]
        public void Test_ExecuteQuery()
        {
            var contents = repository.ExecuteQuery(string.Format("db.{0}.{1}.find({{ }}).limit(50);", repository.Name, newsSchema.Name));
        }

        [TestMethod]
        public void Test_UpdateFields()
        {
            Kooboo.CMS.Content.Services.ServiceFactory.TextContentManager.Update(repository, newsSchema, news3.UUID, new string[] { "Published" }, new object[] { true });

            var news = newsFolder.CreateQuery().WhereEquals("UUID", news3.UUID).First();

            Assert.AreEqual(true, news.Published);
        }
        //https://jira.mongodb.org/browse/SERVER-2585
        [TestMethod]
        public void Test_NestedOr()
        {
            IWhereExpression exp = new FalseExpression();

            exp = new OrElseExpression(exp, (new WhereContainsExpression(null, "title", "news1")));
            exp = new OrElseExpression(exp, (new WhereContainsExpression(null, "title", "news2")));

            var news = newsFolder.CreateQuery().Where(exp);

            Assert.AreEqual(3, news.Count());

        }
        [TestMethod]
        public void OrderByTitle()
        {
            var news = newsFolder.CreateQuery().WhereEquals("Published", null).OrderBy("Title");
            Assert.AreEqual(newsContent.UUID, news.First().UUID);
        }
    }
}
