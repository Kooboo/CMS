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
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Translator;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query.Translator.String;
namespace Kooboo.CMS.Content.Tests.Query.Translator.String
{
    /// <summary>
    /// Summary description for TextTranslator
    /// </summary>
    [TestClass]
    public class TextTranslatorTests
    {
        [TestMethod]
        public void Test1()
        {
            Repository repository = new Repository("TextTranslatorTests");

            MediaFolder binaryFolder = new MediaFolder(repository, "image");

            var binaryQuery = binaryFolder.CreateQuery().WhereEquals("Title", "title1");

            Assert.AreEqual("[MediaContent] SELECT * FROM [TextTranslatorTests.image] WHERE Title = title1 ORDER  | OP:Unspecified PageSize:0 TOP:0 Skip:0 ", TextTranslator.Translate(binaryQuery));

            Schema schema = new Schema(repository, "news") { IsDummy = false };

            TextFolder textFolder = new TextFolder(repository, "news") { SchemaName = "news", IsDummy = false };

            var textQuery = textFolder.CreateQuery().WhereEquals("Title", "title1").WhereCategory(textFolder.CreateQuery());

            Assert.AreEqual("[TextContent] SELECT * FROM [TextTranslatorTests.news$news] WHERE Title = title1 Category:([TextContent] SELECT * FROM [TextTranslatorTests.news$news] WHERE  Category:() ORDER  | OP:Unspecified PageSize:0 TOP:0 Skip:0 ) ORDER  | OP:Unspecified PageSize:0 TOP:0 Skip:0 ", TextTranslator.Translate(textQuery));

        }
    }
}
