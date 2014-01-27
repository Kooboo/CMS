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
        #region .ctor
        ModuleInstaller _moduleInstaller;
        ModuleUninstaller _moduleUninstaller;
        IInstallationFileManager _installationFileManager;
        public ModuleReinstaller(ModuleInstaller moduleInstaller, ModuleUninstaller moduleUninstaller, IInstallationFileManager moduleVersioning)
        {
            this._moduleInstaller = moduleInstaller;
            this._moduleUninstaller = moduleUninstaller;
            this._installationFileManager = moduleVersioning;
        }
        #endregion

        #region RunEvent
        public void RunEvent(string moduleName, System.Web.Mvc.ControllerContext controllerContext)
        {
            var moduleEvent = Kooboo.CMS.Common.Runtime.EngineContext.Current.TryResolve<IModuleReinstallingEvents>(moduleName);
            if (moduleEvent == null)
            {
                moduleEvent = Kooboo.CMS.Common.Runtime.EngineContext.Current.TryResolve<IModuleEvents>(moduleName);
            }
            if (moduleEvent != null)
            {
                var installationContext = _installationFileManager.GetLastestInstallation(moduleName);
                moduleEvent.OnReinstalling(new ModuleContext(moduleName), controllerContext, installationContext);
            }
        }
        #endregion

        public UploadModuleResult Upload(string moduleName, Stream moduleStream, string user)
        {
            var result = _moduleInstaller.Upload(moduleName, moduleStream, user);
            result.SourceModuleInfo = ModuleInfo.Get(result.ModuleName);
            return result;
        }
        public void CopyAssemblies(string moduleName, bool @overrideFiles)
        {
            ModuleInfo sourceModuleInfo = ModuleInfo.Get(moduleName);

            _moduleUninstaller.RemoveAssemblies(moduleName);
            _moduleUninstaller.RemoveModuleArea(moduleName);
            _moduleInstaller.CopyFiles(moduleName, overrideFiles);
        }

        public void RunReinstallation(string moduleName, ControllerContext controllerContext, string user)
        {
            ModuleInfo sourceModuleInfo = ModuleInfo.Get(moduleName);

            RunEvent(moduleName, controllerContext);
            ModuleInfo targetModuleInfo = ModuleInfo.Get(moduleName);
            var installationFile = this._installationFileManager.ArchiveTempInstallationPath(moduleName, targetModuleInfo.Version);
            this._installationFileManager.LogInstallation(moduleName, new InstallationContext(sourceModuleInfo.ModuleName, sourceModuleInfo.Version, targetModuleInfo.Version, DateTime.UtcNow) { User = user, InstallationFileName = installationFile });
        }


        public IPath GetTempInstallationPath(string moduleName)
        {
            return _moduleInstaller.GetTempInstallationPath(moduleName);
        }
    }
}
