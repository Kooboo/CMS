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
            var usedInViews = _viewProvider.All(dataSourceSetting.Site)
                .Select(it => it.AsActual())
                .Where(it => it.DataSources != null && it.DataSources.Any(ds => ds.MainDataSourceName.EqualsOrNullEmpty(dataSourceSetting.DataName, StringComparison.OrdinalIgnoreCase)))
                .Select(it => new RelationModel()
            {
                DisplayName = it.Name,
                ObjectUUID = it.Name,
                RelationObject = it,
                RelationType = "View"
            });

            var usedInDataSources = All(dataSourceSetting.Site, null)
                .Select(it => it.AsActual())
                .Where(it => it.Relations != null && !it.DataName.EqualsOrNullEmpty(dataSourceSetting.DataName, StringComparison.OrdinalIgnoreCase) && it.Relations.Any(r => r.TargetDataSourceName.EqualsOrNullEmpty(dataSourceSetting.DataName, StringComparison.OrdinalIgnoreCase)))
                .Select(it => new RelationModel()
                {
                    DisplayName = it.DataName,
                    ObjectUUID = it.DataName,
                    RelationObject = it,
                    RelationType = "Data source"
                });

            return usedInDataSources.Concat(usedInViews);
        }
        #endregion

        #region GetDefinitions
        public virtual IDictionary<string, object> GetDefinitions(DataSourceSetting setting, DataSourceContext dataSourceContext)
        {
            var definitions = setting.DataSource.GetDefinitions(dataSourceContext);

            if (setting.Relations != null)
            {
                foreach (var item in setting.Relations)
                {
                    var name = item.TargetDataSourceName;
                    var relateDataSource = new DataSourceSetting(setting.Site, name).AsActual();
                    if (relateDataSource != null)
                    {
                        definitions[name] = GetDefinitions(relateDataSource, dataSourceContext);
                    }
                }
            }
            return definitions;
        }
        #endregion
    }
}
