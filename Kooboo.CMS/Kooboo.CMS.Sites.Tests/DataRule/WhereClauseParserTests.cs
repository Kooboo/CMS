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
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.Models;
using System.Collections.Specialized;
using Kooboo.CMS.Content.Query.Translator.String;
using Moq;
using Kooboo.CMS.Content.Models;
namespace Kooboo.CMS.Sites.Tests.DataRule
{
    /// <summary>
    /// Summary description for WhereClauseParser
    /// </summary>
    [TestClass]
    public class WhereClauseParserTests
    {
        IValueProvider valueProvider;
        TranslatedQuery translatedQuery;
        Schema schema;

        private class TranslatedQueryMock : TranslatedQuery { }

        public WhereClauseParserTests()
        {
            var mockValueProvider = new Mock<IValueProvider>();
            valueProvider = mockValueProvider.Object;
            translatedQuery = new TranslatedQueryMock();

            schema = new Schema() { IsDummy = false };
            schema.AddColumn(new Column() { Name = "title", DataType = Common.DataType.String });
            schema.AddColumn(new Column() { Name = "body", DataType = Common.DataType.String });
            schema.AddColumn(new Column() { Name = "userkey", DataType = Common.DataType.String });
        }
        [TestMethod]
        public void Test_Or_ThenAnd_OR_ThenOr_Or()
        {
            WhereClause[] clauses = new WhereClause[] { 
                new WhereClause(){ FieldName="title", Operator = Operator.Contains, Value1= "title1",Logical = Logical.Or },
                new WhereClause(){ FieldName="title", Operator = Operator.Contains,Value1="title1",Logical = Logical.ThenAnd },
                new WhereClause(){ FieldName="body", Operator = Operator.Contains,Value1="body1",Logical = Logical.Or },
                new WhereClause(){ FieldName="body", Operator = Operator.Contains,Value1="body2",Logical = Logical.ThenOr },
                new WhereClause(){ FieldName="userkey", Operator = Operator.Contains,Value1="userkey1",Logical = Logical.Or }
            };
            var expression = WhereClauseToContentQueryHelper.Parse(clauses, schema, valueProvider);
            StringVisitor visitor = new StringVisitor(translatedQuery);
            visitor.Visite(expression);
            Assert.AreEqual("((((((title Contains title1) OR (title Contains title1))) AND (((body Contains body1) OR (body Contains body2))))) OR (userkey Contains userkey1))", translatedQuery.ClauseText);
        }
        [TestMethod]
        public void Test_Or_And_Or_Or_Or()
        {
            DataRuleContext context = new DataRuleContext(null, null) { ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection()) };
            WhereClause[] clauses = new WhereClause[] { 
                new WhereClause(){ FieldName="title", Operator = Operator.Contains, Value1= "title1",Logical = Logical.Or },
                new WhereClause(){ FieldName="title", Operator = Operator.Contains,Value1="title1",Logical = Logical.And },
                new WhereClause(){ FieldName="body", Operator = Operator.Contains,Value1="body1",Logical = Logical.Or },
                new WhereClause(){ FieldName="body", Operator = Operator.Contains,Value1="body2",Logical = Logical.Or },
                new WhereClause(){ FieldName="userkey", Operator = Operator.Contains,Value1="userkey1",Logical = Logical.Or }
            };
            var expression = WhereClauseToContentQueryHelper.Parse(clauses, schema, valueProvider);
            StringVisitor visitor = new StringVisitor(translatedQuery);
            visitor.Visite(expression);
            Assert.AreEqual("((((((((title Contains title1) OR (title Contains title1))) AND (body Contains body1))) OR (body Contains body2))) OR (userkey Contains userkey1))", translatedQuery.ClauseText);
        }
    }
}
