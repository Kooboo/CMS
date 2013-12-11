#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    /// <summary>
    /// upgrade steps:
    /// 1. Upgrade the module area file(Delete the older folder and unzip the new module files,copy the new files to bin folder) 
    /// 2. Run upgrading event
    /// 3. Run the upgradation files(1.0.0.0-2.0.0.0.sql,etc)
    /// </summary>
    public interface IModuleReinstaller
    {
        string Unzip(string moduleName, Stream moduleStream, string user);
        IEnumerable<ConflictedAssemblyReference> CheckConflictedAssemblyReferences(string moduleName);
        void CopyAssemblies(string moduleName, bool @overrideSystemVersion);
        void RunEvent(string moduleName, ControllerContext controllerContext);
    }
}
