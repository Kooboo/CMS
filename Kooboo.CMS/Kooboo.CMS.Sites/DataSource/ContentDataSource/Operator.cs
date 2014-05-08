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
    public enum Operator
    {
        Equals = 0,
        NotEquals = 1,
        GreaterThan = 2,
        LessThan = 3,
        Contains = 4,
        StartsWith = 5,
        EndsWith = 6,
        Between = 7,
        IsNull = 8,
        NotNull = 9
    }
}
