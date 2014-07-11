using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Kooboo.Common.Web.WebResourceLoader.DynamicClientResource;
using dotless.Core;
using dotless.Core.Input;
using dotless.Core.Parser;
using dotless.Core.Stylizers;
using dotless.Core.Importers;
using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Tree;
namespace Kooboo.CMS.Web.Misc
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IDynamicClientResource), Key = "less")]
    public class LessCssProvider : IDynamicClientResource
    {
        #region IDynamicClientResource
        public ResourceType ResourceType
        {
            get { return Kooboo.Common.Web.WebResourceLoader.DynamicClientResource.ResourceType.Stylesheet; }
        }
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
            return string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", Kooboo.Common.Web.UrlUtility.ResolveUrl("~/Scripts/less.js"));
        }

        public string Parse(string virtualPath)
        {
            var physicalPath = Kooboo.Common.Web.UrlUtility.MapPath(virtualPath);
            if (File.Exists(physicalPath))
            {
                var lessEngine = GetLessEngine(physicalPath);
                var source = Kooboo.Common.IO.IOUtility.ReadAsString(physicalPath);
                return lessEngine.TransformToCss(source, null);
            }
            return "";
        } 
        #endregion

        #region GetLessEngine
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
        public class RawUrlNodeProvider : DefaultNodeProvider, INodeProvider
        {
            dotless.Core.Parser.Tree.Url INodeProvider.Url(dotless.Core.Parser.Infrastructure.Nodes.Node value, IImporter importer, NodeLocation location)
            {
                //ignore the raw imported file path. It is the default behavior in less.js
                return new Url(value);
            }
        }
        private ILessEngine GetLessEngine(string physicalPath)
        {
            var basePath = Path.GetDirectoryName(physicalPath);
            var stylizer = new PlainStylizer();
            var importer = new Importer(new FileReader(new BasePathResolver(basePath)));
            var parser = new Parser(stylizer, importer) { NodeProvider = new RawUrlNodeProvider() };
            var lessEngine = new LessEngine(parser);
            return lessEngine;            
        }
        #endregion
    }



}