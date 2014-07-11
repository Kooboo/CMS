#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(Kooboo.CMS.Content.Services.TextFolderManager), Order = 2)]
    public class SiteTextFolderManager : Kooboo.CMS.Content.Services.TextFolderManager
    {
        #region .ctor
        ViewManager _viewManager;
        PageManager _pageManager;
        public SiteTextFolderManager(ITextFolderProvider provider, ViewManager viewManager, PageManager pageManager)
            : base(provider)
        {
            this._viewManager = viewManager;
            this._pageManager = pageManager;
        }
        #endregion

        #region Relation
        public override IEnumerable<RelationModel> Relations(TextFolder textFolder)
        {
            var relations = base.Relations(textFolder);
            var site = Site.Current;
            if (site != null)
            {
                var viewDataRules = _viewManager.All(site, null).Select(it => it.AsActual()).Where(it => it.DataRules != null)
                .SelectMany(it => FindDataRuleRelations("View:" + it.Name, it.DataRules, textFolder));

                var pageDataRules = _pageManager.AllPagesFlattened(site).Select(it => it.AsActual()).Where(it => it.DataRules != null)
                .SelectMany(it => FindDataRuleRelations("Page:" + it.FullName, it.DataRules, textFolder));

                relations = relations.Concat(viewDataRules).Concat(pageDataRules);
            }
            return relations;
        }
        private IEnumerable<RelationModel> FindDataRuleRelations(string relationType, IEnumerable<DataRuleSetting> dataRules, TextFolder textFolder)
        {
            List<RelationModel> relations = new List<RelationModel>();
            foreach (var item in dataRules)
            {
                if (item.DataRule is FolderDataRule)
                {
                    var folderDataRule = (FolderDataRule)item.DataRule;
                    if (folderDataRule.FolderName.EqualsOrNullEmpty(textFolder.FullName, StringComparison.OrdinalIgnoreCase))
                    {
                        relations.Add(new RelationModel()
                        {
                            DisplayName = textFolder.FriendlyText,
                            ObjectUUID = textFolder.FullName,
                            RelationObject = textFolder,
                            RelationType = relationType + "." + item.DataName
                        });
                    }
                }

            }
            return relations;
        }
        #endregion
    }
}
