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
using Kooboo.Globalization;
using System.IO;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Extension.ModuleArea.Management.Events;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleReinstaller))]
    public class ModuleReinstaller : IModuleReinstaller
    {
        IModuleInstaller _moduleInstaller;
        IModuleUninstaller _moduleUninstaller;
        IModuleVersioning _moduleVersioning;
        public ModuleReinstaller(IModuleInstaller moduleInstaller, IModuleUninstaller moduleUninstaller, IModuleVersioning moduleVersioning)
        {
            this._moduleInstaller = moduleInstaller;
            this._moduleUninstaller = moduleUninstaller;
            this._moduleVersioning = moduleVersioning;
        }
        public string Unzip(string moduleName, System.IO.Stream moduleStream, string user)
        {
            //remove the old module
            string error = "";
            using (ModuleStreamEntry moduleEntry = new ModuleStreamEntry(moduleName, moduleStream))
            {
                if (!moduleEntry.IsValid())
                {
                    error = "The module is invalid.".Localize();
                    return error;
                }

                var moduleInfo = moduleEntry.ModuleInfo;
                if (moduleInfo == null)
                {
                    error = "The module.config file is invalid.".Localize();
                    return error;
                }
                if (!moduleName.EqualsOrNullEmpty(moduleEntry.ModuleName, StringComparison.OrdinalIgnoreCase))
                {
                    error = "The uploading module is a different module, reinstallation aborted.".Localize();
                    return error;
                }
                moduleName = moduleEntry.ModuleName;

                ModulePath modulePath = new ModulePath(moduleEntry.ModuleName);
                var modulePhysicalPath = modulePath.PhysicalPath;

                if (!Directory.Exists(modulePath.PhysicalPath))
                {
                    error = "The module does not exist.".Localize();
                    return error;
                }
                //save the module version
                var currentModuleInfo = ModuleInfo.Get(moduleName);
                var installationFile = this._moduleVersioning.SaveInstallationFile(moduleEntry);
                this._moduleVersioning.LogInstallation(moduleName, new InstallationContext(moduleInfo.ModuleName, currentModuleInfo.Version, moduleInfo.Version, DateTime.UtcNow) { User = user, InstallationFileName = installationFile });

                //unstall the current version
                this._moduleUninstaller.RemoveAssemblies(moduleName);
                this._moduleUninstaller.DeleteModuleArea(moduleName);

                moduleEntry.Extract(modulePhysicalPath);
            }
            return error;
        }

        #region CheckConflictedAssemblyReferences
        public IEnumerable<Extension.Management.ConflictedAssemblyReference> CheckConflictedAssemblyReferences(string moduleName)
        {
            return _moduleInstaller.CheckConflictedAssemblyReferences(moduleName);
        }
        #endregion

        #region CopyAssemblies
        public void CopyAssemblies(string moduleName, bool overrideSystemVersion)
        {
            _moduleInstaller.CopyAssemblies(moduleName, overrideSystemVersion);
        }
        #endregion

        #region RunEvent
        public void RunEvent(string moduleName, System.Web.Mvc.ControllerContext controllerContext)
        {
            var moduleEvents = Kooboo.CMS.Common.Runtime.EngineContext.Current.TryResolve<IModuleReinstallingEvents>(moduleName);

            if (moduleEvents != null)
            {
                var installationContext = _moduleVersioning.GetLastestInstallation(moduleName);
                moduleEvents.OnReinstalling(new ModuleContext(moduleName), controllerContext, installationContext);
            }
        }
        #endregion
    }
}
