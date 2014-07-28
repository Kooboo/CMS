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
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Common.Extension.Tab
{
    public interface ITabPluginLoader
    {
        IEnumerable<LoadedTabPlugin> LoadTabPlugins(ControllerContext controllerContext);

        IEnumerable<LoadedTabPlugin> SubmitToTabPlugins(ControllerContext controllerContext);
    }
}
