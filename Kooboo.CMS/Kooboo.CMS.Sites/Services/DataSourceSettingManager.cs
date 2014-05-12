#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Services
{
    public class DataSourceSettingManager : ManagerBase<DataSourceSetting, IDataSourceSettingProvider>
    {
        #region .ctor
        IViewProvider _viewProvider;
        public DataSourceSettingManager(IDataSourceSettingProvider provider, IViewProvider viewProvider)
            : base(provider)
        {
            this._viewProvider = viewProvider;
        }
        #endregion

        #region All/Get/Update
        public override IEnumerable<DataSourceSetting> All(Models.Site site, string filterName)
        {
            var result = Provider.All(site).Select(it => it.AsActual());
            if (!string.IsNullOrEmpty(filterName))
            {
                result = result.Where(it => it.UUID.Contains(filterName, StringComparison.CurrentCultureIgnoreCase));
            }
            return result;
        }

        public override DataSourceSetting Get(Models.Site site, string uuid)
        {
            return Provider.Get(new DataSourceSetting(site, uuid));
        }

        public override void Update(Models.Site site, DataSourceSetting @new, DataSourceSetting old)
        {
            @new.Site = site;
            @old.Site = site;
            Provider.Update(@new, @old);
        }
        #endregion

        #region Relation
        public override IEnumerable<RelationModel> Relations(DataSourceSetting dataSourceSetting)
        {
            return _viewProvider.All().Select(it => it.AsActual()).Where(it => it.DataSources != null && it.DataSources.Contains(dataSourceSetting.DataName, StringComparer.OrdinalIgnoreCase)).Select(it => new RelationModel()
            {
                DisplayName = it.Name,
                ObjectUUID = it.Name,
                RelationObject = it,
                RelationType = "View"
            });
        }
        #endregion
    }
}
