#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.View.CodeSnippet;

namespace Kooboo.CMS.Sites.TemplateEngines.WebForm
{
    public class WebFormCodeHelper : ICodeHelper
    {
        #region Group1
        public string RegisterTitle()
        {
            return "<%: Html.FrontHtml().HtmlTitle() %>";
        }

        public string RegisterHtmlMeta()
        {
            return "<%: Html.FrontHtml().Meta() %>";
        }

        public string RegisterStyles()
        {
            return "<%: Html.FrontHtml().RegisterStyles() %>";
        }

        public string RegisterScripts()
        {
            return "<%:Html.FrontHtml().RegisterScripts() %>";
        }

        public string RenderView(string viewName)
        {
            return string.Format("<%:Html.FrontHtml().RenderView(\"{0}\",ViewData) %>", viewName);
        }

        public string DefaultViewCode()
        {
            return @"<%@ Control Language=""C#"" Inherits=""System.Web.Mvc.ViewUserControl<dynamic>"" %>";
        }
        #endregion

        public string RegisterParameterCode()
        {
            return @"function getParameterCode(name){
return '<%: Page_Context.Current[""'+name+'""] %>'
}";
        }

    }
}
