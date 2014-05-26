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
    public interface IDataSource
    {
        object Execute(DataSourceContext dataSourceContext);

        IEnumerable<string> GetParameters();

        IDictionary<string, object> GetDefinitions(DataSourceContext dataSourceContext);
    }
}
