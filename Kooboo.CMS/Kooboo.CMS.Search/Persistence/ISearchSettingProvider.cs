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
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Search.Persistence
{
    public interface ISearchSettingProvider : IContentElementProvider<SearchSetting>
    {
    }
}
