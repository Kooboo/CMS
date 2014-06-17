#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataSource
{
    public class ExecutingDataSource
    {
        public ExecutingDataSource(DataSourceSetting dataSourceSetting, IEnumerable<DataSourceRelation> includedRelations)
        {
            this.DataSourceSetting = dataSourceSetting;
            this.IncludedRelations = includedRelations;
        }
        public DataSourceSetting DataSourceSetting { get; private set; }
        public IEnumerable<DataSourceRelation> IncludedRelations { get; private set; }
    }
}
