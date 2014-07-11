#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Sites.ABTest
{
    public static class ABPageTestTrackingHelper
    {

        public const string Track_CookieName = "A/B_TEST_TRACKING";


        #region ComposeTrackingToken
        public static string ComposeTrackingToken(PageMatchedContext matchedContext)
        {
            var trackingValue = string.Format("{0}||{1}", matchedContext.ABPageSetting.MainPage, matchedContext.MatchedPage.FullName);
            var trackingToken = SecurityHelper.Encrypt(matchedContext.Site, trackingValue);
            return trackingToken;
        }
        #endregion

        #region ParseTrackingToken
        public static void ParseTrackingToken(Site site, string trackingToken, out string pageVisitRuleName, out string matchedPage)
        {
            pageVisitRuleName = null;
            matchedPage = null;

            if (!string.IsNullOrEmpty(trackingToken))
            {
                try
                {
                    string trackingValue = SecurityHelper.Decrypt(site, trackingToken);

                    string[] values = trackingValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

                    if (values.Length > 0)
                    {
                        pageVisitRuleName = values[0];
                        matchedPage = values[1];
                    }
                }
                catch (Exception e)
                {
                    Kooboo.Common.Logging.Logger.Error(e.Message, e);
                }
            }
        }
        #endregion

        #region TryGetABTestPage
        public static void TryGetABTestPage(this HttpRequestBase httpRequest, Site site, out string pageVisitRuleName, out string matchedPage)
        {
            pageVisitRuleName = null;
            matchedPage = null;
            var cookie = httpRequest.Cookies[Track_CookieName];
            if (cookie != null)
            {
                ABPageTestTrackingHelper.ParseTrackingToken(site, cookie.Value, out pageVisitRuleName, out matchedPage);
            }
        }
        #endregion

        #region SetABTestPageCookie
        public static void SetABTestPageCookie(PageMatchedContext matchedContext)
        {
            var trackingToken = ABPageTestTrackingHelper.ComposeTrackingToken(matchedContext);

            matchedContext.HttpContext.Response.SetCookie(new System.Web.HttpCookie(Track_CookieName, trackingToken));
        }
        #endregion
    }
}
