#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.DataRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.DataSource
{
    public class DataSourceExecutor
    {
        public static string ModelName = "model";
        public static void Execute(ViewDataDictionary viewData, DataSourceContext dataSourceContext, IEnumerable<DataSourceSetting> dataSourceSettings)
        {
            foreach (var item in dataSourceSettings)
            {
                var data = item.DataSource.Execute(dataSourceContext);
                if (item.DataName.EqualsOrNullEmpty(ModelName, StringComparison.CurrentCultureIgnoreCase))
                {
                    viewData.Model = data;
                }
                viewData[item.DataName] = data;
            }
        }
    }
}
