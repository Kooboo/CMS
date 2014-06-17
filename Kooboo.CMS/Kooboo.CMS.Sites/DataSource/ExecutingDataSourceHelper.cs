#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataSource
{
    public static class ExecutingDataSourceHelper
    {
        public static IEnumerable<ExecutingDataSource> ToExecutingDataSources(Site site, List<DataSourceSelection> selections)
        {
            List<ExecutingDataSource> executingDataSources = new List<ExecutingDataSource>();
            foreach (var item in selections)
            {
                var dataSourceSetting = new DataSourceSetting(site, item.MainDataSourceName).AsActual();
                if (dataSourceSetting != null)
                {
                    IEnumerable<DataSourceRelation> includedRelations = new DataSourceRelation[0];
                    if (item.RelatedDataSources != null)
                    {
                        includedRelations = (dataSourceSetting.Relations ?? new List<DataSourceRelation>()).Where(it => item.RelatedDataSources.Contains(it.TargetDataSourceName)).ToArray();
                    }
                    executingDataSources.Add(new ExecutingDataSource(dataSourceSetting, includedRelations));
                }
            }
            return executingDataSources;
        }
    }
}
