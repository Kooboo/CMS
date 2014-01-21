#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Caching;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Parsers.ThemeRule;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.View.PositionRender;
using Kooboo.CMS.Sites.Web;
using Kooboo.Globalization;
using Kooboo.Web;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Html;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Web.Url;
using Kooboo.CMS.Sites.Membership;
//# define Page_Trace
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Kooboo.CMS.Sites.Extension.ModuleArea.Runtime;
using Kooboo.Web.Mvc.WebResourceLoader.DynamicClientResource;
namespace Kooboo.CMS.Sites.View
{
    /// <summary>
    /// The html helper for Kooboo CMS page, similar MVC HtmlHelper. 
    /// </summary>
    public class FrontHtmlHelper
    {
        #region Static

        public static string GeneratedByHtmlBlockComment = @"
<!--Html block:{0}-->
{1}
<!--Html block:/{0}-->";
        #endregion

        #region .ctor
        public Page_Context PageContext { get; private set; }

        public HtmlHelper Html { get; private set; }

        public ViewRender ViewRender { get; private set; }

        public ProxyRender ProxyRender { get; private set; }

        public FrontHtmlHelper(Page_Context context, HtmlHelper html, ViewRender viewRender, ProxyRender proxyRender)
        {
            this.PageContext = context;
            this.Html = html;
            this.ViewRender = viewRender;
            this.ProxyRender = proxyRender;
        }
        #endregion

        #region Position

        #region IsEmptyPosition
        public virtual bool IsEmptyPosition(string positionID)
        {
            if (PageContext.PageRequestContext.RequestChannel == FrontRequestChannel.Design)
            {
                return false;
            }
            else
            {
                return GetContentsForPosition(positionID).Length == 0;
            }
        }
        private PagePosition[] GetContentsForPosition(string positionID)
        {
            var positions = (PageContext.PageRequestContext.Page.PagePositions ?? new List<PagePosition>()).Where(it => it.LayoutPositionId.Equals(positionID, StringComparison.InvariantCultureIgnoreCase)).ToArray();
            return positions;
        }
        #endregion

        #region Position
        public virtual IHtmlString Position(string positionID)
        {
            return Position(positionID, "");
        }

        public virtual IHtmlString Position(string positionID, string defaultContent)
        {
            return Position(positionID, () => defaultContent);
        }
        public virtual IHtmlString Position(string positionID, Func<string> defaultContentFunc)
        {
            if (PageContext.PageRequestContext.RequestChannel == FrontRequestChannel.Design)
            {
                return new PageDesignHolder(this, positionID);
            }
            else
            {
                var positions = GetContentsForPosition(positionID);
                if (positions.Length == 0)
                {
                    defaultContentFunc = defaultContentFunc == null ? () => "" : defaultContentFunc;
                    return new HtmlString(defaultContentFunc());
                }
                else
                {
                    var htmlStrings = RenderPositionContents(positions).ToArray();
                    return new AggregateHtmlString(htmlStrings);
                }

            }
        }

        public virtual IHtmlString Position(string positionID, bool requireMembershipAuthentication, params string[] membershipGroups)
        {
            if (PageContext.PageRequestContext.RequestChannel == FrontRequestChannel.Design)
            {
                return new PageDesignHolder(this, positionID);
            }
            else
            {
                if (requireMembershipAuthentication)
                {
                    var permission = new PagePermission() { RequireMember = requireMembershipAuthentication, AllowGroups = membershipGroups };
                    if (!permission.Authorize(Html.ViewContext.HttpContext.Membership().GetMember()))
                    {
                        return new HtmlString("");
                    }
                }
                return Position(positionID);
            }
        }
        #endregion

        #region RenderPositionContents
        protected virtual IEnumerable<IHtmlString> RenderPositionContents(IEnumerable<PagePosition> positions)
        {
            foreach (var position in positions.OrderBy(it => it.Order))
            {
                if (position is ViewPosition)
                {
                    yield return this.RenderView(((ViewPosition)position));
                }
                else if (position is ModulePosition)
                {
                    yield return new HtmlString(RenderModule((ModulePosition)position));
                }
                else if (position is HtmlPosition)
                {
                    yield return RenderHtmlPosition((HtmlPosition)position);
                }
                else if (position is ContentPosition)
                {
                    yield return RenderContentPosition((ContentPosition)position);
                }
                else if (position is HtmlBlockPosition)
                {
                    yield return RenderHtmlBlockPosition((HtmlBlockPosition)position);
                }
                else if (position is ProxyPosition)
                {
                    yield return RenderProxyPosition((ProxyPosition)position);
                }
            }
        }
        #endregion

        #region RenderProxyPosition
        protected virtual IHtmlString RenderProxyPosition(ProxyPosition proxyPosition)
        {
            return ProxyRender.Render(PageContext.ControllerContext, PageContext.PageRequestContext, proxyPosition, null);
        }
        #endregion

        #region RenderHtmlBlockPosition
        protected virtual IHtmlString RenderHtmlBlockPosition(HtmlBlockPosition htmlBlockPosition)
        {
            return RenderHtmlBlock(htmlBlockPosition.BlockName);
        }
        #endregion

        #region RenderHtmlPosition
        protected virtual IHtmlString RenderHtmlPosition(HtmlPosition htmlPosition)
        {
            string html = htmlPosition.Html;
            if (PageContext.EnabledInlineEditing(EditingType.Page) && PageContext.PageRequestContext.Page.IsLocalized(PageContext.PageRequestContext.Site))
            {
                if (PageContext.PageRequestContext.Page.Published.Value == false
                    || (PageContext.PageRequestContext.Page.Published.Value == true && ServiceFactory.UserManager.Authorize(PageContext.PageRequestContext.Site, PageContext.ControllerContext.HttpContext.User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Page_PublishPermission)))
                {
                    html = string.Format("<var start=\"true\" editType=\"html\" dataType=\"{0}\" positionId=\"{1}\" positionName=\"{2}\" style=\"display:none;\"></var>{3}<var end=\"true\" style=\"display:none;\"></var>", FieldDataType.RichText.ToString(), htmlPosition.PagePositionId, HttpUtility.HtmlAttributeEncode(htmlPosition.ToString()), html);
                }
            }
            return new HtmlString(html);
        }
        #endregion

        #region RenderView
        protected internal virtual IHtmlString RenderView(ViewPosition viewPosition)
        {
            try
            {
                Func<IHtmlString> renderView = () => this.RenderView(viewPosition.ViewName, PageContext.GetPositionViewData(viewPosition.PagePositionId), viewPosition.ToParameterDictionary(), false);
                if (viewPosition.EnabledCache)
                {
                    var cacheKey = string.Format("View OutputCache - Full page name:{0};Raw request url:{1};PagePositionId:{2};ViewName:{3};LayoutPositionId:{4}"
                    , PageContext.PageRequestContext.Page.FullName, PageContext.ControllerContext.HttpContext.Request.RawUrl, viewPosition.PagePositionId, viewPosition.ViewName, viewPosition.LayoutPositionId);
                    var cacheItemPolicy = viewPosition.OutputCache.ToCachePolicy();
                    return this.PageContext.PageRequestContext.Site.ObjectCache().GetCache<IHtmlString>(cacheKey,
                              renderView,
                          cacheItemPolicy);
                }
                else
                {
                    return renderView();
                }
            }
            catch (Exception e)
            {
                if (viewPosition.SkipError)
                {
                    Kooboo.HealthMonitoring.Log.LogException(e);
                    return new HtmlString("");
                }

                throw;
            }

        }
        #endregion

        #region RenderModule
        protected virtual string RenderModule(ModulePosition modulePosition)
        {
            ModuleActionInvokedContext actionInvokedContext = GetModuleActionResult(modulePosition.PagePositionId);
            if (actionInvokedContext != null)
            {
                var actionResultExecuted = ModuleExecutor.ExecuteActionResult(actionInvokedContext);
                return actionResultExecuted.ResultHtml;
            }
            return string.Empty;
        }
        protected virtual ModuleActionInvokedContext GetModuleActionResult(string pagePositionId)
        {
            IDictionary<string, ModuleActionInvokedContext> moduleActionResults = this.PageContext.ModuleResults;
            if (moduleActionResults.ContainsKey(pagePositionId))
            {
                return moduleActionResults[pagePositionId];
            }
            return null;
        }

        #endregion

        #region RenderContentPosition
        protected virtual IHtmlString RenderContentPosition(ContentPosition contentPosition)
        {
            var site = this.PageContext.PageRequestContext.Site;
            var repository = site.GetRepository();
            if (repository == null)
            {
                throw new KoobooException("The repository for site is null.");
            }
            var dataRule = (IContentDataRule)(contentPosition.DataRule);
            var dataRuleContext = new DataRuleContext(this.PageContext.PageRequestContext.Site,
                this.PageContext.PageRequestContext.Page) { ValueProvider = this.PageContext.PageRequestContext.GetValueProvider() };

            string viewPath = "";
            TakeOperation operation;
            var schema = dataRule.GetSchema(repository);
            switch (contentPosition.Type)
            {
                case ContentPositionType.Detail:
                    viewPath = schema.GetFormTemplate(FormType.Detail);
                    operation = TakeOperation.First;
                    break;
                case ContentPositionType.List:
                default:
                    viewPath = schema.GetFormTemplate(FormType.List);
                    operation = TakeOperation.List;
                    break;
            }
            var model = dataRule.Execute(dataRuleContext, operation, 0);
            return ViewRender.RenderViewInternal(this.Html, viewPath, null, model);
        }
        #endregion
        #endregion

        #region Partial Render

        #region Render htmlblock
        public virtual IHtmlString RenderHtmlBlock(string htmlBlockName)
        {
            var htmlBlock = (new HtmlBlock(PageContext.PageRequestContext.Site, htmlBlockName).LastVersion()).AsActual();
            var htmlString = new HtmlString("");
            if (htmlBlock != null)
            {
                var body = htmlBlock.Body;
                if (PageContext.EnabledInlineEditing(EditingType.Page) && htmlBlock.IsLocalized(PageContext.PageRequestContext.Site))
                {
                    if (PageContext.PageRequestContext.Page.Published == false
                        || (PageContext.PageRequestContext.Page.Published == true && ServiceFactory.UserManager.Authorize(PageContext.PageRequestContext.Site, PageContext.ControllerContext.HttpContext.User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Page_PublishPermission)))
                    {
                        body = string.Format("<var start=\"true\" editType=\"htmlBlock\" positionId=\"{0}\" positionName=\"{1}\" blockName=\"{2}\" style=\"display:none;\"></var>{3}<var end=\"true\" style=\"display:none;\"></var>", "", "", HttpUtility.HtmlAttributeEncode(htmlBlock.Name), body);
                    }
                }


                if (PageContext.EnableTrace)
                {
                    body = string.Format(GeneratedByHtmlBlockComment, htmlBlockName, body);
                }
                htmlString = new HtmlString(body);
            }

            return htmlString;

        }
        #endregion

        #region RenderView
        /// <summary>
        /// Renders the view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        public virtual IHtmlString RenderView(string viewName)
        {
            var viewData = new ViewDataDictionary(PageContext.ControllerContext.Controller.ViewData);
            return RenderView(viewName, viewData);
        }
        /// <summary>
        /// Renders the view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual IHtmlString RenderView(string viewName, object model)
        {
            var viewData = new ViewDataDictionary(PageContext.ControllerContext.Controller.ViewData);
            viewData.Model = model;
            return RenderView(viewName, viewData);
        }
        /// <summary>
        /// Renders the view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="viewData">The view data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public virtual IHtmlString RenderView(string viewName, ViewDataDictionary viewData, object parameters)
        {
            return RenderView(viewName, viewData, parameters, true);
        }
        /// <summary>
        /// Renders the view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="viewData">The view data.</param>
        /// <returns></returns>
        public virtual IHtmlString RenderView(string viewName, ViewDataDictionary viewData)
        {
            return RenderView(viewName, viewData, null, true);
        }

        /// <summary>
        /// Renders the view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="viewData">The view data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="executeDataRule">if set to <c>true</c> [execute data rule].</param>
        /// <returns></returns>
        public virtual IHtmlString RenderView(string viewName, ViewDataDictionary viewData, object parameters, bool executeDataRule)
        {
            return ViewRender.RenderView(this.Html, PageContext, viewName, viewData, parameters, executeDataRule);
        }

        #endregion

        #region RenderModule
        public virtual IHtmlString RenderModule(string moduleName, string moduleController, string moduleAction)
        {
            return RenderModule(moduleName, moduleController, moduleAction, null);
        }
        public virtual IHtmlString RenderModule(string moduleName, string moduleController, string moduleAction, object routeValues)
        {
            var routeDictionary = new RouteValueDictionary();
            if (routeValues != null)
            {
                routeDictionary = new RouteValueDictionary(routeValues);
            }
            ModulePosition position = new ModulePosition()
            {
                Exclusive = false
                ,
                PagePositionId = moduleName
                ,
                ModuleName = moduleName
                ,
                Entry = new Entry() { Controller = moduleController, Action = moduleAction, Values = routeDictionary }
            };
            return RenderModule(moduleName, null, position);
        }
        public virtual IHtmlString RenderModule(string moduleName, string moduleUrl)
        {
            ModulePosition position = new ModulePosition()
            {
                Exclusive = false
                ,
                PagePositionId = moduleName
                ,
                ModuleName = moduleName
                ,
                Entry = new Entry()
            };
            return RenderModule(moduleName, moduleUrl, position);
        }
        public virtual IHtmlString RenderModule(string moduleName, string moduleUrl, ModulePosition modulePosition)
        {
            var result = ModuleExecutor.InvokeAction(PageContext.ControllerContext, PageContext.PageRequestContext.Site, moduleUrl, modulePosition);
            var actionResultExecuted = ModuleExecutor.ExecuteActionResult(result);
            return new HtmlString(actionResultExecuted.ResultHtml);
        }
        #endregion

        #endregion

        #region Register scripts & styles

        #region IncludeScript IncludeStylesheet
        /// <summary>
        /// Includes a script file to current page.
        /// </summary>    
        /// <param name="script">The script.</param>
        public virtual void IncludeScript(string script)
        {
            this.PageContext.Scripts.Add(this.Html.Script(script));
        }
        /// <summary>
        /// Includes the stylesheet.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="style">The style.</param>
        public virtual void IncludeStylesheet(string style)
        {
            this.PageContext.Styles.Add(this.Html.Stylesheet(style));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        /// <param name="media"></param>
        public virtual void IncludeStylesheet(string style, string media)
        {
            this.PageContext.Styles.Add(this.Html.Stylesheet(style, media));
        }
        #endregion

        #region RegisterScripts
        /// <summary>
        /// Registers the scripts to the view.
        /// </summary>
        /// <returns></returns>
        public virtual IHtmlString RegisterScripts()
        {
            return RegisterAbsoluteScripts(PageContext.PageRequestContext.Site.ResourceDomain);
        }
        /// <summary>
        /// Registers the scripts.
        /// </summary>
        /// <param name="compressed">if set to <c>true</c> [compressed].</param>
        /// <returns></returns>
        public virtual IHtmlString RegisterScripts(bool compressed)
        {
            return RegisterAbsoluteScripts(PageContext.PageRequestContext.Site.ResourceDomain, compressed);
        }
        /// <summary>
        /// Registers the absolute scripts.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <returns></returns>
        public virtual IHtmlString RegisterAbsoluteScripts(string baseUri)
        {
            return RegisterAbsoluteScripts(baseUri, true);
        }
        /// <summary>
        /// Registers the absolute scripts.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="compressed">if set to <c>true</c> [compressed].</param>
        /// <returns></returns>
        public virtual IHtmlString RegisterAbsoluteScripts(string baseUri, bool compressed)
        {
            return new AggregateHtmlString(this.IncludeInlineEditingScripts(baseUri)
              .Concat(this.IncludeStyleEditingScripts(baseUri))
              .Concat(this.IncludeModulesScripts(compressed, baseUri))
              .Concat(this.IncludeSiteScripts(compressed, baseUri))
              .Concat(this.PageContext.Scripts)
              .Distinct(new IHtmlStringComparer()));
        }

        private IEnumerable<IHtmlString> IncludeInlineEditingScripts(string baseUri = null)
        {
            if (PageContext.PageRequestContext.RequestChannel != FrontRequestChannel.Design)
            {
                if (PageContext.InlineEditing)
                {
                    yield return InlineEditingVariablesScript();
                    yield return this.Html.Script("~/Scripts/tiny_mce/tinymce.min.js");
                    //Inline editing的脚本样式不能用CDN
                    yield return Kooboo.Web.Mvc.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, "Sites", "inlineEditingJs", null, null);
                }
            }
        }
        private IHtmlString InlineEditingVariablesScript()
        {
            var repository = PageContext.PageRequestContext.Site.GetRepository().AsActual();
            return this.Html.Script(PageContext.Url.Action("Variables"
                    , new
                    {
                        controller = "InlineEditing",
                        repositoryName = repository == null ? "" : repository.Name,
                        siteName = PageContext.PageRequestContext.Site.FullName,
                        pageName = PageContext.PageRequestContext.Page.FullName,
                        area = "Sites",
                        _draft_ = PageContext.ControllerContext.RequestContext.GetRequestValue("_draft_")
                    }));
        }

        private IEnumerable<IHtmlString> IncludeStyleEditingScripts(string baseUri = null)
        {
            if (PageContext.PageRequestContext.RequestChannel != FrontRequestChannel.Design)
            {
                if (PageContext.StyleEditing)
                {
                    yield return this.StyleEditingVariablesScript();
                    //Inline editing的脚本样式不能用CDN
                    yield return Kooboo.Web.Mvc.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, "Sites", "styleEditingFrontJs", null, null);
                }
            }
        }

        private IHtmlString StyleEditingVariablesScript()
        {
            var repository = PageContext.PageRequestContext.Site.GetRepository().AsActual();
            return this.Html.Script(PageContext.Url.Action("FrontVariables", new
            {
                controller = "StyleEditing",
                repositoryName = repository == null ? "" : repository.Name,
                siteName = PageContext.PageRequestContext.Site.FullName,
                pageName = PageContext.PageRequestContext.Page.FullName,
                area = "Sites",
                _draft_ = PageContext.ControllerContext.RequestContext.GetRequestValue("_draft_")
            }));
        }

        private IEnumerable<IHtmlString> IncludeSiteScripts(bool compressed, string baseUrl = null)
        {
            var site = this.PageContext.PageRequestContext.Site;
            List<IHtmlString> scripts = new List<IHtmlString>();
            if (this.PageContext.PageRequestContext.Page.EnableScript)
            {
                if (this.PageContext.PageRequestContext.Site.EnableJquery)
                {
                    scripts.Add(Kooboo.Web.Mvc.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, null, "jQuery", null, baseUrl));
                }
                scripts.AddRange(GetScriptsBySite(site, "", compressed, baseUrl));
            }
            return scripts;
        }
        private IEnumerable<IHtmlString> GetScriptsBySite(Site site, string folder, bool compressed, string baseUri = null)
        {
            List<IHtmlString> scripts = new List<IHtmlString>();

            var siteScripts = ServiceFactory.ScriptManager.GetFiles(site, folder);
            if (siteScripts != null && siteScripts.Count() > 0)
            {
                if (site.Mode == ReleaseMode.Debug)
                {
                    foreach (var script in siteScripts)
                    {
                        var virtualPath = Kooboo.Web.Url.UrlUtility.ToHttpAbsolute(baseUri, script.VirtualPath);
                        var dynamicScript = DynamicClientResourceFactory.Default.ResolveProvider(virtualPath);
                        if (dynamicScript != null)
                        {
                            scripts.Add(new HtmlString(dynamicScript.RegisterResource(virtualPath)));
                        }
                        else
                        {
                            scripts.Add(this.Html.Script(virtualPath));
                        }
                    }
                    foreach (var item in DynamicClientResourceFactory.Default.ResolveAllProviders().Where(it => it.ResourceType == ResourceType.Javascript))
                    {
                        scripts.Add(new HtmlString(item.RegisterClientParser()));
                    }
                }
                else
                {
                    scripts.Add(this.Html.Script(this.PageContext.FrontUrl.SiteScriptsUrl(baseUri, folder, compressed).ToString()));
                }
            }

            //else
            //{
            //    if (site.Parent != null)
            //    {
            //        return GetScriptsBySite(site.Parent, baseUrl);
            //    }
            //}
            return scripts.Distinct(new IHtmlStringComparer());
        }
        private IEnumerable<IHtmlString> IncludeModulesScripts(bool compressed, string baseUri = null)
        {
            var site = this.PageContext.PageRequestContext.Site;
            if (this.PageContext.PageRequestContext.Page.EnableScript)
            {
                if (PageContext.ModuleResults != null)
                {

                    foreach (ModuleActionInvokedContext actionInvoked in PageContext.ModuleResults.Values)
                    {
                        var moduleRequestContext = (ModuleRequestContext)actionInvoked.ControllerContext.RequestContext;
                        if (moduleRequestContext.ModuleContext.FrontEndContext.EnableScript)
                        {
                            if (site.Mode == ReleaseMode.Debug)
                            {
                                foreach (var script in ServiceFactory.ModuleManager.AllScripts(moduleRequestContext.ModuleContext.ModuleName))
                                {
                                    yield return this.Html.Script(UrlUtility.ToHttpAbsolute(baseUri, script.VirtualPath));
                                }
                            }
                            else
                            {
                                yield return this.Html.Script(this.PageContext.FrontUrl.ModuleScriptsUrl(moduleRequestContext.ModuleContext.ModuleName, baseUri, compressed).ToString());
                            }
                        }

                    }
                }
            }
        }

        #region RegisterScriptFolder
        public virtual IHtmlString RegisterScriptFolder(string folder)
        {
            return RegisterScriptFolder(folder, true, PageContext.PageRequestContext.Site.ResourceDomain);
        }
        public virtual IHtmlString RegisterScriptFolder(string folder, bool compressed, string baseUri)
        {
            return new AggregateHtmlString(GetScriptsBySite(PageContext.PageRequestContext.Site, folder, compressed, baseUri));
        }
        #endregion

        #endregion

        #region RegisterStyles
        /// <summary>
        /// Registers the styles.
        /// </summary>
        /// <returns></returns>
        public virtual IHtmlString RegisterStyles()
        {
            return RegisterStyles(PageContext.PageTheme);
        }
        /// <summary>
        /// Registers the styles to the view.
        /// </summary>     
        /// <returns></returns>
        public virtual IHtmlString RegisterStyles(string themeName)
        {
            return RegisterAbsoluteStyles(PageContext.PageRequestContext.Site.ResourceDomain, themeName);
        }
        public virtual IHtmlString RegisterAbsoluteStyles(string baseUri)
        {
            return RegisterAbsoluteStyles(baseUri, PageContext.PageTheme);
        }
        public virtual IHtmlString RegisterAbsoluteStyles(string baseUri, string themeName)
        {
            IEnumerable<IHtmlString> styles = new IHtmlString[0];
            var key = "___RegisteredSystemStyles____";
            if (Html.ViewContext.HttpContext.Items[key] == null)
            {
                styles = styles
                  .Concat(this.IncludeModuleThemeStyles(baseUri))
                  .Concat(this.IncludeInlineEditingStyles(baseUri))
                  .Concat(this.IncludeStyleEditingStyles(baseUri))
                  .Distinct(new IHtmlStringComparer());

                Html.ViewContext.HttpContext.Items[key] = new object();
            }
            styles = styles.Concat(this.IncludeThemeStyles(themeName, baseUri))
                .Concat(this.PageContext.Styles);
            return new AggregateHtmlString(styles);
        }

        private IEnumerable<IHtmlString> IncludeInlineEditingStyles(string baseUri = null)
        {
            if (PageContext.PageRequestContext.RequestChannel != FrontRequestChannel.Design)
            {
                if (PageContext.InlineEditing)
                {
                    //Inline editing的脚本样式不能用CDN
                    yield return Kooboo.Web.Mvc.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, "Sites", "inlineEditingCss", null, null);
                }
            }
        }

        private IEnumerable<IHtmlString> IncludeStyleEditingStyles(string baseUri = null)
        {
            if (PageContext.PageRequestContext.RequestChannel != FrontRequestChannel.Design)
            {
                if (PageContext.StyleEditing)
                {
                    //Inline editing的脚本样式不能用CDN
                    yield return Kooboo.Web.Mvc.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, "Sites", "styleEditingFrontCss", null, null);
                }
            }
        }

        private IEnumerable<IHtmlString> IncludeThemeStyles(string themeName, string baseUri = null)
        {
            var site = this.PageContext.PageRequestContext.Site;
            if (!string.IsNullOrEmpty(themeName))
            {
                return IncludeThemeStyles(site, themeName, baseUri);
            }
            return new IHtmlString[0];
        }
        private IEnumerable<IHtmlString> IncludeThemeStyles(Site site, string themeName, string baseUri = null)
        {
            List<IHtmlString> htmlStrings = new List<IHtmlString>();
            if (this.PageContext.PageRequestContext.Site.EnableJquery)
            {
                htmlStrings.Add(Kooboo.Web.Mvc.WebResourceLoader.MvcExtensions.ExternalResources(this.Html, null, "jQuery-Styles", null, baseUri));
            }

            string themeRuleBody;

            var styles = ThemeRuleParser.Parse(new Theme(site, themeName).LastVersion(), out themeRuleBody, baseUri);

            if (styles != null && styles.Count() > 0)
            {
                if (site.Mode == ReleaseMode.Debug)
                {
                    foreach (var style in styles)
                    {
                        var virtualPath = UrlUtility.ToHttpAbsolute(baseUri, style.VirtualPath);
                        var dynamicCss = DynamicClientResourceFactory.Default.ResolveProvider(virtualPath);
                        if (dynamicCss != null)
                        {

                            htmlStrings.Add(new HtmlString(dynamicCss.RegisterResource(virtualPath)));
                        }
                        else
                        {
                            htmlStrings.Add(this.Html.Stylesheet(virtualPath));
                        }
                    }
                    foreach (var item in DynamicClientResourceFactory.Default.ResolveAllProviders().Where(it => it.ResourceType == ResourceType.Stylesheet))
                    {
                        htmlStrings.Add(new HtmlString(item.RegisterClientParser()));
                    }
                }
                else
                {
                    htmlStrings.Add(this.Html.Stylesheet(this.PageContext.FrontUrl.SiteThemeUrl(baseUri, themeName).ToString()));
                }
            }

            htmlStrings.Add(new HtmlString(themeRuleBody));

            return htmlStrings.Distinct(new IHtmlStringComparer());
        }
        private IEnumerable<IHtmlString> IncludeModuleThemeStyles(string baseUri = null)
        {
            var site = this.PageContext.PageRequestContext.Site;
            if (this.PageContext.PageRequestContext.Page.EnableTheming)
            {
                if (PageContext.ModuleResults != null)
                {
                    foreach (ModuleActionInvokedContext actionInvoked in PageContext.ModuleResults.Values)
                    {
                        var moduleRequestContext = (ModuleRequestContext)actionInvoked.ControllerContext.RequestContext;
                        if (moduleRequestContext.ModuleContext.FrontEndContext.EnableTheme && moduleRequestContext.ModuleContext.FrontEndContext.ModuleSettings != null && !string.IsNullOrEmpty(moduleRequestContext.ModuleContext.FrontEndContext.ModuleSettings.ThemeName))
                        {
                            string themeRuleBody;
                            var styles = ServiceFactory.ModuleManager.AllThemeFiles(moduleRequestContext.ModuleContext.ModuleName,
                                    moduleRequestContext.ModuleContext.FrontEndContext.ModuleSettings.ThemeName, out themeRuleBody);

                            if (site.Mode == ReleaseMode.Debug)
                            {
                                foreach (var style in styles)
                                {
                                    yield return this.Html.Stylesheet(UrlUtility.ToHttpAbsolute(baseUri, style.VirtualPath));
                                }
                            }
                            else
                            {
                                yield return this.Html.Stylesheet(this.PageContext.FrontUrl.
                                    ModuleThemeUrl(moduleRequestContext.ModuleContext.ModuleName, moduleRequestContext.ModuleContext.FrontEndContext.ModuleSettings.ThemeName, baseUri)
                                    .ToString());
                            }
                            yield return new HtmlString(themeRuleBody);
                        }
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Title & meta
        [Obsolete("Use HtmlTitle")]
        public IHtmlString Title()
        {
            return HtmlTitle();
        }
        /// <summary>
        /// HTMLs the title.
        /// </summary>
        /// <returns></returns>
        public virtual IHtmlString HtmlTitle()
        {
            return HtmlTitle(null);
        }
        /// <summary>
        /// HTMLs the title.
        /// </summary>
        /// <param name="defaultTitle">The HTML title.</param>
        /// <returns></returns>
        public virtual IHtmlString HtmlTitle(string defaultTitle)
        {
            var title = string.IsNullOrEmpty(this.PageContext.HtmlMeta.HtmlTitle) ? defaultTitle : this.PageContext.HtmlMeta.HtmlTitle;
            if (!string.IsNullOrEmpty(title))
            {
                return new HtmlString(string.Format("<title>{0}</title>", Kooboo.StringExtensions.StripAllTags(title)));
            }
            return new HtmlString("");
        }
        public virtual IHtmlString Meta()
        {
            AggregateHtmlString htmlStrings = new AggregateHtmlString();
            var htmlMeta = this.PageContext.HtmlMeta;
            if (htmlMeta != null)
            {
                if (!string.IsNullOrEmpty(htmlMeta.Canonical))
                {
                    htmlStrings.Add(new HtmlString(string.Format("<link rel=\"canonical\" href=\"{0}\"/>", Kooboo.StringExtensions.StripAllTags(htmlMeta.Canonical))));
                }
                if (!string.IsNullOrEmpty(htmlMeta.Author))
                {
                    htmlStrings.Add(BuildMeta("author", htmlMeta.Author));
                }

                if (!string.IsNullOrEmpty(htmlMeta.Description))
                {
                    htmlStrings.Add(BuildMeta("description", htmlMeta.Description));
                }

                if (!string.IsNullOrEmpty(htmlMeta.Keywords))
                {
                    htmlStrings.Add(BuildMeta("keywords", htmlMeta.Keywords));
                }
                if (htmlMeta.Customs != null)
                {
                    foreach (var item in htmlMeta.Customs)
                    {
                        htmlStrings.Add(BuildMeta(item.Key, item.Value));
                    }
                }
            }
            return htmlStrings;
        }
        private static IHtmlString BuildMeta(string name, string value)
        {
            return new HtmlString(string.Format("<meta name=\"{0}\" content=\"{1}\" />", name, Kooboo.StringExtensions.StripAllTags(value)));
        }
        #endregion

        #region PageLink
        public virtual IHtmlString PageLink(string urlMapKey)
        {
            return this.PageLink(null, urlMapKey);
        }
        public virtual IHtmlString PageLink(object linkText, string urlMapKey)
        {
            return this.PageLink(linkText, urlMapKey, null);
        }
        public virtual IHtmlString PageLink(object linkText, string urlMapKey, object routeValues)
        {
            return this.PageLink(linkText, urlMapKey, routeValues, null);
        }
        public virtual IHtmlString PageLink(object linkText, string urlMapKey, object routeValues, object htmlAttributes)
        {
#if Page_Trace
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            var htmlValues = RouteValuesHelpers.GetRouteValues(htmlAttributes);
            Page page;

            string url = this.PageContext.FrontUrl.PageUrl(urlMapKey, routeValues, out page).ToString();

            page = page.AsActual();

            var innerHtml = "";
            if (linkText == null)
            {
                innerHtml = page.LinkText;
            }
            else
            {
                innerHtml = HttpUtility.HtmlEncode(linkText);
            }
            TagBuilder builder = new TagBuilder("a")
            {
                InnerHtml = innerHtml
            };
            builder.MergeAttributes<string, object>(htmlValues);
            builder.MergeAttribute("href", url);
            if (page != null && page.Route != null)
            {
                builder.MergeAttribute("target", page.Route.LinkTarget.ToString());
            }
            var html = new HtmlString(builder.ToString(TagRenderMode.Normal));

#if Page_Trace
            stopwatch.Stop();
            HttpContext.Current.Response.Write(string.Format("PageLink,{0}.</br>", stopwatch.Elapsed));
#endif
            return html;
        }
        #endregion

        #region ViewLink
        public virtual IHtmlString ViewLink(object linkText, string viewName)
        {
            return this.ViewLink(linkText, viewName, null);
        }
        public virtual IHtmlString ViewLink(object linkText, string viewName, object routeValues)
        {
            return this.ViewLink(linkText, viewName, routeValues, null);
        }
        public virtual IHtmlString ViewLink(object linkText, string viewName, object routeValues, object htmlAttributes)
        {
            var htmlValues = RouteValuesHelpers.GetRouteValues(htmlAttributes);

            string url = this.PageContext.FrontUrl.ViewUrl(viewName, routeValues).ToString();


            TagBuilder builder = new TagBuilder("a")
            {
                InnerHtml = linkText != null ? HttpUtility.HtmlEncode(linkText) : string.Empty
            };

            builder.MergeAttributes<string, object>(htmlValues);
            builder.MergeAttribute("href", url);
            var html = new HtmlString(builder.ToString(TagRenderMode.Normal));

            return html;
        }
        #endregion

        #region Pager
        internal class CmsPagerBuilder : PagerBuilder
        {
            private RouteValueDictionary routeValues;
            private IPagedList _pageList = null;
            private readonly PagerOptions _pagerOptions;
            private FrontHtmlHelper frontHtml;
            internal CmsPagerBuilder(FrontHtmlHelper html, IPagedList pageList,
                PagerOptions pagerOptions, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
                : base(html.Html, null, null, pageList, pagerOptions, null, routeValues, htmlAttributes)
            {
                frontHtml = html;
                this.routeValues = routeValues ?? new RouteValueDictionary();


                _pageList = pageList;
                this._pagerOptions = pagerOptions;
            }
            protected override string GenerateUrl(int pageIndex)
            {
                //return null if  page index larger than total page count or page index is current page index
                if (pageIndex > _pageList.TotalPageCount || pageIndex == _pageList.CurrentPageIndex)
                    return null;

                var routeValues = this.routeValues.Merge(frontHtml.PageContext.PageRequestContext.AllQueryString.ToRouteValues())
                    .Merge(_pagerOptions.PageIndexParameterName, pageIndex);
                return frontHtml.PageContext.Url.FrontUrl()
                    .GeneratePageUrl(frontHtml.PageContext.PageRequestContext.Page, routeValues).ToString();
            }
        }
        public virtual IHtmlString Pager(object model)
        {
            return Pager(model, null);
        }
        public virtual IHtmlString Pager(object model, PagerOptions options)
        {
            return Pager(model, null, options, null);
        }
        //Optional parameter does not support NVelocity
        public virtual IHtmlString Pager(object model, object routeValues, PagerOptions options, object htmlAttributes)
        {
            options = options ?? new PagerOptions();

            if ((model is DataRulePagedList))
            {
                options.PageIndexParameterName = ((DataRulePagedList)model).PageIndexParameterName;
            }

            var pagedList = (IPagedList)model;

            var builder = new CmsPagerBuilder
             (
                 this,
                 pagedList,
                 options,
                 RouteValuesHelpers.GetRouteValues(routeValues),
                 RouteValuesHelpers.GetRouteValues(htmlAttributes)
             );
            return new HtmlString(builder.RenderPager().ToString());
        }
        #endregion

        #region ResizeImage
        public virtual IHtmlString ResizeImage(string imagePath, int width, int height)
        {
            return ResizeImage(imagePath, width, height, null, null, null);
        }
        public virtual IHtmlString ResizeImage(string imagePath, int width, int height, string alt)
        {
            return ResizeImage(imagePath, width, height, null, null, alt);
        }
        public virtual IHtmlString ResizeImage(string imagePath, int width, int height, bool? preserverAspectRatio, int? quality, string alt)
        {
            return new HtmlString(string.Format("<img src=\"{0}\" alt=\"{1}\" />"
                , this.PageContext.FrontUrl.ResizeImageUrl(imagePath, width, height, preserverAspectRatio, quality)
                , alt));
        }
        #endregion

    }
}
