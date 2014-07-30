#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Web;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.CMS.Content.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Kooboo.CMS.Content.Models.Paths;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class SiteResourceUrlExtensions
    {
        #region ResourceUrl
        /// <summary>
        /// The URL for combined site scripts.
        /// </summary>
        /// <returns></returns>
        public static IHtmlString SiteScriptsUrl(this IFrontUrlHelper frontUrl)
        {
            return SiteScriptsUrl(frontUrl, frontUrl.Site.DomainSetting.ResourceDomain);
        }
        /// <summary>
        /// The URL for combined site scripts.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <returns></returns>
        public static IHtmlString SiteScriptsUrl(this IFrontUrlHelper frontUrl, string baseUri)
        {
            return SiteScriptsUrl(frontUrl, baseUri, "", true);
        }
        /// <summary>
        /// The URL for combined site scripts.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="compressed">if set to <c>true</c> [compressed].</param>
        /// <returns></returns>
        public static IHtmlString SiteScriptsUrl(this IFrontUrlHelper frontUrl, string baseUri, string folder, bool compressed)
        {
            var site = frontUrl.Site;
            return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, frontUrl.Url.Action("scripts", "Resource", new { siteName = site.FullName, version = site.GetVersionUsedInUrl(), area = "", compressed, name = folder })));
        }

        /// <summary>
        /// Sites the theme URL.
        /// </summary>
        /// <returns></returns>
        public static IHtmlString SiteThemeUrl(this IFrontUrlHelper frontUrl)
        {
            var site = frontUrl.Site;
            return SiteThemeUrl(frontUrl, site.DomainSetting.ResourceDomain, site.Theme);
        }

        /// <summary>
        /// the site theme URL.
        /// </summary>
        /// <retu+rns></returns>
        public static IHtmlString SiteThemeUrl(this IFrontUrlHelper frontUrl, string baseUri, string themeName)
        {
            var site = frontUrl.Site;
            return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, frontUrl.Url.Action("theme", "Resource", new { siteName = site.FullName, name = themeName, version = site.GetVersionUsedInUrl(), area = "" })).ToString());
        }

        ///// <summary>
        ///// the file URL under the theme of current site.
        ///// </summary>
        ///// <param name="relativeUrl">The relative URL.<example>images/logo.png</example></param>
        ///// <returns></returns>
        //public virtual IHtmlString ThemeFileUrl(this IFrontUrlHelper frontUrl, string relativeUrl)
        //{
        //    var site = frontUrl.Site;
        //    IHtmlString url = new HtmlString("");
        //    if (!string.IsNullOrEmpty(site.Name))
        //    {
        //        var fileExists = false;
        //        var themeFileUrl = "";
        //        do
        //        {
        //            site = site.AsActual();

        //            Theme theme = new Theme(site, site.Theme).LastVersion();
        //            themeFileUrl = Kooboo.Common.Web.UrlUtility.Combine(theme.VirtualPath, relativeUrl);
        //            var physicalPath = UrlUtility.MapPath(themeFileUrl);
        //            fileExists = File.Exists(physicalPath);

        //            site = theme.Site.Parent;
        //        } while (site != null && !fileExists);

        //        url = ResourceCDNUrl(themeFileUrl);
        //    }

        //    return url;

        //}

        /// <summary>
        /// Resources the URL. Using the ResourceDomain to generate the resource absolute url.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <returns></returns>
        public static IHtmlString ResourceCDNUrl(this IFrontUrlHelper frontUrl, string relativeUrl)
        {
            string resourceDomain = frontUrl.Site.DomainSetting.ResourceDomain;

            return new HtmlString(UrlUtility.ToHttpAbsolute(resourceDomain, relativeUrl));
        }
        //public virtual IHtmlString FileUrl(this IFrontUrlHelper frontUrl, string relativeFilePath)
        //{
        //    return FileUrl(frontUrl, relativeFilePath, true);
        //}
        ///// <summary>
        ///// The file URL.
        ///// </summary>
        ///// <param name="relativeFilePath">The relative file path.</param>
        ///// <param name="withCDNResolving">if set to <c>true</c> [with CDN resolving].</param>
        ///// <returns></returns>
        //public virtual IHtmlString FileUrl(this IFrontUrlHelper frontUrl, string relativeFilePath, bool withCDNResolving)
        //{
        //    Site site = this.Site;
        //    var dir = Path.GetDirectoryName(relativeFilePath);
        //    CustomFile file;
        //    if (string.IsNullOrEmpty(dir))
        //    {
        //        file = new CustomFile(site, relativeFilePath);
        //    }
        //    else
        //    {
        //        CustomDirectory customDir = new CustomDirectory(site, dir).LastVersion();
        //        file = new CustomFile(customDir, Path.GetFileName(relativeFilePath));
        //    }
        //    file = file.LastVersion();
        //    if (withCDNResolving)
        //    {
        //        return ResourceCDNUrl(file.VirtualPath);
        //    }
        //    else
        //    {
        //        return new HtmlString(Url.Content(file.VirtualPath));
        //    }

        //}
        //public virtual IHtmlString ScriptFileUrl(string relativeScriptFilePath)
        //{
        //    return ScriptFileUrl(relativeScriptFilePath, true);
        //}
        ///// <summary>
        ///// Get the file url under "Scripts" folder.
        ///// </summary>
        ///// <param name="relativeScriptFilePath">The relative file path.</param>
        ///// <param name="withCDNResolving">if set to <c>true</c> [with CDN resolving].</param>
        ///// <returns></returns>
        //public virtual IHtmlString ScriptFileUrl(string relativeScriptFilePath, bool withCDNResolving)
        //{
        //    Site site = this.Site;
        //    var dir = Path.GetDirectoryName(relativeScriptFilePath);
        //    var fileVirtualPath = "";

        //    if (string.IsNullOrEmpty(dir))
        //    {
        //        fileVirtualPath = new ScriptFile(site, relativeScriptFilePath).VirtualPath;
        //    }
        //    else
        //    {
        //        do
        //        {
        //            var scriptsPath = new ScriptFile(site, "");
        //            fileVirtualPath = UrlUtility.Combine(scriptsPath.VirtualPath, relativeScriptFilePath);
        //            var physicalPath = UrlUtility.MapPath(fileVirtualPath);
        //            if (File.Exists(physicalPath))
        //            {
        //                break;
        //            }
        //            else
        //            {
        //                site = site.Parent;
        //            }
        //        } while (site != null);
        //    }
        //    if (withCDNResolving)
        //    {
        //        return ResourceCDNUrl(fileVirtualPath);
        //    }
        //    else
        //    {
        //        return new HtmlString(Url.Content(fileVirtualPath));
        //    }

        //}
        #endregion
    }
}
