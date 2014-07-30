#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class LoadStylesExtensions
    {
        /// <summary>
        /// Includes the stylesheet.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="style">The style.</param>
        public static void IncludeStylesheet(this IFrontHtmlHelper frontHtml, string style)
        {
            frontHtml.Page_Context.Styles.Add(frontHtml.Html.Stylesheet(style));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        /// <param name="media"></param>
        public static void IncludeStylesheet(this IFrontHtmlHelper frontHtml, string style, string media)
        {
            frontHtml.Page_Context.Styles.Add(frontHtml.Html.Stylesheet(style, media));
        }

        #region LoadStyles
        /// <summary>
        /// Registers the styles.
        /// </summary>
        /// <returns></returns>
        public static IHtmlString LoadStyles(this IFrontHtmlHelper frontHtml)
        {
            return LoadStyles(frontHtml, frontHtml.Page_Context.PageTheme);
        }
        /// <summary>
        /// Registers the styles to the view.
        /// </summary>     
        /// <returns></returns>
        public static IHtmlString LoadStyles(this IFrontHtmlHelper frontHtml, string themeName)
        {
            return LoadAbsoluteStyles(frontHtml, frontHtml.Page_Context.PageRequestContext.Site.DomainSetting.ResourceDomain, themeName);
        }
        public static IHtmlString LoadAbsoluteStyles(this IFrontHtmlHelper frontHtml, string baseUri)
        {
            return LoadAbsoluteStyles(frontHtml, baseUri, frontHtml.Page_Context.PageTheme);
        }
        public static IHtmlString LoadAbsoluteStyles(this IFrontHtmlHelper frontHtml, string baseUri, string themeName)
        {
            IEnumerable<IHtmlString> styles = new IHtmlString[0];
            var key = "___RegisteredSystemStyles____";
            if (frontHtml.Html.ViewContext.HttpContext.Items[key] == null)
            {
                styles = styles
                    //.Concat(this.IncludeModuleThemeStyles(baseUri))
                    //.Concat(this.IncludeInlineEditingStyles(baseUri))
                    //.Concat(this.IncludeStyleEditingStyles(baseUri))
                  .Distinct(new IHtmlStringComparer());


                //if (this.PageContext.PageRequestContext.Site.EnableJquery)
                //{
                //    styles = styles.Concat(new[] { Kooboo.Common.Web.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, null, "jQuery-Styles", null, baseUri) });
                //}
                styles = styles.Concat(frontHtml.Page_Context.Styles);
                frontHtml.Html.ViewContext.HttpContext.Items[key] = new object();
            }

            styles = styles.Concat(IncludeThemeStyles(frontHtml, themeName, baseUri));

            return new AggregateHtmlString(styles);
        }

        //private IEnumerable<IHtmlString> IncludeInlineEditingStyles(string baseUri = null)
        //{
        //    if (PageContext.PageRequestContext.RequestChannel != FrontRequestChannel.Design)
        //    {
        //        if (PageContext.InlineEditing)
        //        {
        //            //Inline editing的脚本样式不能用CDN
        //            yield return Kooboo.Common.Web.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, "Sites", "inlineEditingCss", null, null);
        //        }
        //    }
        //}

        //private IEnumerable<IHtmlString> IncludeStyleEditingStyles(string baseUri = null)
        //{
        //    if (PageContext.PageRequestContext.RequestChannel != FrontRequestChannel.Design)
        //    {
        //        if (PageContext.StyleEditing)
        //        {
        //            //Inline editing的脚本样式不能用CDN
        //            yield return Kooboo.Common.Web.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, "Sites", "styleEditingFrontCss", null, null);
        //        }
        //    }
        //}

        private static IEnumerable<IHtmlString> IncludeThemeStyles(IFrontHtmlHelper frontHtml, string themeName, string baseUri = null)
        {
            var site = frontHtml.Page_Context.PageRequestContext.Site;
            if (!string.IsNullOrEmpty(themeName))
            {
                return IncludeThemeStyles(site, themeName, baseUri);
            }
            return new IHtmlString[0];
        }
        private static IEnumerable<IHtmlString> IncludeThemeStyles(Site site, string themeName, string baseUri = null)
        {
            List<IHtmlString> htmlStrings = new List<IHtmlString>();

            //string themeRuleBody;

            //var styles = ThemeRuleParser.Parse(new Theme(site, themeName).LastVersion(), out themeRuleBody, baseUri);

            //if (styles != null && styles.Count() > 0)
            //{
            //    if (site.Mode == ReleaseMode.Debug)
            //    {
            //        foreach (var style in styles)
            //        {
            //            var virtualPath = UrlUtility.ToHttpAbsolute(baseUri, style.VirtualPath);
            //            var dynamicCss = DynamicClientResourceFactory.Default.ResolveProvider(virtualPath);
            //            if (dynamicCss != null)
            //            {

            //                htmlStrings.Add(new HtmlString(dynamicCss.RegisterResource(virtualPath)));
            //            }
            //            else
            //            {
            //                htmlStrings.Add(this.Html.Stylesheet(virtualPath));
            //            }
            //        }
            //        foreach (var item in DynamicClientResourceFactory.Default.ResolveAllProviders().Where(it => it.ResourceType == ResourceType.Stylesheet))
            //        {
            //            htmlStrings.Add(new HtmlString(item.RegisterClientParser()));
            //        }
            //    }
            //    else
            //    {
            //        htmlStrings.Add(this.Html.Stylesheet(this.PageContext.FrontUrl.SiteThemeUrl(baseUri, themeName).ToString()));
            //    }
            //}

            //htmlStrings.Add(new HtmlString(themeRuleBody));

            return htmlStrings.Distinct(new IHtmlStringComparer());
        }
        //private IEnumerable<IHtmlString> IncludeModuleThemeStyles(string baseUri = null)
        //{
        //    var site = this.PageContext.PageRequestContext.Site;
        //    if (this.PageContext.PageRequestContext.Page.EnableTheming)
        //    {
        //        if (PageContext.ModuleResults != null)
        //        {
        //            foreach (ModuleActionInvokedContext actionInvoked in PageContext.ModuleResults.Values)
        //            {
        //                var moduleRequestContext = (ModuleRequestContext)actionInvoked.ControllerContext.RequestContext;
        //                if (moduleRequestContext.ModuleContext.FrontEndContext.EnableTheme && moduleRequestContext.ModuleContext.FrontEndContext.ModuleSettings != null && !string.IsNullOrEmpty(moduleRequestContext.ModuleContext.FrontEndContext.ModuleSettings.ThemeName))
        //                {
        //                    string themeRuleBody;
        //                    var styles = ServiceFactory.ModuleManager.AllThemeFiles(moduleRequestContext.ModuleContext.ModuleName,
        //                            moduleRequestContext.ModuleContext.FrontEndContext.ModuleSettings.ThemeName, out themeRuleBody);

        //                    if (site.Mode == ReleaseMode.Debug)
        //                    {
        //                        foreach (var style in styles)
        //                        {
        //                            yield return this.Html.Stylesheet(UrlUtility.ToHttpAbsolute(baseUri, style.VirtualPath));
        //                        }
        //                    }
        //                    else if (styles.Count() > 0)
        //                    {
        //                        yield return this.Html.Stylesheet(this.PageContext.FrontUrl.
        //                            ModuleThemeUrl(moduleRequestContext.ModuleContext.ModuleName, moduleRequestContext.ModuleContext.FrontEndContext.ModuleSettings.ThemeName, baseUri)
        //                            .ToString());
        //                    }
        //                    yield return new HtmlString(themeRuleBody);
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion
    }
}
