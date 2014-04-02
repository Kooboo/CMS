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

namespace Kooboo.CMS.Sites.Extension.UI.Tabs
{
    public class TabInfo
    {
        public string Name { get; set; }
        public string DisplayText { get; set; }
        public string VirtualPath { get; set; }

        public int Order { get; set; }
    }
}
