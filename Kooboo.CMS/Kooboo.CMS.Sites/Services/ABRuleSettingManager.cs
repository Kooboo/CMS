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
    public class ABRuleSettingManager : ManagerBase<ABRuleSetting, IABRuleSettingProvider>
    {
        #region .ctor
        IABRuleSettingProvider _provider;
        IABSiteSettingProvider _abSiteSettingProvider;
        IABPageSettingProvider _abPageSettingProvider;
        public ABRuleSettingManager(IABRuleSettingProvider provider, IABSiteSettingProvider abSiteSettingProvider, IABPageSettingProvider abPageSettingProvider)
            : base(provider)
        {
            _provider = provider;
            _abSiteSettingProvider = abSiteSettingProvider;
            _abPageSettingProvider = abPageSettingProvider;
        }
        #endregion

        #region All
        public override IEnumerable<ABRuleSetting> All(Models.Site site, string filterName)
        {
            var list = _provider.All(site);
            if (!string.IsNullOrEmpty(filterName))
            {
                list = list.Where(it => it.Name.Contains(filterName, StringComparison.OrdinalIgnoreCase));
            }
            return list;
        }

        #endregion

        #region Get
        public override ABRuleSetting Get(Models.Site site, string name)
        {
            return _provider.Get(new ABRuleSetting() { Site = site, Name = name });
        }
        #endregion

        #region Update
        public override void Update(Models.Site site, ABRuleSetting @new, ABRuleSetting old)
        {
            @new.Site = site;
            @old.Site = site;
            _provider.Update(@new, old);
        }
        #endregion

        #region Import/export
        public virtual void Import(Site site, Stream zipStream, bool @override)
        {
            Provider.Import(site, zipStream, @override);
        }
        public virtual void Export(Site site, IEnumerable<ABRuleSetting> ruleSettings, System.IO.Stream outputStream)
        {
            Provider.Export(site, ruleSettings, outputStream);
        }      
        #endregion

        #region Relations
        public override IEnumerable<RelationModel> Relations(ABRuleSetting o)
        {
            List<RelationModel> list = new List<RelationModel>();
            if (o.Site == null)
            {
                list.AddRange(_abSiteSettingProvider.All().Select(it => it.AsActual())
                    .Where(it => it != null && it.RuleName.EqualsOrNullEmpty(o.Name, StringComparison.OrdinalIgnoreCase))
                    .Select(it => new Site(it.MainSite).AsActual())
                    .Select(it => new RelationModel()
                    {
                        DisplayName = it.FriendlyName,
                        ObjectUUID = it.FullName,
                        RelationType = "Site",
                        RelationObject = it
                    }));
            }
            else
            {
                list.AddRange(_abPageSettingProvider.All(o.Site).Select(it => it.AsActual())
                                   .Where(it => it != null && it.RuleName.EqualsOrNullEmpty(o.Name, StringComparison.OrdinalIgnoreCase))
                                   .Select(it => new Page(o.Site, it.MainPage).AsActual())
                                   .Select(it => new RelationModel()
                                   {
                                       DisplayName = it.FriendlyName,
                                       ObjectUUID = it.FullName,
                                       RelationType = "Page",
                                       RelationObject = it
                                   }));
            }
            return list;
        }

        #endregion

    }
}
