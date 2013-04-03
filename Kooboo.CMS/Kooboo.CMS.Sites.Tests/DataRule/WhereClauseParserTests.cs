using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.Models;
using System.Collections.Specialized;
using Kooboo.CMS.Content.Query.Translator.String;

namespace Kooboo.CMS.Sites.Tests.DataRule
{
    /// <summary>
    /// Summary description for WhereClauseParser
    /// </summary>
    [TestClass]
    public class WhereClauseParserTests
    {
        private class TranslatedQueryMock : TranslatedQuery
        {
        }
        [TestMethod]
        public void Test1()
        {
            DataRuleContext context = new DataRuleContext(null, null) { ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection()) };
            WhereClause[] clauses = new WhereClause[] { 
                new WhereClause(){ FieldName="title", Operator = Operator.Contains, Value1= "title1",Logical = Logical.Or },
                new WhereClause(){ FieldName="title", Operator = Operator.Contains,Value1="title1",Logical = Logical.ThenAnd },
                new WhereClause(){ FieldName="body", Operator = Operator.Contains,Value1="body1",Logical = Logical.Or },
                new WhereClause(){ FieldName="body", Operator = Operator.Contains,Value1="body2",Logical = Logical.ThenOr },
                new WhereClause(){ FieldName="userkey", Operator = Operator.Contains,Value1="userkey1",Logical = Logical.Or }
            };
            var expression = WhereClauseToContentQueryHelper.Parse(clauses, context);
            var query = new TranslatedQueryMock();
            StringVisitor visitor = new StringVisitor(query);
            visitor.Visite(expression);
            Assert.AreEqual("((((((title Conatins title1) OR (title Conatins title1))) AND (((body Conatins body1) OR (body Conatins body2))))) OR (userkey Conatins userkey1))", query.ClauseText);
        }
        [TestMethod]
        public void Test2()
        {
            DataRuleContext context = new DataRuleContext(null, null) { ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection()) };
            WhereClause[] clauses = new WhereClause[] { 
                new WhereClause(){ FieldName="title", Operator = Operator.Contains, Value1= "title1",Logical = Logical.Or },
                new WhereClause(){ FieldName="title", Operator = Operator.Contains,Value1="title1",Logical = Logical.And },
                new WhereClause(){ FieldName="body", Operator = Operator.Contains,Value1="body1",Logical = Logical.Or },
                new WhereClause(){ FieldName="body", Operator = Operator.Contains,Value1="body2",Logical = Logical.Or },
                new WhereClause(){ FieldName="userkey", Operator = Operator.Contains,Value1="userkey1",Logical = Logical.Or }
            };
            var expression = WhereClauseToContentQueryHelper.Parse(clauses, context);
            var query = new TranslatedQueryMock();
            StringVisitor visitor = new StringVisitor(query);
            visitor.Visite(expression);
            Assert.AreEqual("((((((((title Conatins title1) OR (title Conatins title1))) AND (body Conatins body1))) OR (body Conatins body2))) OR (userkey Conatins userkey1))", query.ClauseText);
        }
    }
}
