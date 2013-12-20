using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Kooboo.Web.Mvc.WebResourceLoader.DynamicClientResource;
using dotless.Core;
using dotless.Core.Input;
using dotless.Core.Parser;
using dotless.Core.Stylizers;
using dotless.Core.Importers;
namespace Kooboo.CMS.Web.Misc
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IDynamicClientResource), Key = "less")]
    public class LessCssProvider : LessCssHttpHandlerBase, IDynamicClientResource
    {

        public IEnumerable<string> SupportedFileExtensions
        {
            get { return new[] { ".less" }; }
        }

        public bool Accept(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return !string.IsNullOrEmpty(extension) && extension.ToLower() == ".less";
        }

        public string RegisterResource(string filePath)
        {
            return string.Format("<link rel=\"stylesheet/less\" href=\"{0}\" type=\"text/css\" />", filePath);
        }

        public string RegisterClientParser()
        {
            return string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", Kooboo.Web.Url.UrlUtility.ResolveUrl("~/Scripts/less.js"));
        }

        public string Parse(string virtualPath)
        {
            var physicalPath = Kooboo.Web.Url.UrlUtility.MapPath(virtualPath);
            if (File.Exists(physicalPath))
            {
                var lessEngine = GetLessEngine(physicalPath);
                var source = Kooboo.IO.IOUtility.ReadAsString(physicalPath);
                return lessEngine.TransformToCss(source, null);
            }
            return "";
        }
        public class BasePathResolver : IPathResolver
        {
            string _basePath;
            public BasePathResolver(string basePath)
            {
                _basePath = basePath;
            }
            public string GetFullPath(string path)
            {
                return Path.Combine(_basePath, path);
            }
        }

        private ILessEngine GetLessEngine(string physicalPath)
        {
            var basePath = Path.GetDirectoryName(physicalPath);
            var stylizer = new PlainStylizer();
            var importer = new Importer(new FileReader(new BasePathResolver(basePath)));
            var parser = new Parser(stylizer, importer);
            var lessEngine = new LessEngine(parser);
            return lessEngine;
        }

        public ResourceType ResourceType
        {
            get { return Kooboo.Web.Mvc.WebResourceLoader.DynamicClientResource.ResourceType.Stylesheet; }
        }
    }



}