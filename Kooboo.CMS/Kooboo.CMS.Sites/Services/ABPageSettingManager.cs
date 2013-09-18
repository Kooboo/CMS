#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.ABTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Services
{
    public class ABPageSettingManager : ManagerBase<ABPageSetting, IABPageSettingProvider>
    {
        #region .ctor
        IABPageSettingProvider _provider;
        IABPageMatchedObserver[] _observers;
        ABPageTestResultManager _abPageTestResultManager;
        public ABPageSettingManager(IABPageSettingProvider provider, IABPageMatchedObserver[] observers, ABPageTestResultManager abPageTestResultManager)
            : base(provider)
        {
            _provider = provider;
            _observers = observers;
            _abPageTestResultManager = abPageTestResultManager;
        }
        #endregion


        #region All
        public override IEnumerable<ABPageSetting> All(Site site, string filterName)
        {
            var list = _provider.All(site).Select(it => it.AsActual());
            if (!string.IsNullOrEmpty(filterName))
            {
                list = list.Where(it => it.MainPage.Contains(filterName));
            }
            return list;
        }
        #endregion

        #region Get
        public override ABPageSetting Get(Site site, string name)
        {
            return _provider.Get(new ABPageSetting() { Site = site, MainPage = name });
        }
        #endregion

        #region Update
        public override void Update(Site site, ABPageSetting @new, ABPageSetting old)
        {
            @new.Site = site;
            old.Site = site;
            _provider.Update(@new, old);
        }

        #endregion

        #region Import/export
        public virtual void Import(Site site, Stream zipStream, bool @override)
        {
            _provider.Import(site, zipStream, @override);
        }
        public virtual void Export(IEnumerable<ABPageSetting> pageVisitRules, System.IO.Stream outputStream)
        {
            _provider.Export(pageVisitRules, outputStream);
        }
        public virtual void ExportAll(Site site, System.IO.Stream outputStream)
        {
            _provider.Export(All(site, ""), outputStream);
        }
        #endregion

        #region Match page
        public virtual Page MatchRule(Site site, Page page, HttpContextBase httpContext)
        {
            var matchedPage = page;
            var ruleName = page.FullName;

            //string abpageRuleInCookie = null;
            //string abPageInCookie = null;

            //ABPageTestTrackingHelper.TryGetABTestPage(httpContext.Request, site, out abpageRuleInCookie, out abPageInCookie);
            //var matchedRuleInCookie = false;
            //if (!string.IsNullOrEmpty(abpageRuleInCookie))
            //{
            //    matchedRuleInCookie = abpageRuleInCookie.EqualsOrNullEmpty(ruleName, StringComparison.OrdinalIgnoreCase);
            //}

            var visitRule = Get(site, ruleName);
            if (visitRule != null)
            {
                ABPageRuleItem matchedRuleItem = null;
                var ruleSetting = new ABRuleSetting(site, visitRule.RuleName).AsActual();
                if (ruleSetting != null && ruleSetting.RuleItems != null)
                {
                    foreach (var item in visitRule.Items)
                    {
                        var isMatched = false;
                        //if (matchedRuleInCookie && abPageInCookie.EqualsOrNullEmpty(item.PageName, StringComparison.OrdinalIgnoreCase))
                        //{
                        //    isMatched = true;
                        //}
                        //else
                        //{
                        var ruleItem = ruleSetting.RuleItems.Where(it => it.Name.EqualsOrNullEmpty(item.RuleItemName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        isMatched = (ruleItem.IsMatch(httpContext.Request));
                        //}

                        if (isMatched && !string.IsNullOrEmpty(item.PageName))
                        {
                            var rulePage = new Page(site, item.PageName).LastVersion().AsActual();
                            if (rulePage != null)
                            {
                                matchedPage = rulePage;
                                matchedRuleItem = item;
                                break;
                            }
                        }

                    }

                    OnRuleMatch(new PageMatchedContext() { HttpContext = httpContext, Site = site, RawPage = page, MatchedPage = matchedPage, ABPageSetting = visitRule, MatchedRuleItem = matchedRuleItem });
                }
            }
            return matchedPage;
        }

        protected virtual void OnRuleMatch(PageMatchedContext context)
        {
            if (!string.IsNullOrEmpty(context.ABPageSetting.ABTestGoalPage))
            {
                _abPageTestResultManager.IncreaseShowTime(context.Site, context.ABPageSetting.UUID, context.MatchedPage.FullName);
            }
            if (this._observers != null)
            {
                foreach (var item in this._observers)
                {
                    item.OnMatched(context);
                }
            }
        }
        #endregion
    }
}
