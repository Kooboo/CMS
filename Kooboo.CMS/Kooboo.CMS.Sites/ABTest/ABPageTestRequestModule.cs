#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.ABTest
{
    [Dependency(typeof(IPageRequestModule), Key = "ABTestPageRequestModule")]
    public class ABPageTestRequestModule : PageRequestModuleBase
    {
        ABPageSettingManager _pageVisitRuleManager;
        ABPageTestResultManager _abPageTestResultManager;
        public ABPageTestRequestModule(ABPageSettingManager pageVisitRuleManager, ABPageTestResultManager abPageTestResultManager)
        {
            this._pageVisitRuleManager = pageVisitRuleManager;
            this._abPageTestResultManager = abPageTestResultManager;
        }
        public override void OnResolvedPage(System.Web.Mvc.ControllerContext controllerContext, View.PageRequestContext pageRequestContext)
        {

            string pageVisitRuleName;
            string matchedPage;
            ABPageTestTrackingHelper.TryGetABTestPage(controllerContext.HttpContext.Request, pageRequestContext.Site, out pageVisitRuleName, out matchedPage);

            if (!string.IsNullOrEmpty(pageVisitRuleName) && !string.IsNullOrEmpty(matchedPage))
            {
                var pageVisitRule = _pageVisitRuleManager.Get(pageRequestContext.Site, pageVisitRuleName);
                if (pageVisitRule != null && !string.IsNullOrEmpty(pageVisitRule.ABTestGoalPage))
                {
                    if (pageVisitRule.ABTestGoalPage.EqualsOrNullEmpty(pageRequestContext.Page.FullName, StringComparison.OrdinalIgnoreCase))
                    {
                        _abPageTestResultManager.IncreaseHitTime(pageRequestContext.Site, pageVisitRuleName, matchedPage);
                    }
                }
            }
        }
    }
}
