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
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Content.Query.Translator;
using Kooboo.CMS.Content.Query.Translator.String;
namespace Kooboo.CMS.Content.Tests.Query.Translator.String
{
    /// <summary>
    /// Summary description for StringVisitorTests
    /// </summary>
    [TestClass]
    public class StringVisitorTests
    {
        private class MockQuery : TranslatedQuery
        { }
        [TestMethod]
        public void Test1()
        {
            MockQuery query = new MockQuery();

            StringVisitor visitor = new StringVisitor(query);

            Expression ex = new WhereEqualsExpression(null, "Title", "title1");

            visitor.Visite(ex);

            Assert.AreEqual("Title = title1", query.ClauseText);

        }
    }
}
