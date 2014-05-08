#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface IDataSourceSettingProvider : ISiteElementProvider<DataSourceSetting>, ILocalizableProvider<DataSourceSetting>
    {
    }
}
