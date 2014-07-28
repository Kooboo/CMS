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

namespace Kooboo.CMS.Common.Extension.TopBar
{
    public interface ITopBarPluginExecutor
    {
        IEnumerable<ITopBarPlugin> LoadTopBarPlugins(ControllerContext controllerContext);

        ActionResult Execute(ControllerContext controllerContext, string pluginName);
    }
}
