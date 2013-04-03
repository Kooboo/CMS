using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.View.CodeSnippet;
using Kooboo.CMS.Content.Models;
using System.Text.RegularExpressions;
using System.Web;
using Kooboo.CMS.Sites.TemplateEngines.NVelocity.MvcViewEngine;

namespace Kooboo.CMS.Sites.TemplateEngines.NVelocity
{
    public class NVelocityTemplateEngine : ITemplateEngine
    {
        public IEnumerable<string> FileExtensions
        {
            get { return new string[] { ".vm" }; }
        }

        public System.Web.Mvc.IViewEngine GetViewEngine()
        {
            return NVelocityViewEngine.Default;
        }

        public System.Web.Mvc.IView CreateView(System.Web.Mvc.ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new NVelocityView(controllerContext, viewPath, masterPath);
        }

        public string Name
        {
            get { return "NVelocity"; }
        }

        public string GetFileExtensionForLayout()
        {
            return ".vm";
        }

        public string GetFileExtensionForView()
        {
            return ".vm";
        }


        public Kooboo.CMS.Sites.View.CodeSnippet.IDataRuleCodeSnippet GetDataRuleCodeSnippet(TakeOperation takeOperation)
        {
            switch (takeOperation)
            {
                case TakeOperation.List:
                    return new NVelocityListCodeSnippet();
                case TakeOperation.First:
                    return new NVelocityDetailCodeSnippet();
                case TakeOperation.Count:
                default:
                    return null;
            }
        }


        public View.CodeSnippet.ILayoutPositionParser GetLayoutPositionParser()
        {
            return new NVelocityLayoutPositionParser();
        }


        public View.CodeSnippet.ICodeHelper GetCodeHelper()
        {
            return new NVelocityCodeHelper();
        }


        public int Order
        {
            get { return 3; }
        }
    }
}
