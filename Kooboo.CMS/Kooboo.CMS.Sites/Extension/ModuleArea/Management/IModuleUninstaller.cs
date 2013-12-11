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

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{

    /// <summary>
    /// Uninstall module steps:
    /// 1. Run uninstall event
    /// 2. Remove the assembly files from Bin folder(Remove the reference record from the sharing assemblies).
    /// 3. Delete the module area folder
    /// 4. Remove the module versions
    /// </summary>
    public interface IModuleUninstaller
    {
        void RunEvent(string moduleName, ControllerContext controllerContext);
        void RemoveAssemblies(string moduleName);
        void DeleteModuleArea(string moduleName);

        void RemoveVersions(string moduleName);

    }
}
