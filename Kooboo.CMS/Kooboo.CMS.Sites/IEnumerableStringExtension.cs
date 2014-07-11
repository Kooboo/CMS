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

namespace Kooboo.CMS.Sites
{
    public static class IEnumerableStringExtension
    {
        public static IEnumerable<string> ExcludeSvn(this IEnumerable<string> dirs)
        {
            return dirs.Where(it => !(it.Contains(".svn", StringComparison.OrdinalIgnoreCase) || it.Contains("_svn", StringComparison.OrdinalIgnoreCase)));
        }
    }
}
