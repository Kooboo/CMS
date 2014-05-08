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
        public DataSourceSettingManager(IDataSourceSettingProvider provider)
            : base(provider)
        {

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
    }
}
