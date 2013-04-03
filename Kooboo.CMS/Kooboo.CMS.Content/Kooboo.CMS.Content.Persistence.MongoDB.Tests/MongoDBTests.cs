using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver.Builders;
using QueryBuilder = MongoDB.Driver.Builders;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;
namespace Kooboo.CMS.Content.Persistence.MongoDB.Tests
{
    [TestClass]
    public class MongoDBTests
    {
        [TestMethod]
        public void Connect_Server()
        {
            string connectionString = "mongodb://localhost";
            MongoServer server = MongoServer.Create(connectionString);
            server.Connect();
            Assert.AreEqual(MongoServerState.Connected, server.State);
        }
        [TestMethod]
        public void Create_MongoDB()
        {
            string connectionString = "mongodb://localhost";
            MongoServer server = MongoServer.Create(connectionString);
            var database = server.GetDatabase("MongoDBTests");
            Assert.IsNotNull(database);
        }
        [TestMethod]
        public void Query_BsonJavascript()
        {
            string connectionString = "mongodb://localhost";
            MongoServer server = MongoServer.Create(connectionString);
            var database = server.GetDatabase("MongoDBTests");

            var data = database.GetCollection("Where_BsonJavascript");
            BsonDocument doc = new BsonDocument();
            doc["value"] = 10;

            data.Insert(doc);

            var query = QueryBuilder.Query.Where(new BsonJavaScript("this.value>9 && this.value<11"));
            doc = data.Find(query).FirstOrDefault();
            Assert.IsNotNull(doc);
            Assert.AreEqual(10, doc["value"].ToInt32());
        }
        [TestMethod]
        public void Query()
        {
            string connectionString = "mongodb://localhost";
            MongoServer server = MongoServer.Create(connectionString);
            var database = server.GetDatabase("MongoDBTests");

            var data = database.GetCollection("Where_BsonJavascript");
            BsonDocument doc = new BsonDocument();
            doc["value"] = 10;

            data.Insert(doc);

            var query = QueryBuilder.Query.GT("value", BsonValue.Create(9)).LT(BsonValue.Create(11));
            doc = data.Find(query).FirstOrDefault();
            Assert.IsNotNull(doc);
            Assert.AreEqual(10, doc["value"].ToInt32());
        }
        [TestMethod]
        public void Where_Contains_Javascript()
        {
            string connectionString = "mongodb://localhost";
            MongoServer server = MongoServer.Create(connectionString);
            var database = server.GetDatabase("MongoDBTests");

            var data = database.GetCollection("Where_BsonJavascript");
            BsonDocument doc = new BsonDocument();
            doc["value"] = "abcdef";
            data.Insert(doc);

            var query = QueryBuilder.Query.Where(new BsonJavaScript("this.value.toString().toLowerCase().indexOf('abc') != -1"));
            doc = data.Find(query).FirstOrDefault();
            Assert.IsNotNull(doc);
            Assert.AreEqual("abcdef", doc["value"].ToString());
        }
        [TestMethod]
        public void Where_Contains_RegularExpression()
        {
            string connectionString = "mongodb://localhost";
            MongoServer server = MongoServer.Create(connectionString);
            var database = server.GetDatabase("MongoDBTests");

            var data = database.GetCollection("Where_BsonJavascript");
            BsonDocument doc = new BsonDocument();
            doc["value"] = "ab'|cdef";
            data.Insert(doc);

            var query = QueryBuilder.Query.Matches("value", new BsonRegularExpression(Regex.Escape("ab'|c"), "i"));
            doc = data.Find(query).FirstOrDefault();
            Assert.IsNotNull(doc);
            Assert.AreEqual("ab'|cdef", doc["value"].ToString());
        }
    }
}
