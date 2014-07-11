#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension.Management;
using Kooboo.CMS.Sites.Extension.ModuleArea.Management.Events;
using Kooboo.Common;
using Kooboo.Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IModuleUninstaller))]
    public class ModuleUninstaller : IModuleUninstaller
    {
        #region .ctor
        IAssemblyReferences _assemblyReferences;
        IInstallationFileManager _moduleVersioning;
        public ModuleUninstaller(IAssemblyReferences assemblyReferences, IInstallationFileManager moduleVersioning)
        {
            this._assemblyReferences = assemblyReferences;
            this._moduleVersioning = moduleVersioning;
        }
        #endregion

        #region RunEvent
        private void RunEvent(string moduleName, ControllerContext controllerContext)
        {
            var moduleEvent = Kooboo.Common.ObjectContainer.EngineContext.Current.TryResolve<IModuleUninstallingEvents>(moduleName);
            if (moduleEvent == null)
            {
                moduleEvent = Kooboo.Common.ObjectContainer.EngineContext.Current.TryResolve<IModuleEvents>(moduleName);
            }
            if (moduleEvent != null)
            {
                moduleEvent.OnUninstalling(new ModuleContext(moduleName), controllerContext);
            }
        }
        #endregion

        #region RemoveAssemblies
        internal void RemoveAssemblies(string moduleName)
        {
            var binPath = Settings.BinDirectory;
            foreach (var item in GetAssemblyFiles(moduleName))
            {
                var binFile = Path.Combine(binPath, Path.GetFileName(item));
                var removable = _assemblyReferences.RemoveReference(binFile, moduleName);
                if (removable)
                {
                    if (File.Exists(binFile))
                    {
                        File.Delete(binFile);
                    }
                }
            }
        }

        #region GetAssemblyFiles
        private IEnumerable<string> GetAssemblyFiles(string moduleName)
        {
            ModulePath modulePath = new ModulePath(moduleName);
            ModuleItemPath moduleBinPath = new ModuleItemPath(moduleName, "Bin");
            if (Directory.Exists(moduleBinPath.PhysicalPath))
            {
                return Directory.EnumerateFiles(moduleBinPath.PhysicalPath);
            }
            else
            {
                return new string[0];
            }
        }
        #endregion
        #endregion

        #region DeleteModuleArea
        internal void RemoveModuleArea(string moduleName)
        {
            ModulePath modulePath = new ModulePath(moduleName);
            IOUtility.DeleteDirectory(modulePath.PhysicalPath, true);
        }
        #endregion


        #region RemoveVersions
        public void RemoveVersions(string moduleName)
        {
            _moduleVersioning.RemoveHistory(moduleName);
        }
        #endregion

        public void RunUninstall(string moduleName, ControllerContext controllerContext)
        {
            RunEvent(moduleName, controllerContext);
            RemoveAssemblies(moduleName);
            RemoveModuleArea(moduleName);
            RemoveVersions(moduleName);
        }
    }
}
