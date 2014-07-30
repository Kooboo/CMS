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
    public static class LoadScriptsExtensions
    {
        public static void IncludeScript(this IFrontHtmlHelper frontHtml, string script)
        {
            frontHtml.Page_Context.Scripts.Add(frontHtml.Html.Script(script));
        }

        #region LoadScripts
        /// <summary>
        /// Registers the scripts to the view.
        /// </summary>
        /// <returns></returns>
        public static IHtmlString LoadScripts(this IFrontHtmlHelper frontHtml)
        {
            return LoadAbsoluteScripts(frontHtml, frontHtml.Page_Context.PageRequestContext.Site.DomainSetting.ResourceDomain);
        }
        /// <summary>
        /// Registers the scripts.
        /// </summary>
        /// <param name="compressed">if set to <c>true</c> [compressed].</param>
        /// <returns></returns>
        public static IHtmlString LoadScripts(this IFrontHtmlHelper frontHtml, bool compressed)
        {
            return LoadAbsoluteScripts(frontHtml, frontHtml.Page_Context.PageRequestContext.Site.DomainSetting.ResourceDomain, compressed);
        }
        /// <summary>
        /// Registers the absolute scripts.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <returns></returns>
        public static IHtmlString LoadAbsoluteScripts(this IFrontHtmlHelper frontHtml, string baseUri)
        {
            return LoadAbsoluteScripts(frontHtml, baseUri, true);
        }
        /// <summary>
        /// Registers the absolute scripts.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="compressed">if set to <c>true</c> [compressed].</param>
        /// <returns></returns>
        public static IHtmlString LoadAbsoluteScripts(this IFrontHtmlHelper frontHtml, string baseUri, bool compressed)
        {
            return new AggregateHtmlString(IncludeInlineEditingScripts(frontHtml, baseUri)
                //.Concat(IncludeStyleEditingScripts(frontHtml, baseUri))
                //.Concat(IncludeModulesScripts(frontHtml, compressed, baseUri))
              .Concat(IncludeSiteScripts(frontHtml, compressed, baseUri))
              .Concat(frontHtml.Page_Context.Scripts)
              .Distinct(new IHtmlStringComparer()));
        }

        private static IEnumerable<IHtmlString> IncludeInlineEditingScripts(this IFrontHtmlHelper frontHtml, string baseUri = null)
        {
            //if (PageContext.PageRequestContext.RequestChannel != FrontRequestChannel.Design)
            //{
            //    if (PageContext.InlineEditing)
            //    {
            //        yield return InlineEditingVariablesScript();
            //        yield return this.Html.Script("~/Scripts/tiny_mce/tinymce.min.js");
            //        //Inline editing的脚本样式不能用CDN
            //        yield return Kooboo.Common.Web.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, "Sites", "inlineEditingJs", null, null);
            //    }
            //}

            return new IHtmlString[0];
        }
        //private static IHtmlString InlineEditingVariablesScript(this IFrontHtmlHelper frontHtml)
        //{
        //    var repository = PageContext.PageRequestContext.Site.GetRepository().AsActual();
        //    return this.Html.Script(PageContext.Url.Action("Variables"
        //            , new
        //            {
        //                controller = "InlineEditing",
        //                repositoryName = repository == null ? "" : repository.Name,
        //                siteName = PageContext.PageRequestContext.Site.FullName,
        //                pageName = PageContext.PageRequestContext.Page.FullName,
        //                area = "Sites",
        //                _draft_ = PageContext.ControllerContext.RequestContext.GetRequestValue("_draft_")
        //            }));
        //}

        //private IEnumerable<IHtmlString> IncludeStyleEditingScripts(string baseUri = null)
        //{
        //    if (PageContext.PageRequestContext.RequestChannel != FrontRequestChannel.Design)
        //    {
        //        if (PageContext.StyleEditing)
        //        {
        //            yield return this.StyleEditingVariablesScript();
        //            //Inline editing的脚本样式不能用CDN
        //            yield return Kooboo.Common.Web.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, "Sites", "styleEditingFrontJs", null, null);
        //        }
        //    }
        //}

        //private IHtmlString StyleEditingVariablesScript()
        //{
        //    var repository = PageContext.PageRequestContext.Site.GetRepository().AsActual();
        //    return this.Html.Script(PageContext.Url.Action("FrontVariables", new
        //    {
        //        controller = "StyleEditing",
        //        repositoryName = repository == null ? "" : repository.Name,
        //        siteName = PageContext.PageRequestContext.Site.FullName,
        //        pageName = PageContext.PageRequestContext.Page.FullName,
        //        area = "Sites",
        //        _draft_ = PageContext.ControllerContext.RequestContext.GetRequestValue("_draft_")
        //    }));
        //}

        private static IEnumerable<IHtmlString> IncludeSiteScripts(this IFrontHtmlHelper frontHtml, bool compressed, string baseUrl = null)
        {
            var site = frontHtml.Page_Context.PageRequestContext.Site;
            List<IHtmlString> scripts = new List<IHtmlString>();
            if (frontHtml.Page_Context.PageRequestContext.Page.EnableScript)
            {
                //if (frontHtml.Page_Context.PageRequestContext.Site.EnableJquery)
                //{
                //    scripts.Add(Kooboo.Common.Web.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, null, "jQuery", null, baseUrl));
                //}
                scripts.AddRange(GetScriptsBySite(site, "", compressed, baseUrl));
            }
            return scripts;
        }
        private static IEnumerable<IHtmlString> GetScriptsBySite(Site site, string folder, bool compressed, string baseUri = null)
        {
            List<IHtmlString> scripts = new List<IHtmlString>();

            //var siteScripts = ServiceFactory.ScriptManager.GetFiles(site, folder);
            //if (siteScripts != null && siteScripts.Count() > 0)
            //{
            //    if (site.Mode == ReleaseMode.Debug)
            //    {
            //        foreach (var script in siteScripts)
            //        {
            //            var virtualPath = UrlUtility.ToHttpAbsolute(baseUri, script.VirtualPath);
            //            var dynamicScript = DynamicClientResourceFactory.Default.ResolveProvider(virtualPath);
            //            if (dynamicScript != null)
            //            {
            //                scripts.Add(new HtmlString(dynamicScript.RegisterResource(virtualPath)));
            //            }
            //            else
            //            {
            //                scripts.Add(this.Html.Script(virtualPath));
            //            }
            //        }
            //        foreach (var item in DynamicClientResourceFactory.Default.ResolveAllProviders().Where(it => it.ResourceType == ResourceType.Javascript))
            //        {
            //            scripts.Add(new HtmlString(item.RegisterClientParser()));
            //        }
            //    }
            //    else
            //    {
            //        scripts.Add(this.Html.Script(this.PageContext.FrontUrl.SiteScriptsUrl(baseUri, folder, compressed).ToString()));
            //    }
            //}
            return scripts.Distinct(new IHtmlStringComparer());
        }
        private static IEnumerable<IHtmlString> IncludeModulesScripts(this IFrontHtmlHelper frontHtml, bool compressed, string baseUri = null)
        {
            //var site = frontHtml.Page_Context.PageRequestContext.Site;
            //if (frontHtml.Page_Context.PageRequestContext.Page.EnableScript)
            //{
            //    if (frontHtml.Page_Context.ModuleResults != null)
            //    {

            //        foreach (ModuleActionInvokedContext actionInvoked in PageContext.ModuleResults.Values)
            //        {
            //            var moduleRequestContext = (ModuleRequestContext)actionInvoked.ControllerContext.RequestContext;
            //            if (moduleRequestContext.ModuleContext.FrontEndContext.EnableScript)
            //            {
            //                var scripts = ServiceFactory.ModuleManager.AllScripts(moduleRequestContext.ModuleContext.ModuleName);
            //                if (site.Mode == ReleaseMode.Debug)
            //                {
            //                    foreach (var script in scripts)
            //                    {
            //                        yield return this.Html.Script(UrlUtility.ToHttpAbsolute(baseUri, script.VirtualPath));
            //                    }
            //                }
            //                else if (scripts.Count() > 0)
            //                {
            //                    yield return this.Html.Script(this.PageContext.FrontUrl.ModuleScriptsUrl(moduleRequestContext.ModuleContext.ModuleName, baseUri, compressed).ToString());
            //                }
            //            }

            //        }
            //    }
            //}

            return new IHtmlString[0];
        }

        #region RegisterScriptFolder
        public static IHtmlString LoadScriptFolder(this IFrontHtmlHelper frontHtml, string folder)
        {
            return LoadScriptFolder(frontHtml, folder, true, frontHtml.Page_Context.PageRequestContext.Site.DomainSetting.ResourceDomain);
        }
        public static IHtmlString LoadScriptFolder(this IFrontHtmlHelper frontHtml, string folder, bool compressed, string baseUri)
        {
            return new AggregateHtmlString(GetScriptsBySite(frontHtml.Page_Context.PageRequestContext.Site, folder, compressed, baseUri));
        }
        #endregion

        #endregion
    }
}
