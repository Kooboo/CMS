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
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Content.Persistence.Sqlce.Tests
{
    public class EmptyUserKeyGenerator : UserKeyGenerator
    {
        public override string Generate(ContentBase content)
        {
            return content.UserKey;
        }
    }
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
        TextContent categoryContent;
        TextContent commenContent;

        TextContent news2;
        TextContent news3;

        TextContentProvider provider = new TextContentProvider();
        public QueryTests()
        {
            EmptyUserKeyGenerator.DefaultGenerator = new EmptyUserKeyGenerator();
            Providers.DefaultProviderFactory = new Sqlce.ProviderFactory();

            repository = new Repository(Guid.NewGuid().ToString());
            Providers.DefaultProviderFactory.GetProvider<IRepositoryProvider>().Add(repository);

            categorySchema = new Schema(repository, "category") { IsDummy = false };
            categorySchema.AddColumn(new Column() { Name = "Title" });
            Providers.DefaultProviderFactory.GetProvider<ISchemaProvider>().Add(categorySchema);
            categoryFolder = new TextFolder(repository, "Category") { SchemaName = categorySchema.Name, IsDummy = false };
            Providers.DefaultProviderFactory.GetProvider<ITextFolderProvider>().Add(categoryFolder);

            newsSchema = new Schema(repository, "News") { IsDummy = false };
            newsSchema.AddColumn(new Column() { Name = "title", DataType = DataType.String, Length = 256 });
            newsSchema.AddColumn(new Column() { Name = "Body", DataType = DataType.String });
            newsSchema.AddColumn(new Column() { Name = "Comments", DataType = DataType.Int });
            Providers.DefaultProviderFactory.GetProvider<ISchemaProvider>().Add(newsSchema);
            newsFolder = new TextFolder(repository, "News") { SchemaName = newsSchema.Name, Categories = new List<CategoryFolder>() { new CategoryFolder() { FolderName = categoryFolder.FullName, SingleChoice = false } }, OrderSetting = new OrderSetting() { FieldName = "Sequence", Direction = OrderDirection.Descending } };
            Providers.DefaultProviderFactory.GetProvider<ITextFolderProvider>().Add(newsFolder);

            commentSchema = new Schema(repository, "Comment") { IsDummy = false };
            commentSchema.AddColumn(new Column() { Name = "Title" });
            Providers.DefaultProviderFactory.GetProvider<ISchemaProvider>().Add(commentSchema);

            categoryContent = new TextContent(repository.Name, categorySchema.Name, categoryFolder.FullName);
            categoryContent["title"] = "category1";
            provider.Add(categoryContent);

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

            provider.AddCategories(newsContent, new Category() { ContentUUID = newsContent.UUID, CategoryUUID = categoryContent.UUID, CategoryFolder = categoryContent.FolderName });

            commenContent = new TextContent(repository.Name, commentSchema.Name, "");
            commenContent.ParentUUID = newsContent.UUID;
            commenContent["title"] = "comment1";
            provider.Add(commenContent);

        }
        [TestMethod]
        public void SimpleQuery()
        {
            var content = newsFolder.CreateQuery().WhereEquals("UUID", newsContent.UUID).FirstOrDefault();
            Assert.IsNotNull(content);
            Assert.AreEqual(newsContent.UUID, content.UUID);
            Assert.AreEqual(newsContent.UtcCreationDate.ToUniversalTime().ToString(), content.UtcCreationDate.ToUniversalTime().ToString());
        }

        [TestMethod]
        public void Skip_FirtOrDefault_Query()
        {
            var content = newsFolder.CreateQuery().WhereEquals("UUID", newsContent.UUID).Skip(1).FirstOrDefault();
            Assert.IsNull(content);
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
            var content = newsFolder.CreateQuery().WhereCategory(categoryFolder.CreateQuery().WhereEquals("UUID", categoryContent.UUID)).FirstOrDefault();
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
            Assert.AreEqual(1, count);
        }
        [TestMethod]
        public void Query_WhereCategory_CategoriesQuery()
        {
            var categories = newsContent.Categories(categoryFolder);
            var newsInCategories = newsFolder.CreateQuery().WhereCategory(categories);
            Assert.AreEqual(1, newsInCategories.Count());
        }
        [TestMethod]
        public void Query_WhereBetween()
        {
            var count = newsFolder.CreateQuery().WhereBetween("Comments", 1, 2).Count();
            Assert.AreEqual(0, count);
        }
        [TestMethod]
        public void Order_ThenOrder()
        {
            var news = newsFolder.CreateQuery().OrderBy("Title").OrderByDescending("Comments");
            Assert.AreEqual(newsContent["Title"], news.First()["Title"].ToString());
            Assert.AreEqual(news2["Title"], news.Last()["Title"].ToString());

            news = newsFolder.CreateQuery().OrderBy("Comments").OrderBy("Title");
            Assert.AreEqual(news2["UUID"], news.First()["UUID"].ToString());
        }

        [TestMethod]
        public void Test_ExecuteQuery()
        {
            var contents = repository.ExecuteQuery(string.Format("SELECT * FROM [{0}]", newsSchema.GetTableName()), System.Data.CommandType.Text);
            Assert.AreEqual(3, contents.Count());

            var count = repository.ExecuteScalar(string.Format("SELECT COUNT(*) FROM [{0}] WHERE UUID=@uuid", newsSchema.GetTableName()), System.Data.CommandType.Text,
                 new[] { new KeyValuePair<string, object>("@uuid", newsContent.UUID) });
            Assert.AreEqual(1, count);
        }
        [TestMethod]
        public void Test_Where_Clause()
        {
            var news = newsFolder.CreateQuery().Where(string.Format("UUID='{0}'", newsContent.UUID));
            Assert.AreEqual(1, news.Count());
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
        [TestMethod]
        public void OrderByTitle()
        {
            var news = newsFolder.CreateQuery().WhereEquals("Published", null).OrderBy("Title");
            Assert.AreEqual(newsContent.UUID, news.First().UUID);
        }
        [TestMethod]
        public void Add_with_Transaction_Commit()
        {
            TextContent content1 = null;
            using (var unit = provider.CreateTransaction(repository))
            {
                content1 = new TextContent(repository.Name, newsSchema.Name, newsFolder.FullName);
                content1["title"] = "Add_with_Transaction_Commit";
                content1["body"] = "Add_with_Transaction_Commit";
                content1["comments"] = 1;
                provider.Add(content1);

                unit.Commit();
            }
            var news = newsFolder.CreateQuery().WhereEquals("UUID", content1.UUID);
            Assert.AreEqual(1, news.Count());

        }
        [TestMethod]
        public void Add_with_Transaction_Rollback()
        {
            TextContent content1 = null;
            try
            {
                using (var unit = provider.CreateTransaction(repository))
                {
                    content1 = new TextContent(repository.Name, newsSchema.Name, newsFolder.FullName);
                    content1["title"] = "Add_with_Transaction_Rollback";
                    content1["body"] = "Add_with_Transaction_Rollback";
                    content1["comments"] = 1;
                    provider.Add(content1);

                    throw new Exception();

                    unit.Commit();
                }
            }
            catch
            {

            }
            var news = newsFolder.CreateQuery().WhereEquals("UUID", content1.UUID);
            Assert.AreEqual(0, news.Count());

        }

        [TestMethod]
        public void WhereNot()
        {
            var news = newsFolder.CreateQuery().WhereNot(new WhereContainsExpression("title", "notintitle"));
            Assert.AreEqual(3, news.Count());
        }
    }
}
