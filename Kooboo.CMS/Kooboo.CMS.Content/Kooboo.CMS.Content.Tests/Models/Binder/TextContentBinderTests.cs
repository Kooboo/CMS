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
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Binder;
using System.Collections.Specialized;

using Kooboo.CMS.Form;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Content.Tests.Models.Binder
{
    /// <summary>
    /// Summary description for TextContentBinderTests
    /// </summary>
    [TestClass]
    public class TextContentBinderTests
    {
        Schema schema = null;
        TextContentBinder binder = new TextContentBinder();

        public TextContentBinderTests()
        {
            schema = new Schema() { IsDummy = false };
            schema.AddColumn(new Column()
            {
                Name = "Title",
                DataType = DataType.String,
                ControlType = "TextBox",
                Validations = new ColumnValidation[] { new RequiredValidation() { ErrorMessage = "The field is requried" } }
            });
            schema.AddColumn(new Column()
                {
                    Name = "Body",
                    DataType = DataType.String,
                    ControlType = "TextBox"
                });
            schema.AddColumn(new Column()
            {
                Name = "Comments",
                DataType = DataType.Int,
                ControlType = "TextBox"
            });
            schema.AddColumn(new Column()
            {
                Name = "PostDate",
                DataType = DataType.DateTime,
                ControlType = "TextBox"
            });
        }
        [TestMethod]
        public void Test1()
        {
            NameValueCollection values = new NameValueCollection();
            values["title"] = "title1";
            values["body"] = "body1";
            values["comments"] = "0";
            var postdate = DateTime.Now;
            values["Postdate"] = postdate.ToString();

            TextContent textContent = new TextContent();
            textContent = binder.Bind(schema, textContent, values);

            Assert.AreEqual(values["title"], textContent["title"].ToString());
            Assert.AreEqual(values["body"], textContent["body"].ToString());
            Assert.AreEqual(0, (int)textContent["comments"]);
            Assert.AreEqual(postdate.ToUniversalTime().ToString(), ((DateTime)textContent["postdate"]).ToString());
        }
        [TestMethod]
        [ExpectedException(typeof(RuleViolationException))]
        public void TestNullViolation()
        {
            NameValueCollection values = new NameValueCollection();
            values["body"] = "body1";
            values["comments"] = "0";
            var postdate = DateTime.Now;
            values["Postdate"] = postdate.ToString();

            TextContent textContent = new TextContent();
            textContent = binder.Bind(schema, textContent, values);
        }
        [TestMethod]
        [ExpectedException(typeof(RuleViolationException))]
        public void TestRequiredViolation()
        {
            NameValueCollection values = new NameValueCollection();
            values["title"] = "";
            values["body"] = "body1";
            values["comments"] = "0";
            var postdate = DateTime.Now;
            values["Postdate"] = postdate.ToString();

            TextContent textContent = new TextContent();
            textContent = binder.Bind(schema, textContent, values);
        }
    }
}
