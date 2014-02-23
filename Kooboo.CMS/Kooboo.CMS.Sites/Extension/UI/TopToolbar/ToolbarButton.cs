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
    public class ToolbarButton
    {
        public string CommandText { get; set; }

        public string IconClass { get; set; }

        public MvcRoute CommandTarget { get; set; }

        public string GroupName { get; set; }

        public IDictionary<string, object> HtmlAttributes { get; set; }

        public int Order { get; set; }
    }
}
