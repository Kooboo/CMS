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
    public class VisitRuleSettingManager : ManagerBase<ABRuleSetting, IABRuleSettingProvider>
    {
        #region .ctor
        IABRuleSettingProvider _provider;
        public VisitRuleSettingManager(IABRuleSettingProvider provider)
            : base(provider)
        {
            _provider = provider;
        }
        #endregion

        #region All
        public override IEnumerable<ABRuleSetting> All(Models.Site site, string filterName)
        {
            var list = _provider.All(site);
            if (!string.IsNullOrEmpty(filterName))
            {
                list = list.Where(it => it.Name.Contains(filterName));
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
        public virtual void Export(IEnumerable<ABRuleSetting> ruleSettings, System.IO.Stream outputStream)
        {
            Provider.Export(ruleSettings, outputStream);
        }
        public virtual void ExportAll(Site site, System.IO.Stream outputStream)
        {
            Provider.Export(All(site, ""), outputStream);
        }
        #endregion

      

    }
}
