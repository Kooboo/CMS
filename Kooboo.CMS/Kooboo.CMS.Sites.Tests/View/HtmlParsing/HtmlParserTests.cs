#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Sites.View.HtmlParsing;
using Moq;

namespace Kooboo.CMS.Sites.Tests.View.HtmlParsing
{
    [TestClass]
    public class HtmlParserTests
    {
        HtmlParser _htmlParser;
        public HtmlParserTests()
        {
            var urlParser = new Mock<IParser>();
            urlParser.Setup(it => it.Parse(It.IsAny<string>()))
                .Returns<string>((s) => s);
            var parsers = new Mock<IParsers>();
            parsers.Setup(it => it.GetParser(It.IsAny<string>())).Returns(urlParser.Object);
            _htmlParser = new HtmlParser(parsers.Object);
        }
        [TestMethod]
        public void Test_Parse_Url()
        {
            string html = "[[url:Articles]]";
            string expected = "Articles";
            Assert.AreEqual(expected, _htmlParser.Parse(html));
        }
    }
}
