#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Controllers.ActionFilters;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Parsers.ThemeRule;
using Kooboo.CMS.Sites.Services;
using Kooboo.Drawing;
using Kooboo.IO;
using Kooboo.Web.Mvc.WebResourceLoader;
using Kooboo.Web.Mvc.WebResourceLoader.DynamicClientResource;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Models;
namespace Kooboo.CMS.Sites.Controllers
{
    /// <summary>
    /// 
    /// </summary>

    public class ResourceController : FrontControllerBase
    {
        #region Scripts
        /// <summary>
        /// Scriptses the specified site name.
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <param name="name">The name. the folder name</param>
        /// <param name="compressed">The compressed.</param>
        /// <returns></returns>
        public virtual ActionResult Scripts(string siteName, string name, bool? compressed)
        {
            var site = new Site(siteName);
            var scripts = ServiceFactory.ScriptManager.GetFiles(site, name);

            Output(CompressJavascript(scripts, compressed), "text/javascript", 2592000, "*");

            return null;
        }
        private string CompressJavascript(IEnumerable<IPath> jsFiles, bool? compressed)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var file in jsFiles)
            {
                string content;
                var dynamicResource = DynamicClientResourceFactory.Default.ResolveProvider(file.VirtualPath);

                if (dynamicResource != null)
                {
                    content = dynamicResource.Parse(file.VirtualPath);
                }
                else
                {
                    content = IOUtility.ReadAsString(file.PhysicalPath);
                }
                sb.Append(content + ";\n");
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

            Output(CompressJavascript(scripts, compressed), "text/javascript", 2592000, "*");

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
                string content;
                var dynamicResource = DynamicClientResourceFactory.Default.ResolveProvider(file.PhysicalPath);

                if (dynamicResource != null)
                {
                    content = dynamicResource.Parse(file.VirtualPath);
                }
                else
                {
                    content = IOUtility.ReadAsString(file.PhysicalPath);
                }
                sb.AppendFormat("{0}\n", CSSMinify.Minify(Url, file.VirtualPath, Request.Url.AbsolutePath, content));
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
        [CheckSiteExistsActionFilter(Order = 0)]
        public virtual ActionResult RobotsTxt()
        {
            Robots_Txt robotTxt = new Robots_Txt(Site);
            var body = robotTxt.Read();
            return Content(body, "text/plain");
        }
        #endregion

        #region File
        [CheckSiteExistsActionFilter(Order = 0)]
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

            preserverAspectRatio = preserverAspectRatio ?? true;
            quality = quality ?? 80;

            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                //now no image cache for azure blob
                var provider = Kooboo.CMS.Content.Persistence.Providers.DefaultProviderFactory.GetProvider<Kooboo.CMS.Content.Persistence.IMediaContentProvider>();
                var mediaContent = new MediaContent() { VirtualPath = url };
                Stream stream = provider.GetContentStream(mediaContent);
                var imageFormat = ImageTools.ConvertToImageFormat(Path.GetExtension(mediaContent.VirtualPath));
                Stream outStream = new MemoryStream();
                ImageTools.ResizeImage(stream, outStream, imageFormat, width, height, preserverAspectRatio.Value, quality.Value);
                outStream.Position = 0;
                return File(outStream, IOUtility.MimeType(url));
            }
            else
            {
                var imageFullPath = Server.MapPath(url);
                var cachingPath = GetCachingFilePath(imageFullPath, width, height, preserverAspectRatio.Value, quality.Value);

                if (!System.IO.File.Exists(cachingPath))
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
                SetCache(HttpContext.Response, 2592000, "*");
                return File(cachingPath, IOUtility.MimeType(imageFullPath));
            }
        }
        private string GetCachingFilePath(string imagePath, int width, int height, bool preserverAspectRatio, int quality)
        {
            var lastModeifyDate = System.IO.File.GetLastWriteTimeUtc(imagePath);
            var baseDir = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IBaseDir>();
            string cms_dataPath = baseDir.Cms_DataPhysicalPath;
            string fileName = Path.GetFileNameWithoutExtension(imagePath);
            string newFileName = fileName + "-" + width.ToString() + "-" + height.ToString() + "-" + preserverAspectRatio.ToString() + "-" + quality.ToString() + "-" + lastModeifyDate.Ticks;
            string imageCachingPath = Path.Combine(cms_dataPath, "ImageCaching");
            string cachingPath = imageCachingPath + imagePath.Substring(cms_dataPath.Length);
            return Path.Combine(Path.GetDirectoryName(cachingPath), newFileName + Path.GetExtension(imagePath));
        }
        #endregion

        #region Cache setting
        protected virtual void CacheThisRequest()
        {
            SetCache(HttpContext.Response, 2592000, "*");
        }
        protected virtual void SetCache(HttpResponseBase response, int cacheDuration, params string[] varyByParams)
        {
            // Cache
            if (cacheDuration > 0)
            {
                DateTime timestamp = DateTime.Now;

                HttpCachePolicyBase cache = response.Cache;
                int duration = cacheDuration;

                cache.SetCacheability(HttpCacheability.Public);
                cache.SetAllowResponseInBrowserHistory(true);
                cache.SetExpires(timestamp.AddSeconds(duration));
                cache.SetMaxAge(new TimeSpan(0, 0, duration));
                cache.SetValidUntilExpires(true);
                cache.SetLastModified(timestamp);
                cache.VaryByHeaders["Accept-Encoding"] = true;
                if (varyByParams != null)
                {
                    foreach (var p in varyByParams)
                    {
                        cache.VaryByParams[p] = true;
                    }
                }

                cache.SetOmitVaryStar(true);
                response.AddHeader("Cache-Control", string.Format("public, max-age={0}", cacheDuration));

            }
        }
        #endregion
    }
}
