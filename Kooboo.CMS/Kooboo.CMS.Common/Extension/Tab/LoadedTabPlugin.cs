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
using System.Threading.Tasks;

namespace Kooboo.CMS.Common.Extension.Tab
{
    public class LoadedTabPlugin
    {
        public LoadedTabPlugin(ITabPlugin tabPlugin,TabContext tabLoadContext)
        {
            this.TabPlugin = tabPlugin;
            this.Context = tabLoadContext;
        }
        public ITabPlugin TabPlugin { get; private set; }
        public TabContext Context { get; private set; }
    }
}
