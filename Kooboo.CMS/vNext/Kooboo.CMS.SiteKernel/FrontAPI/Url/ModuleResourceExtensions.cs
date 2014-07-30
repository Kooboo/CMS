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
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class ModuleResourceExtensions
    {
        ///// <summary>
        ///// The URL for module scripts.
        ///// </summary>
        ///// <param name="moduleName">Name of the module.</param>
        ///// <returns></returns>
        //public virtual IHtmlString ModuleScriptsUrl(string moduleName)
        //{
        //    return ModuleScriptsUrl(moduleName, this.Site.ResourceDomain);
        //}
        ///// <summary>
        ///// The URL for module scripts.
        ///// </summary>
        ///// <param name="moduleName">Name of the module.</param>
        ///// <param name="baseUri">The base URI.</param>
        ///// <returns></returns>
        //public virtual IHtmlString ModuleScriptsUrl(string moduleName, string baseUri)
        //{
        //    return ModuleScriptsUrl(moduleName, baseUri, true);
        //}
        ///// <summary>
        ///// The URL for module scripts.
        ///// </summary>
        ///// <param name="moduleName">Name of the module.</param>
        ///// <param name="baseUri">The base URI.</param>
        ///// <param name="compressed">if set to <c>true</c> [compressed].</param>
        ///// <returns></returns>
        //public virtual IHtmlString ModuleScriptsUrl(string moduleName, string baseUri, bool compressed)
        //{
        //    Site site = this.Site;
        //    return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, this.Url.Action("ModuleScripts", "Resource", new { siteName = site.FullName, moduleName = moduleName, version = site.VersionUsedInUrl, area = "", compressed = compressed })).ToString());
        //}
        ///// <summary>
        ///// Modules the theme URL.
        ///// </summary>
        ///// <param name="moduleName">Name of the module.</param>
        ///// <param name="themeName">Name of the theme.</param>
        ///// <returns></returns>
        //public virtual IHtmlString ModuleThemeUrl(string moduleName, string themeName)
        //{
        //    return ModuleThemeUrl(moduleName, themeName, this.Site.ResourceDomain);
        //}
        //public virtual IHtmlString ModuleThemeUrl(string moduleName, string themeName, string baseUri)
        //{
        //    Site site = this.Site;
        //    return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, this.Url.Action("ModuleTheme", "Resource", new { siteName = site.FullName, moduleName = moduleName, name = themeName, version = site.VersionUsedInUrl, area = "" })).ToString());
        //}
    }
}
