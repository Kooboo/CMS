#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Sites.ABTest
{
    public class SiteMatchedContext
    {
        public HttpContextBase HttpContext { get; set; }
        public Site RawSite { get; set; }
        public Site MatchedSite { get; set; }
        public ABSiteSetting SiteVisitRule { get; set; }
        public ABSiteRuleItem MatchedRuleItem { get; set; }
    }
    public interface ISiteVisitRuleMatchedObserver
    {
        void OnMatched(SiteMatchedContext matchedContext);
    }
}
