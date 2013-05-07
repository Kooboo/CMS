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
using System.Web.Routing;
using System.Collections.Generic;
namespace Kooboo.CMS.Sites.Tests.View.HtmlParsing
{
    [TestClass]
    public class UrlParserTests
    {
        UrlParser _urlParser;
        public UrlParserTests()
        {
            var urlGenerator = new Mock<IPageUrlGenerator>();
            urlGenerator.Setup(it => it.PageUrl("articles", It.IsAny<RouteValueDictionary>()))
                 .Returns<string, RouteValueDictionary>((pageName, routes) =>
                 {
                     var url = "/" + pageName;
                     List<string> list = new List<string>();
                     if (routes!=null)
                     {
                         foreach (var item in routes)
                         {
                             if (item.Value != null)
                             {
                                 list.Add(item.Key + "=" + item.Value.ToString());
                             }
                         }
                         if (list.Count > 0)
                         {
                             url = url + "?" + string.Join("&", list);
                         }
                     }
                    
                     return url;
                 });
            _urlParser = new UrlParser(urlGenerator.Object);
        }

        [TestMethod]
        public void Test_Parse_Articles()
        {
            var s = "articles";
            var expected = "/articles";
            Assert.AreEqual(expected, _urlParser.Parse(s));

        }
        [TestMethod]
        public void Test_Parse_EmptyString()
        {
            var s = "";
            string expected = null;
            Assert.AreEqual(expected, _urlParser.Parse(s));

        }
        [TestMethod]
        public void Test_Parse_Articles_UserKey()
        {
            var s = "articles|userkey=key1";
            var expected = "/articles?userkey=key1";
            Assert.AreEqual(expected, _urlParser.Parse(s));

        }
    }
}
