using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.View.CodeSnippet;

namespace Kooboo.CMS.Sites.TemplateEngines.NVelocity
{
    public class NVelocityCodeHelper : ICodeHelper
    {
        public string RegisterTitle()
        {
            return "$Html.FrontHtml().HtmlTitle()";
        }

        public string RegisterHtmlMeta()
        {
            return "$Html.FrontHtml().Meta()";
        }

        public string RegisterStyles()
        {
            return "$Html.FrontHtml().RegisterStyles()";
        }

        public string RegisterScripts()
        {
            return "$Html.FrontHtml().RegisterScripts()";
        }

        public string RenderView(string viewName)
        {
            return string.Format("$Html.FrontHtml().RenderView(\"{0}\",$ViewData)", viewName);
        }

        public string DefaultViewCode()
        {
            return "";
        }

        public string RegisterParameterCode()
        {
            return @"function getParameterCode(name){
return '$Page_Context.Current[""'+name+'""]'
}";
        }

    }
}