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
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management.Events
{
    public interface IModuleUninstallingEvents
    {
        /// <summary>
        /// Called when [uninstalling].
        /// Will only enabled when the module have install template(The InstallingTemplate property in ModuleInfo)
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        /// <param name="controllerContext">The controller context.</param>
        void OnUninstalling(ModuleContext moduleContext, ControllerContext controllerContext);
    }
}
