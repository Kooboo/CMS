using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Content.Persistence.Couchbase.Query;

namespace Kooboo.CMS.Content.Persistence.Couchbase.Tests
{
    [TestClass]
    public class CouchbaseVisitorTest
    {
        [TestMethod]
        public void Test_Visit_Where_In()
        {
            var whereIn = new WhereInExpression(null, "UUID", new[] { "value1" });
            CouchbaseVisitor visitor = new CouchbaseVisitor();
            visitor.Visite(whereIn);

            Console.WriteLine(visitor.ViewName);
            Console.WriteLine(visitor.WhereClause);
            Console.WriteLine(visitor.EQUUIDs.First());
        }
    }
}
