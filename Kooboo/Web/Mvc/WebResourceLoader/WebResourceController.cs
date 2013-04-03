//http://mvcresourceloader.codeplex.com/
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Web.Mvc.WebResourceLoader.Configuration;
namespace Kooboo.Web.Mvc.WebResourceLoader
{
    public class WebResourceController : Controller
    {
        public WebResourceController()
        {
            //TempDataProvider = new NullTempDataProvider();
        }

        [NonAction]
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "WebResource",
                "WebResource/{name}/{version}/{condition}",
                new { controller = "ExternalResource", action = "Index", condition = "" }
            );
        }

        [NonAction]
        public static void RegisterRoutes(RouteCollection routes, string rootFolder)
        {
            routes.MapRoute(
                "WebResource",
                rootFolder,
                new { controller = "WebResource", action = "Index" }
            );
        }

        public void Index(string name, string version, string condition)
        {
            HttpResponseBase response = Response;

            WebResourcesSection section = ConfigurationManager.GetSection(Kooboo.Web.Mvc.AreaHelpers.GetAreaName(this.RouteData));
            if (section == null)
            {
                throw new HttpException(500, "Unable to find the web resource configuration.");
            }

            ReferenceElement settings = section.References[name];

            if (settings == null)
            {
                throw new HttpException(500, string.Format("Unable to find any matching web resource settings for {0}.", name));
            }

            Condition conditionInfo = new Condition
            {
                If = condition ?? string.Empty
            };

            // filter the files based on the condition (Action / If) passed in
            IList<FileInfoElement> files = new List<FileInfoElement>();
            foreach (FileInfoElement fileInfo in settings.Files)
            {
                if (fileInfo.If.Equals(conditionInfo.If))
                {
                    files.Add(fileInfo);
                }
            }

            // Ooutput Type
            response.ContentType = settings.MimeType;
            Stream output = response.OutputStream;

            // Compress
            if (section.Compress)
            {
                string acceptEncoding = Request.Headers["Accept-Encoding"];

                if (!string.IsNullOrEmpty(acceptEncoding))
                {
                    acceptEncoding = acceptEncoding.ToLowerInvariant();

                    if (acceptEncoding.Contains("gzip"))
                    {
                        response.AddHeader("Content-encoding", "gzip");
                        output = new GZipStream(output, CompressionMode.Compress);
                    }
                    else if (acceptEncoding.Contains("deflate"))
                    {
                        response.AddHeader("Content-encoding", "deflate");
                        output = new DeflateStream(output, CompressionMode.Compress);
                    }
                }
            }

            // Combine
            using (StreamWriter sw = new StreamWriter(output))
            {
                foreach (FileInfoElement fileInfo in files)
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath(fileInfo.Filename));

                    switch (settings.MimeType)
                    {
                        case "text/css":
                            content = CSSMinify.Minify(Url, fileInfo.Filename, Request.Url.AbsolutePath, content);
                            break;
                        case "text/x-javascript":
                        case "text/javascript":
                        case "text/ecmascript":
                            if (section.Compact)
                            {
                                content = JSMinify.Minify(content);
                            }
                            break;
                    }
                    sw.WriteLine(content.Trim());
                }
            }

            // Cache

            if (section.CacheDuration > 0)
            {
                DateTime timestamp = HttpContext.Timestamp;
                HttpCachePolicyBase cache = response.Cache;
                int duration = section.CacheDuration;

                cache.SetCacheability(HttpCacheability.Public);
                cache.SetExpires(timestamp.AddSeconds(duration));
                cache.SetMaxAge(new TimeSpan(0, 0, duration));
                cache.SetValidUntilExpires(true);
                cache.SetLastModified(timestamp);
                cache.VaryByHeaders["Accept-Encoding"] = true;
                cache.VaryByParams["name"] = true;
                cache.VaryByParams["version"] = true;
                cache.VaryByParams["display"] = true;
                cache.VaryByParams["condition"] = true;
                cache.SetOmitVaryStar(true);
            }
        }
    }
}
