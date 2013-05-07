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
using System.Web;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Globalization;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Sites.Web
{
    public class FrontHttpRequestWrapper : System.Web.HttpRequestWrapper
    {
        #region .ctor
        private HttpRequest _request;
        public FrontHttpRequestWrapper(HttpRequest httpRequest)
            : base(httpRequest)
        {
            //applicationPath = base.ApplicationPath;

            //HttpContext.Current.Items["ApplicationPath"] = applicationPath;

            this._request = httpRequest;

            // "~/site1/index"
            appRelativeCurrentExecutionFilePath = base.AppRelativeCurrentExecutionFilePath;

        }
        #endregion

        #region override
        string appRelativeCurrentExecutionFilePath;
        public override string AppRelativeCurrentExecutionFilePath
        {
            get
            {
                return appRelativeCurrentExecutionFilePath;
            }
        }
        #endregion

        #region Properties

        private Site _site;
        public Site Site
        {
            get
            {
                return _site;
            }
            private set
            {
                _site = value;
                Site.Current = value;
            }
        }

        public Site RawSite { get; set; }

        public FrontRequestChannel RequestChannel
        {
            get;
            set;
        }
        private string _requestUrl = "";
        public string RequestUrl
        {
            get { return _requestUrl; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var queryQueryIndex = value.IndexOf("?");
                    if (queryQueryIndex > -1)
                    {
                        _requestUrl = value.Substring(0, queryQueryIndex);
                    }
                    else
                    {
                        _requestUrl = value;
                    }
                }
            }
        }
        string sitePath;
        public virtual string SitePath
        {
            get
            {
                return sitePath;
            }
        }
        #endregion

        #region ResolveSite

        private static bool IgnoreResolveSite(string appRelativeCurrentExecutionFilePath)
        {
            return appRelativeCurrentExecutionFilePath.ToLower().Contains("/cms_data/");
        }
        private static string GetRawHostWithoutPort(HttpRequest request)
        {
            return request.Url.Host;
        }
        internal void ResolveSite()
        {
            if (IgnoreResolveSite(appRelativeCurrentExecutionFilePath))
            {
                return;
            }
            if (!string.IsNullOrEmpty(this.PathInfo))
            {
                appRelativeCurrentExecutionFilePath = appRelativeCurrentExecutionFilePath.TrimEnd('/') + "/" + PathInfo;
            }
            //trim "~/"
            var trimedPath = appRelativeCurrentExecutionFilePath.Substring(2);

            //if the RawUrl is not start with the debug site url.
            //http://www.site1.com/index
            //http://www.site1.com/en/index
            var siteProvider = Persistence.Providers.SiteProvider;
            if (!trimedPath.StartsWith(SiteHelper.PREFIX_FRONT_DEBUG_URL, StringComparison.InvariantCultureIgnoreCase))
            {
                #region RequestByHostName
                var host = GetRawHostWithoutPort(_request);
                RawSite = siteProvider.GetSiteByHostNameNPath(host, trimedPath);
                if (RawSite != null)
                {

                    sitePath = RawSite.SitePath;
                    var sitePathLength = 0;
                    if (!string.IsNullOrEmpty(sitePath))
                    {
                        sitePathLength = sitePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Length;
                        RequestChannel = FrontRequestChannel.HostNPath;
                    }
                    else
                    {
                        RequestChannel = FrontRequestChannel.Host;
                    }
                    var path = trimedPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    RequestUrl = UrlUtility.Combine(path.Skip(sitePathLength).ToArray());
                    appRelativeCurrentExecutionFilePath = "~/" + RequestUrl;
                }

                #endregion
            }
            else
            {
                #region dev~
                //dev~site1/index
                var path = trimedPath.Split('/');
                var sitePaths = SiteHelper.SplitFullName(path[0].Substring(SiteHelper.PREFIX_FRONT_DEBUG_URL.Count()));

                RawSite = siteProvider.Get(Site.ParseSiteFromRelativePath(sitePaths));
                if (RawSite != null)
                {
                    RequestChannel = FrontRequestChannel.Debug;
                }

                RequestUrl = Kooboo.Web.Url.UrlUtility.Combine(path.Skip(1).ToArray());
                appRelativeCurrentExecutionFilePath = "~/" + RequestUrl;
                #endregion
            }

            if (RawSite != null)
            {
                Site = MatchSiteByVisitRule(RawSite);

                //set current site repository
                Kooboo.CMS.Content.Models.Repository.Current = Site.GetRepository();

                if (!string.IsNullOrEmpty(Site.Culture))
                {
                    var culture = CultureInfoHelper.CreateCultureInfo(Site.Culture);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }

            //decode the request url. for chinese character
            this.RequestUrl = HttpUtility.UrlDecode(this.RequestUrl);
        }

        protected virtual Site MatchSiteByVisitRule(Site site)
        {
            return Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Sites.Services.SiteVisitRuleManager>().MatchRule(site, new HttpContextWrapper(HttpContext.Current));
        }
        #endregion
    }
}
