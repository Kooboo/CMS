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
    public class PageMatchedContext
    {
        public HttpContextBase HttpContext { get; set; }
        public Site Site { get; set; }
        public Page RawPage { get; set; }
        public Page MatchedPage { get; set; }
        public ABPageSetting ABPageSetting { get; set; }
        public ABPageRuleItem MatchedRuleItem { get; set; }
    }
    public interface IABPageMatchedObserver
    {
        void OnMatched(PageMatchedContext matchedContext);
    }
}
