using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.IO;
using System.Web;
using System.IO;
using System.IO.Compression;
using Kooboo.CMS.Sites.Parsers.ThemeRule;
using Kooboo.Web.Mvc.WebResourceLoader;
using Kooboo.Drawing;

namespace Kooboo.CMS.Sites.Controllers.Front
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceController : FrontControllerBase
    {
        #region Scripts
        public virtual ActionResult Scripts(string siteName, bool? compressed)
        {
            var site = new Site(siteName);
            var scripts = Persistence.Providers.ScriptsProvider.All(site);

            Output(CompressJavascript(scripts.Select(it => it.PhysicalPath), compressed), "text/javascript", 2592000, "*");

            return null;
        }
        private string CompressJavascript(IEnumerable<string> jsFiles, bool? compressed)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var file in jsFiles)
            {
                sb.Append(IOUtility.ReadAsString(file) + ";\n");
            }

            if (!compressed.HasValue || compressed.Value == true)
            {
                return Kooboo.Web.Script.JSMin.Minify(sb.ToString());
            }
            else
            {
                return sb.ToString();
            }

        }
        #region ModuleScripts
        public virtual ActionResult ModuleScripts(string moduleName, bool? compressed)
        {
            var scripts = Services.ServiceFactory.ModuleManager.AllScripts(moduleName);

            Output(CompressJavascript(scripts.Select(it => it.PhysicalPath), compressed), "text/javascript", 2592000, "*");

            return null;
        }
        #endregion
        #endregion

        #region Themes
        public virtual ActionResult Theme(string siteName, string name)
        {
            var site = new Site(siteName).AsActual();
            string cssHackBody;
            var styles = ThemeRuleParser.Parse(new Theme(site, name).LastVersion(), out cssHackBody);
            Output(CompressCss(styles), "text/css", 2592000, "*");
            return null;
        }
        private string CompressCss(IEnumerable<IPath> cssFiles)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var file in cssFiles)
            {
                using (FileStream fileStream = new FileStream(file.PhysicalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var content = fileStream.ReadString();
                    sb.AppendFormat("{0}\n", CSSMinify.Minify(Url, file.VirtualPath, Request.Url.AbsolutePath, content));
                }
            }
            return sb.ToString();
        }
        public virtual ActionResult ModuleTheme(string moduleName, string name)
        {
            string themeRuleBody;
            var styles = Services.ServiceFactory.ModuleManager.AllThemeFiles(moduleName, name, out themeRuleBody);
            Output(CompressCss(styles), "text/css", 2592000, "*");
            return null;
        }
        #endregion

        #region robots.txt
        public virtual ActionResult RobotsTxt()
        {
            Robots_Txt robotTxt = new Robots_Txt(Site);
            var body = robotTxt.Read();
            return Content(body, "text/plain");
        }
        #endregion

        #region File
        public virtual ActionResult File(string name)
        {
            var dir = Path.GetDirectoryName(name);
            CustomFile file;
            if (string.IsNullOrEmpty(dir))
            {
                file = new CustomFile(Site, name);
            }
            else
            {
                CustomDirectory customDir = new CustomDirectory(Site, dir).LastVersion();
                file = new CustomFile(customDir, Path.GetFileName(name));
            }
            file = file.LastVersion();
            if (file.Exists())
            {
                SetCache(Response, 2592000, "*");
                return File(file.PhysicalPath, IOUtility.MimeType(file.PhysicalPath));
            }
            return null;
        }
        #endregion

        #region Output
        private void Output(string content, string contentType, int cacheDuration, params string[] varyByParams)
        {
            HttpResponseBase response = Response;
            response.ContentType = contentType;
            Stream output = response.OutputStream;

            // Compress
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

            // Write output
            using (StreamWriter sw = new StreamWriter(output))
            {
                sw.WriteLine(content);
            }

            SetCache(response, cacheDuration, varyByParams);

        }


        #endregion

        #region ResizeImage
        private static object resizeImageLocker = new object();
        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="preserverAspectRatio">The preserver aspect ratio.保持比例</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public virtual ActionResult ResizeImage(string url, int width, int height, bool? preserverAspectRatio, int? quality)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(url);
            }
            url = HttpUtility.UrlDecode(url);
            var index = url.IndexOf("?");
            if (index != -1)
            {
                url = url.Substring(0, index);
            }
            var imageFullPath = Server.MapPath(url);
            preserverAspectRatio = preserverAspectRatio ?? true;
            quality = quality ?? 80;
            var cachingPath = GetCachingFilePath(imageFullPath, width, height, preserverAspectRatio.Value, quality.Value);

            if (!System.IO.File.Exists(cachingPath))
            {
                lock (resizeImageLocker)
                {
                    if (!System.IO.File.Exists(cachingPath))
                    {
                        var dir = Path.GetDirectoryName(cachingPath);
                        IOUtility.EnsureDirectoryExists(dir);
                        var success = ImageTools.ResizeImage(imageFullPath, cachingPath, width, height, preserverAspectRatio.Value, quality.Value);
                        if (!success)
                        {
                            cachingPath = imageFullPath;
                        }
                    }
                }

            }
            SetCache(HttpContext.Response, 2592000, "*");
            return File(cachingPath, IOUtility.MimeType(imageFullPath));
        }
        private string GetCachingFilePath(string imagePath, int width, int height, bool preserverAspectRatio, int quality)
        {
            var lastModeifyDate = System.IO.File.GetLastWriteTimeUtc(imagePath);
            string cms_dataPath = Path.Combine(Kooboo.Settings.BaseDirectory, PathEx.BasePath);
            string fileName = Path.GetFileNameWithoutExtension(imagePath);
            string newFileName = fileName + "-" + width.ToString() + "-" + height.ToString() + "-" + preserverAspectRatio.ToString() + "-" + quality.ToString() + "-" + lastModeifyDate.Ticks;
            string imageCachingPath = Path.Combine(Kooboo.Settings.BaseDirectory, PathEx.BasePath, "ImageCaching");
            string cachingPath = imageCachingPath + imagePath.Substring(cms_dataPath.Length);
            return Path.Combine(Path.GetDirectoryName(cachingPath), newFileName + Path.GetExtension(imagePath));
        }
        #endregion
    }
}
