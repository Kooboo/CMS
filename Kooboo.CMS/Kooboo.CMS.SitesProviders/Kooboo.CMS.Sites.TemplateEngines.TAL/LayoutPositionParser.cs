#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.View.CodeSnippet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.Sites.TemplateEngines.TAL
{
    public class LayoutPositionParser : ILayoutPositionParser
    {
        public IEnumerable<string> Parse(string layoutBody)
        {
            return new string[0];
        }

        public System.Web.IHtmlString RegisterClientParser()
        {
            return new HtmlString("");
        }

        public System.Web.IHtmlString RegisterClientAddPosition()
        {
            return new HtmlString("");
        }

        public System.Web.IHtmlString RegisterClientRemovePosition()
        {
            return new HtmlString("");
        }
    }
}
