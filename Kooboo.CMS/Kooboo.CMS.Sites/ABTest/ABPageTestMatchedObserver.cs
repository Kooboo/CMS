#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.ABTest
{
    [Dependency(typeof(IPageVisitRuleMatchedObserver), Key = "GoingPageObserver")]
    public class ABPageTestMatchedObserver : IPageVisitRuleMatchedObserver
    {
        public const string Track_CookieName = "A/B_TEST_TRACKING";
        public void OnMatched(PageMatchedContext matchedContext)
        {
            var trackingToken = ABPageTestTrackingHelper.ComposeTrackingToken(matchedContext);

            matchedContext.HttpContext.Response.SetCookie(new System.Web.HttpCookie(Track_CookieName, trackingToken));
        }
    }
}
