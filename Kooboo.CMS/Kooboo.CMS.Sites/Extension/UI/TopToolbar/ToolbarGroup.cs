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

namespace Kooboo.CMS.Sites.Extension.UI.TopToolbar
{
    public class ToolbarGroup
    {
        public string GroupName { get; set; }

        public string DisplayText { get; set; }

        public int Order { get; set; }

        public IDictionary<string, object> HtmlAttributes { get; set; }
    }

    public class ToolbarGroupEqualityComparer : IEqualityComparer<ToolbarGroup>
    {
        public bool Equals(ToolbarGroup x, ToolbarGroup y)
        {
            return x.GroupName.EqualsOrNullEmpty(y.GroupName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(ToolbarGroup obj)
        {
            return obj.GetHashCode();
        }
    }
}
