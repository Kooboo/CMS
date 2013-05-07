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
    public class ABSiteSettingManager : IManager<ABSiteSetting>
    {
        #region .ctor
        IABSiteSettingProvider _provider;
        ISiteVisitRuleMatchedObserver[] _observers;
        public ABSiteSettingManager(IABSiteSettingProvider provider, ISiteVisitRuleMatchedObserver[] observers)
        {
            _provider = provider;
            _observers = observers;
        }
        #endregion

        #region All
        public virtual IEnumerable<ABSiteSetting> All(Models.Site site, string filterName)
        {
            return All(filterName);
        }
        public virtual IEnumerable<ABSiteSetting> All(string filterName)
        {
            var list = _provider.All();
            if (!string.IsNullOrEmpty(filterName))
            {
                list = list.Where(it => it.MainSite.Contains(filterName));
            }
            return list;
        }
        #endregion

        #region Get
        public virtual ABSiteSetting Get(string name)
        {
            return _provider.Get(new ABSiteSetting() { MainSite = name });
        }
        public virtual ABSiteSetting Get(Models.Site site, string name)
        {
            return Get(name);
        }
        #endregion

        #region Add

        public void Add(Site site, ABSiteSetting item)
        {
            _provider.Add(item);
        }
        #endregion

        #region Remove
        public void Remove(Site site, ABSiteSetting item)
        {
            _provider.Remove(item);
        }
        #endregion

        #region Update
        public virtual void Update(Models.Site site, ABSiteSetting @new, ABSiteSetting old)
        {
            _provider.Update(@new, old);
        }
        #endregion

        #region Import/export
        public virtual void Import(Site site, Stream zipStream, bool @override)
        {
            _provider.Import(zipStream, @override);
        }
        public virtual void Export(IEnumerable<ABSiteSetting> siteVisitRules, System.IO.Stream outputStream)
        {
            _provider.Export(siteVisitRules, outputStream);
        }
        public virtual void ExportAll(System.IO.Stream outputStream)
        {
            _provider.Export(All(""), outputStream);
        }
        #endregion

        #region Match site
        public virtual Site MatchRule(Site site, HttpContextBase httpContext)
        {
            var matchedSite = site;
            var ruleName = site.FullName;

            var visitRule = Get(ruleName);
            if (visitRule != null)
            {
                ABSiteRuleItem matchedRuleItem = null;
                var ruleSetting = new ABRuleSetting(null, visitRule.RuleName).AsActual();
                if (ruleSetting != null && ruleSetting.RuleItems != null)
                {
                    foreach (var item in visitRule.Items)
                    {
                        var ruleItem = ruleSetting.RuleItems.Where(it => it.Name.EqualsOrNullEmpty(item.RuleItemName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (ruleItem.IsMatch(httpContext.Request))
                        {
                            if (!string.IsNullOrEmpty(item.SiteName))
                            {
                                var ruleSite = new Site(item.SiteName).AsActual();
                                if (ruleSite != null)
                                {
                                    matchedSite = ruleSite;
                                    matchedRuleItem = item;
                                    break;
                                }
                            }
                        }
                    }

                    OnRuleMatch(new SiteMatchedContext() { HttpContext = httpContext, RawSite = site, MatchedSite = matchedSite, SiteVisitRule = visitRule, MatchedRuleItem = matchedRuleItem });
                }

            }
            return matchedSite;
        }
        protected virtual void OnRuleMatch(SiteMatchedContext context)
        {
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
