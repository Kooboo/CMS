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

namespace Kooboo.CMS.Sites.DataSource.ContentDataSource
{
    public enum Logical
    {
        And = 0,
        Or = 1,
        ThenAnd = 2,
        ThenOr = 3,
    }
}
