using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Web;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Sites.DataRule;

namespace Kooboo.CMS.Sites.View
{
    public class ModuleUrlContext
    {
        public ModuleUrlContext(PageRequestContext pageRequextContext, string moduleUrl)
        {
            this.PageRequestContext = pageRequextContext;
            this.ModuleUrl = moduleUrl;
            ParseModuleRouteValues(moduleUrl);
        }

        public static string ModuleUrlSplitterFormat = "({0})";
        readonly static Regex ModuleUrlSplitterRegex = new Regex("\\([^/]*\\)", RegexOptions.Compiled);

        public PageRequestContext PageRequestContext { get; private set; }
        public RouteValueDictionary ModuleRouteValues { get; private set; }

        public string ModuleUrl { get; private set; }

        private void ParseModuleRouteValues(string moduleUrl)
        {
            ModuleRouteValues = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(moduleUrl))
            {
                moduleUrl = System.Web.HttpUtility.UrlDecode(moduleUrl);
                MatchCollection matches = ModuleUrlSplitterRegex.Matches(moduleUrl);
                for (int i = 0; i < matches.Count; i++)
                {
                    var current = matches[i];
                    int startIndex = current.Index + current.Value.Length;
                    string url = string.Empty;
                    if (i + 1 < matches.Count)
                    {
                        var next = matches[i + 1];
                        url = moduleUrl.Substring(startIndex, next.Index - startIndex - 1);
                    }
                    else
                    {
                        url = moduleUrl.Substring(startIndex);
                    }

                    ModuleRouteValues.Add(current.Value, url);
                }
            }
        }

        public string GetModuleUrl(string modulePositionId)
        {
            string moduleUrlSplitter = string.Format(ModuleUrlSplitterFormat, modulePositionId);
            if (ModuleRouteValues[moduleUrlSplitter] != null)
            {
                //Decode the module url before using. Because the other module urls will be used when next page url generating.
                return "~" + ModuleUrlHelper.Decode(ModuleRouteValues[moduleUrlSplitter].ToString());
            }
            return string.Empty;
        }

        public string GetModulePositionIdForUrl(string moduleName, string currentModulePositionId, RouteValueDictionary routeValues)
        {
            string controller = routeValues["controller"].ToString();
            string action = routeValues["action"].ToString();
            var modulePosition = this.PageRequestContext.Page.PagePositions.OfType<ModulePosition>()
                .Where(it => it.ModuleName.EqualsOrNullEmpty(moduleName, StringComparison.OrdinalIgnoreCase))
                .Where(it => it.Entry != null)
                .Where(it => it.Entry.Controller.EqualsOrNullEmpty(controller, StringComparison.OrdinalIgnoreCase) && it.Entry.Action.EqualsOrNullEmpty(action, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
            if (modulePosition == null)
            {
                return currentModulePositionId;
            }
            else
            {
                return modulePosition.PagePositionId;
            }
        }

        public RouteValueDictionary GetRouteValuesWithModuleUrl(string modulePositionId, string moduleUrl, bool exclusive)
        {
            RouteValueDictionary values = new RouteValueDictionary(PageRequestContext.RouteValues);
            RouteValueDictionary moduleRouteValues = null;
            if (exclusive)
            {
                moduleRouteValues = new RouteValueDictionary();
            }
            else
            {
                moduleRouteValues = new RouteValueDictionary(ModuleRouteValues);
            }
            var moduleUrlSpliter = string.Format(ModuleUrlSplitterFormat, modulePositionId);
            if (!string.IsNullOrEmpty(moduleUrl) && moduleUrl != "/")
            {
                moduleRouteValues[moduleUrlSpliter] = moduleUrl;
            }
            else
            {
                if (moduleRouteValues.ContainsKey(moduleUrlSpliter))
                {
                    moduleRouteValues.Remove(moduleUrlSpliter);
                }
            }

            StringBuilder moduleUrlBuilder = new StringBuilder();
            foreach (var item in moduleRouteValues)
            {
                //item.Value start with '/'
                moduleUrlBuilder.AppendFormat("{0}/{1}/", item.Key, item.Value.ToString().TrimStart('/'));
            }

            values[PageRequestContext.ModuleUrlSegment] = moduleUrlBuilder.ToString().TrimEnd('/');

            return values;
        }
    }

    /// <summary>
    /// 是用于存放一些只读的CMS上下文信息。。
    /// 比如： Site,Page,解析后的QueryString和RouteValues,还有一些ModuleUrl的处理。
    /// 这个类的作用有点类似于MVC的RequestContext
    /// </summary>
    public class PageRequestContext
    {
        private class PageHttpContenxt : HttpContextWrapper
        {
            private HttpRequestBase requestBase;
            public PageHttpContenxt(HttpContext httpContext, HttpRequestBase requestBase)
                : base(httpContext)
            {
                this.requestBase = requestBase;
            }
            public override HttpRequestBase Request
            {
                get
                {
                    return this.requestBase;
                }
            }
        }
        private class PageHttpRequest : HttpRequestWrapper
        {
            private string appRelativeCurrentExecutionFilePath;
            private string pathInfo;
            public PageHttpRequest(HttpRequest httpRequest, string appRelativeCurrentExecutionFilePath, string pathInfo)
                : base(httpRequest)
            {
                this.appRelativeCurrentExecutionFilePath = appRelativeCurrentExecutionFilePath;
                this.pathInfo = pathInfo;
            }
            public override string AppRelativeCurrentExecutionFilePath
            {
                get
                {
                    return this.appRelativeCurrentExecutionFilePath;
                }

            }
            public override string PathInfo
            {
                get
                {
                    return this.pathInfo;
                }
            }
        }

        public static string ModuleUrlSegment = "ModuleUrl";

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontRequestContext"/> class.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="page">The page.</param>
        /// <param name="requestChannel">The request channel.</param>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="pageRequestUrl">The page request url with out page virtual path.</param>
        public PageRequestContext(ControllerContext controllerContext, Site site, Page page, FrontRequestChannel requestChannel, string pageRequestUrl)
        {
            RouteValues = new RouteValueDictionary();

            this.ControllerContext = controllerContext;

            var httpContext = HttpContext.Current;

            this.Site = site.AsActual();
            this.Page = page.AsActual();
            this.RequestChannel = requestChannel;

            this.AllQueryString = new NameValueCollection(httpContext.Request.QueryString);


            HttpContextBase pageContext = new PageHttpContenxt(httpContext, new PageHttpRequest(httpContext.Request, "~/" + pageRequestUrl, ""));
            var routeData = page.Route.ToMvcRoute().GetRouteData(pageContext);

            if (routeData != null)
            {
                RouteValues = routeData.Values;
                //Combine page parameters to [QueryString].
                foreach (var item in RouteValues)
                {
                    if (item.Value != null)
                    {
                        AllQueryString[item.Key] = item.Value.ToString();
                    }
                }
            }

            var moduleUrl = AllQueryString[ModuleUrlSegment];
            ModuleUrlContext = new ModuleUrlContext(this, moduleUrl);
        }

        public Site Site { get; private set; }
        public Page Page { get; private set; }

        public ControllerContext ControllerContext { get; private set; }

        public FrontRequestChannel RequestChannel { get; private set; }

        public RouteValueDictionary RouteValues { get; private set; }

        public NameValueCollection AllQueryString { get; private set; }

        public ModuleUrlContext ModuleUrlContext { get; private set; }

        public Kooboo.CMS.Sites.DataRule.ValueProviderCollection GetValueProvider()
        {
            return new Kooboo.CMS.Sites.DataRule.ValueProviderCollection(new List<Kooboo.CMS.Sites.DataRule.IValueProvider>()
            {
                new Kooboo.CMS.Sites.DataRule.FormValueProvider(this.ControllerContext),
                new RouteValueProvider(this),
                new Kooboo.CMS.Sites.DataRule.QueryStringValueProvider(this.ControllerContext),
                new SessionValueProvider(this.ControllerContext)
            });
        }

    }
}
