#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Web;
using Kooboo.Globalization;
using Kooboo.Web.Url;
//# define Page_Trace
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Sites.View
{
    #region InvalidPageRouteException
    /// <summary>
    /// View中使用的UrlHelper，类似于MVC的UrlHelper
    /// </summary>
    public class InvalidPageRouteException : Exception
    {
        public InvalidPageRouteException(Page page)
            : base(
            string.Format("Plese set the default route values for the page \"{0}\".".Localize(), page))
        {
            this.Page = page;
        }
        public virtual Page Page { get; private set; }
    }
    #endregion

    #region FrontUrlHelper
    public class FrontUrlHelper
    {
        #region .ctor
        public FrontUrlHelper(UrlHelper url, Site site, FrontRequestChannel requestChannel)
        {
            this.Url = url;
            this.Site = site;
            this.RequestChannel = requestChannel;
        }

        public UrlHelper Url { get; private set; }
        public Site Site { get; private set; }
        public FrontRequestChannel RequestChannel { get; private set; }
        #endregion

        #region WrapperUrl
        public virtual IHtmlString WrapperUrl(string url)
        {
            return FrontUrlHelper.WrapperUrl(url, this.Site, this.RequestChannel);
        }
        public static IHtmlString WrapperUrl(string url, Site site, FrontRequestChannel channel)
        {
            return WrapperUrl(url, site, channel, false);
        }
        public static IHtmlString WrapperUrl(string url, Site site, FrontRequestChannel channel, bool requiredSSL)
        {
            if (string.IsNullOrEmpty(url))
            {
                return new HtmlString(url);
            }
            var applicationPath = HttpContext.Current.Request.ApplicationPath.TrimStart(new char[] { '/' });
            if (!url.StartsWith("/") && !string.IsNullOrEmpty(applicationPath))
            {
                url = "/" + applicationPath + "/" + url;
            }
            var urlSplit = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var sitePath = site.AsActual().SitePath;
            if (channel == FrontRequestChannel.Debug || channel == FrontRequestChannel.Design || channel == FrontRequestChannel.Unknown)
            {
                sitePath = SiteHelper.PREFIX_FRONT_DEBUG_URL + site.FullName;
            }
            IEnumerable<string> urlPaths = urlSplit;
            if (!string.IsNullOrEmpty(sitePath))
            {
                if (urlSplit.Length > 0 && applicationPath.EqualsOrNullEmpty(urlSplit[0], StringComparison.OrdinalIgnoreCase))
                {
                    urlPaths = new string[] { applicationPath, sitePath }.Concat(urlSplit.Skip(1));
                }
                else
                {
                    urlPaths = new string[] { sitePath }.Concat(urlSplit);
                }
            }

            url = "/" + string.Join("/", urlPaths.ToArray());


            #region SSL
            if (channel == FrontRequestChannel.Host || channel == FrontRequestChannel.HostNPath)
            {
                if (HttpContext.Current.Request.IsSecureConnection)
                {
                    if (!requiredSSL)
                    {
                        url = "http://" + HttpContext.Current.Request.Url.Host + url;
                    }
                }
                else if (requiredSSL)
                {
                    url = "https://" + HttpContext.Current.Request.Url.Host + url;
                }
            }

            #endregion

            return new HtmlString(url);
        }

        #endregion

        #region ResourceUrl
        /// <summary>
        /// The URL for combined site scripts.
        /// </summary>
        /// <returns></returns>
        public virtual IHtmlString SiteScriptsUrl()
        {
            return SiteScriptsUrl(Site.ResourceDomain);
        }
        /// <summary>
        /// The URL for combined site scripts.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <returns></returns>
        public virtual IHtmlString SiteScriptsUrl(string baseUri)
        {
            return SiteScriptsUrl(baseUri, true);
        }
        /// <summary>
        /// The URL for combined site scripts.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="compressed">if set to <c>true</c> [compressed].</param>
        /// <returns></returns>
        public virtual IHtmlString SiteScriptsUrl(string baseUri, bool compressed)
        {
            Site site = this.Site;
            return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, this.Url.Action("scripts", "Resource", new { siteName = site.FullName, version = site.VersionUsedInUrl, area = "", compressed })));
        }

        /// <summary>
        /// Sites the theme URL.
        /// </summary>
        /// <returns></returns>
        public virtual IHtmlString SiteThemeUrl()
        {
            return SiteThemeUrl(Site.ResourceDomain, Site.Theme);
        }

        /// <summary>
        /// the site theme URL.
        /// </summary>
        /// <returns></returns>
        public virtual IHtmlString SiteThemeUrl(string baseUri, string themeName)
        {
            Site site = this.Site;
            return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, this.Url.Action("theme", "Resource", new { siteName = site.FullName, name = themeName, version = site.VersionUsedInUrl, area = "" })).ToString());
        }
        /// <summary>
        /// Get the media content url.
        /// </summary>
        /// <param name="fullFoldername"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual IHtmlString MediaContentUrl(string fullFoldername, string fileName)
        {
            var mediaFolder = new Kooboo.CMS.Content.Models.MediaFolder(this.Site.GetRepository(), fullFoldername);

            HtmlString htmlString = new HtmlString("");
            if (string.IsNullOrEmpty(fullFoldername))
            {
                return htmlString;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                var folderPath = new FolderPath(mediaFolder);
                htmlString = new HtmlString(this.Url.Content(folderPath.VirtualPath));
            }
            else
            {
                var mediaContent = mediaFolder.CreateQuery().WhereEquals("FileName", fileName).FirstOrDefault();
                if (mediaContent != null)
                {
                    htmlString = new HtmlString(this.Url.Content(mediaContent.VirtualPath));
                }
            }

            return htmlString;

        }
        /// <summary>
        /// The URL for module scripts.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns></returns>
        public virtual IHtmlString ModuleScriptsUrl(string moduleName)
        {
            return ModuleScriptsUrl(moduleName, this.Site.ResourceDomain);
        }
        /// <summary>
        /// The URL for module scripts.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <returns></returns>
        public virtual IHtmlString ModuleScriptsUrl(string moduleName, string baseUri)
        {
            return ModuleScriptsUrl(moduleName, baseUri, true);
        }
        /// <summary>
        /// The URL for module scripts.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="compressed">if set to <c>true</c> [compressed].</param>
        /// <returns></returns>
        public virtual IHtmlString ModuleScriptsUrl(string moduleName, string baseUri, bool compressed)
        {
            Site site = this.Site;
            return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, this.Url.Action("ModuleScripts", "Resource", new { siteName = site.FullName, moduleName = moduleName, version = site.VersionUsedInUrl, area = "", compressed = compressed })).ToString());
        }
        /// <summary>
        /// Modules the theme URL.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="themeName">Name of the theme.</param>
        /// <returns></returns>
        public virtual IHtmlString ModuleThemeUrl(string moduleName, string themeName)
        {
            return ModuleThemeUrl(moduleName, themeName, this.Site.ResourceDomain);
        }
        public virtual IHtmlString ModuleThemeUrl(string moduleName, string themeName, string baseUri)
        {
            Site site = this.Site;
            return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, this.Url.Action("ModuleTheme", "Resource", new { siteName = site.FullName, moduleName = moduleName, name = themeName, version = site.VersionUsedInUrl, area = "" })).ToString());
        }
        /// <summary>
        /// Resizes the image URL.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public virtual IHtmlString ResizeImageUrl(string imagePath, int width, int height)
        {
            return ResizeImageUrl(imagePath, width, height, null, null);
        }
        /// <summary>
        /// Resizes the image URL.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="preserverAspectRatio">The preserver aspect ratio.</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public virtual IHtmlString ResizeImageUrl(string imagePath, int width, int height, bool? preserverAspectRatio, int? quality)
        {
            return ResourceCDNUrl(this.WrapperUrl(this.Url.Action("ResizeImage", "Resource", new { siteName = Site.FullName, url = imagePath, area = "", width = width, height = height, preserverAspectRatio = preserverAspectRatio, quality = quality })).ToString());
        }

        /// <summary>
        /// the file URL under the theme of current site.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.<example>images/logo.png</example></param>
        /// <returns></returns>
        public virtual IHtmlString ThemeFileUrl(string relativeUrl)
        {
            var site = this.Site;
            IHtmlString url = new HtmlString("");
            if (!string.IsNullOrEmpty(site.Name))
            {
                var fileExists = false;
                var themeFileUrl = "";
                do
                {
                    site = site.AsActual();

                    Theme theme = new Theme(site, site.Theme).LastVersion();
                    themeFileUrl = Kooboo.Web.Url.UrlUtility.Combine(theme.VirtualPath, relativeUrl);
                    var physicalPath = UrlUtility.MapPath(themeFileUrl);
                    fileExists = File.Exists(physicalPath);

                    site = theme.Site.Parent;
                } while (site != null && !fileExists);

                url = ResourceCDNUrl(themeFileUrl);
            }

            return url;

        }

        /// <summary>
        /// Resources the URL. Using the ResourceDomain to generate the resource absolute url.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <returns></returns>
        public virtual IHtmlString ResourceCDNUrl(string relativeUrl)
        {
            string resourceDomain = Site.ResourceDomain;

            return new HtmlString(UrlUtility.ToHttpAbsolute(resourceDomain, relativeUrl));
        }
        /// <summary>
        /// The file URL.
        /// </summary>
        /// <param name="relativeFilePath">The relative file path.</param>
        /// <returns></returns>
        public virtual IHtmlString FileUrl(string relativeFilePath)
        {
            Site site = this.Site;
            var dir = Path.GetDirectoryName(relativeFilePath);
            CustomFile file;
            if (string.IsNullOrEmpty(dir))
            {
                file = new CustomFile(site, relativeFilePath);
            }
            else
            {
                CustomDirectory customDir = new CustomDirectory(site, dir).LastVersion();
                file = new CustomFile(customDir, Path.GetFileName(relativeFilePath));
            }
            file = file.LastVersion();
            return ResourceCDNUrl(file.VirtualPath);
        }

        /// <summary>
        /// Get the file url under "Scripts" folder.
        /// </summary>
        /// <param name="relativeScriptFilePath">The relative file path.</param>
        /// <returns></returns>
        public virtual IHtmlString ScriptFileUrl(string relativeScriptFilePath)
        {
            Site site = this.Site;
            var dir = Path.GetDirectoryName(relativeScriptFilePath);
            var fileVirtualPath = "";

            if (string.IsNullOrEmpty(dir))
            {
                fileVirtualPath = new ScriptFile(site, relativeScriptFilePath).VirtualPath;
            }
            else
            {
                do
                {
                    var scriptsPath = new ScriptFile(site, "");
                    fileVirtualPath = UrlUtility.Combine(scriptsPath.VirtualPath, relativeScriptFilePath);
                    var physicalPath = UrlUtility.MapPath(fileVirtualPath);
                    if (File.Exists(physicalPath))
                    {
                        break;
                    }
                    else
                    {
                        site = site.Parent;
                    }
                } while (site != null);
            }

            return ResourceCDNUrl(fileVirtualPath);
        }
        #endregion

        #region PageUrl
        public virtual IHtmlString PageUrl(string urlMapKey)
        {
            //System.Diagnostics.Contracts.Contract.Requires(!string.IsNullOrEmpty(urlMapKey));

            return this.PageUrl(urlMapKey, null);
        }

        public virtual IHtmlString PageUrl(string urlMapKey, object values)
        {
            Page page;
            return PageUrl(urlMapKey, values, out page);
        }
        public virtual IHtmlString PageUrl(string urlMapKey, object values, out Page page)
        {
#if Page_Trace
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            var url = GeneratePageUrl(urlMapKey, values, (site, key) => null, out page);

#if Page_Trace
            stopwatch.Stop();
            HttpContext.Current.Response.Write(string.Format("PageUrl,{0}.</br>", stopwatch.Elapsed));
#endif
            return url;
        }

        #endregion

        #region GeneratePageUrl
        internal IHtmlString GeneratePageUrl(string urlMapKey, object values, Func<Site, string, Page> findPage, out Page page)
        {

            //System.Diagnostics.Contracts.Contract.Requires(!string.IsNullOrEmpty(urlMapKey));
            var site = this.Site;

            page = null;
            var urlKeyMap = Persistence.Providers.UrlKeyMapProvider.Get(new UrlKeyMap(site, urlMapKey));
            if (urlKeyMap != null)
            {
                if (!string.IsNullOrEmpty(urlKeyMap.PageFullName))
                {
                    page = new Page(site, PageHelper.SplitFullName(urlKeyMap.PageFullName).ToArray());
                }
                else
                    page = null;
            }
            if (page == null)
            {
                page = new Page(site, PageHelper.SplitFullName(urlMapKey).ToArray()).LastVersion();
                if (page == null || !page.Exists())
                {
                    page = findPage(site, urlMapKey);
                    string pageFullName = "";
                    if (page != null && page.Exists())
                    {
                        pageFullName = page.FullName;
                    }
                    if (urlKeyMap == null)
                        Services.ServiceFactory.UrlKeyMapManager.Add(site, new UrlKeyMap(site, urlMapKey) { PageFullName = pageFullName });
                }
            }

            if (page != null && page.Exists())
            {

                var url = GeneratePageUrl(page, values);

                return url;
            }
            else
            {
                return new HtmlString("");
            }
        }

        internal IHtmlString GeneratePageUrl(Page page, object values)
        {
            return GeneratePageUrl(this.Url, this.Site, page, values, this.RequestChannel);
        }
        public static IHtmlString GeneratePageUrl(UrlHelper urlHelper, Site site, Page page, object values, FrontRequestChannel channel)
        {
            RouteValueDictionary routeValues = RouteValuesHelpers.GetRouteValues(values);

            page = page.AsActual();

            if (page == null)
            {
                return new HtmlString("");
            }
            if (page.Route != null && !string.IsNullOrEmpty(page.Route.ExternalUrl))
            {
                return new HtmlString(page.Route.ExternalUrl);
            }

            var pageRoute = page.Route.ToMvcRoute();

            routeValues = RouteValuesHelpers.MergeRouteValues(pageRoute.Defaults, routeValues);

            var routeVirtualPath = pageRoute.GetVirtualPath(urlHelper.RequestContext, routeValues);
            if (routeVirtualPath == null)
            {
                throw new InvalidPageRouteException(page);
            }
            //string contentUrl = routeVirtualPath.VirtualPath;//don't decode the url. why??
            //if do not decode the url, the route values contains Chinese character will cause bad request.
            string contentUrl = HttpUtility.UrlDecode(routeVirtualPath.VirtualPath);
            string pageUrl = contentUrl;
            if (!string.IsNullOrEmpty(contentUrl) || (string.IsNullOrEmpty(pageUrl) && !page.IsDefault))
            {
                pageUrl = Kooboo.Web.Url.UrlUtility.Combine(page.VirtualPath, contentUrl);
            }
            if (string.IsNullOrEmpty(pageUrl))
            {
                pageUrl = urlHelper.Content("~/");
            }
            else
            {
                pageUrl = HttpUtility.UrlDecode(
                urlHelper.RouteUrl("Page", new { PageUrl = new HtmlString(pageUrl) }));
            }
            var url = FrontUrlHelper.WrapperUrl(pageUrl, site, channel, page.RequireHttps);

            return url;
        }
        #endregion

        #region Preview
        public static IHtmlString Preview(UrlHelper urlHelper, Site site, Page page)
        {
            return Preview(urlHelper, site, page, null);
        }
        public static IHtmlString Preview(UrlHelper urlHelper, Site site, Page page, object values)
        {
            if (page != null)
            {
                page = page.AsActual();
            }
            var pageUrl = urlHelper.Content("~/");
            if (page != null && !page.IsDefault)
            {
                pageUrl = urlHelper.Content("~/" + page.VirtualPath);
            }
            var previewUrl = FrontUrlHelper.WrapperUrl(pageUrl, site, FrontRequestChannel.Unknown).ToString();
            if (values != null)
            {
                RouteValueDictionary routeValues = new RouteValueDictionary(values);

                foreach (var item in routeValues)
                {
                    if (item.Value != null)
                    {
                        previewUrl = Kooboo.Web.Url.UrlUtility.AddQueryParam(previewUrl, item.Key, item.Value.ToString());
                    }
                }
            }
            return new HtmlString(previewUrl);
        }

        #endregion

        #region ViewUrl
        public virtual IHtmlString ViewUrl(string viewName)
        {
            System.Diagnostics.Contracts.Contract.Requires(!string.IsNullOrEmpty(viewName));

            return ViewUrl(viewName, null);
        }

        public virtual IHtmlString ViewUrl(string viewName, object values)
        {
            RouteValueDictionary routeValues = RouteValuesHelpers.GetRouteValues(values);
            routeValues["ViewName"] = viewName;
            return this.WrapperUrl(this.Url.Action("ViewEntry", "Page", routeValues));
        }

        #endregion

        #region SubmissionUrl
        public virtual IHtmlString SubmissionUrl(string submissionName)
        {
            return SubmissionUrl(submissionName, null);
        }
        public virtual IHtmlString SubmissionUrl(string submissionName, object routeValues)
        {
            System.Diagnostics.Contracts.Contract.Requires(!string.IsNullOrEmpty(submissionName));

            var routes = RouteValuesHelpers.GetRouteValues(routeValues);
            routes["SubmissionName"] = submissionName;
            return this.WrapperUrl(this.Url.Action("Submit", "Submission", routes));
        }
        #endregion
    }
    #endregion
}
