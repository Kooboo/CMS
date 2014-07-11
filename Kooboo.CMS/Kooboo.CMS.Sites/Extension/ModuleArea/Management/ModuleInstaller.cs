using Kooboo.CMS.Sites.Extension.Management;
using Kooboo.CMS.Sites.Extension.ModuleArea.Management.Events;
using Kooboo.Common;
using Kooboo.Common.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    /// <summary>
    /// Module installation steps:
    /// 1. Unzip
    /// 2. Check conflict assembly references
    /// 2. Copy assemblies
    /// 3. Run module installing event
    /// </summary>
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IModuleInstaller))]
    public class ModuleInstaller : IModuleInstaller
    {
        #region .ctor
        IAssemblyReferences _assemblyReferences;
        IInstallationFileManager _installationFileManager;
        public ModuleInstaller(IAssemblyReferences assemblyReferences, IInstallationFileManager installationFileManager)
        {
            this._assemblyReferences = assemblyReferences;
            this._installationFileManager = installationFileManager;
        }
        #endregion

        //#region Unzip
        //public string Unzip(ref string moduleName, Stream moduleStream, string user)
        //{
        //    string error = "";
        //    using (ModuleStreamEntry moduleEntry = new ModuleStreamEntry(moduleName, moduleStream))
        //    {
        //        if (!moduleEntry.IsValid())
        //        {
        //            error = "The module is invalid.".Localize();
        //            return error;
        //        }

        //        var moduleInfo = moduleEntry.ModuleInfo;
        //        if (moduleInfo == null)
        //        {
        //            error = "The module.config file is invalid.".Localize();
        //            return error;
        //        }

        //        moduleName = moduleEntry.ModuleName;

        //        ModulePath modulePath = new ModulePath(moduleEntry.ModuleName);
        //        var modulePhysicalPath = modulePath.PhysicalPath;

        //        if (Directory.Exists(modulePath.PhysicalPath))
        //        {
        //            error = "The module name already exists.".Localize();
        //            return error;
        //        }

        //        //save the module version
        //        var installationFile = this._installationFileManager.SaveInstallationFile(moduleEntry);
        //        this._installationFileManager.LogInstallation(moduleName, new InstallationContext(moduleInfo.ModuleName, moduleInfo.Version, DateTime.UtcNow) { User = user, InstallationFileName = installationFile });


        //        moduleEntry.Extract(modulePhysicalPath);
        //    }
        //    return error;
        //}
        //#endregion

        //#region CheckConflictedAssemblyReferences
        //public IEnumerable<ConflictedAssemblyReference> CheckConflictedAssemblyReferences(string moduleName)
        //{
        //    var assemblyFiles = GetAssemblyFiles(moduleName);
        //    return _assemblyReferences.Check(assemblyFiles);
        //}
        //#endregion

        //#region GetAssemblyFiles
        //private IEnumerable<string> GetAssemblyFiles(string moduleName)
        //{
        //    ModulePath modulePath = new ModulePath(moduleName);
        //    ModuleItemPath moduleBinPath = new ModuleItemPath(moduleName, "Bin");
        //    if (Directory.Exists(moduleBinPath.PhysicalPath))
        //    {
        //        return Directory.EnumerateFiles(moduleBinPath.PhysicalPath);
        //    }
        //    else
        //    {
        //        return new string[0];
        //    }
        //}
        //#endregion

        public UploadModuleResult Upload(string moduleName, Stream moduleStream, string user)
        {
            UploadModuleResult result = new UploadModuleResult();
            IPath tempInstallationPath = null;
            using (ModuleStreamEntry moduleEntry = new ModuleStreamEntry(moduleName, moduleStream))
            {
                if (!moduleEntry.IsValid())
                {
                    result.IsValid = false;
                }

                var moduleInfo = moduleEntry.ModuleInfo;
                if (moduleInfo == null)
                {
                    result.IsValid = false;
                }

                if (result.IsValid)
                {
                    moduleName = moduleEntry.ModuleName;
                    var existsModule = ModuleInfo.Get(moduleName);
                    if (existsModule != null)
                    {
                        result.ModuleExists = true;
                    }

                    tempInstallationPath = _installationFileManager.GetTempInstallationPath(moduleName);

                    moduleEntry.Extract(tempInstallationPath.PhysicalPath);

                    result.ModuleName = moduleName;
                    result.TempInstallationPath = tempInstallationPath;
                    result.TargetModuleInfo = moduleInfo;

                    var binFolder = Path.Combine(tempInstallationPath.PhysicalPath, "Bin");
                    if (Directory.Exists(binFolder))
                    {
                        var assemblyFiles = Directory.EnumerateFiles(binFolder);
                        result.ConflictedAssemblies = _assemblyReferences.Check(assemblyFiles);
                    }
                }
            }




            return result;
        }

        public void CopyAssemblies(string moduleName, bool @overrideFiles)
        {
            CopyFiles(moduleName, @overrideFiles);
        }
        public void RunInstallation(string moduleName, ControllerContext controllerContext, string user)
        {
            RunEvent(moduleName, controllerContext);
            ModuleInfo moduleInfo = ModuleInfo.Get(moduleName);
            var installationFile = this._installationFileManager.ArchiveTempInstallationPath(moduleName, moduleInfo.Version);
            this._installationFileManager.LogInstallation(moduleName, new InstallationContext(moduleInfo.ModuleName, moduleInfo.Version, DateTime.UtcNow) { User = user, InstallationFileName = installationFile });

        }
        #region CopyFiles
        internal void CopyFiles(string moduleName, bool @overrideSystemVersion)
        {
            var tempInstallationPath = _installationFileManager.GetTempInstallationPath(moduleName);
            if (!Directory.Exists(tempInstallationPath.PhysicalPath))
            {
                throw new Exception("The temporary installation directory has been deleted.".Localize());
            }
            ModulePath modulePath = new ModulePath(moduleName);
            Kooboo.Common.IO.IOUtility.CopyDirectory(tempInstallationPath.PhysicalPath, modulePath.PhysicalPath);
            var assemblyFiles = Directory.EnumerateFiles(modulePath.GetModuleInstallationFilePath("Bin").PhysicalPath);
            var binPath = Settings.BinDirectory;
            foreach (var item in assemblyFiles)
            {
                if (!_assemblyReferences.IsSystemAssembly(item))
                {
                    var fileName = Path.GetFileName(item);
                    var fileNameInBin = Path.Combine(binPath, fileName);
                    var exists = File.Exists(fileNameInBin);
                    if (!exists || (exists && overrideSystemVersion))
                    {
                        File.Copy(item, fileNameInBin, overrideSystemVersion);
                    }
                    _assemblyReferences.AddReference(fileNameInBin, moduleName);
                }
            }
        }
        #endregion

        #region RunEvent
        private void RunEvent(string moduleName, ControllerContext controllerContext)
        {
            var moduleEvent = Kooboo.Common.ObjectContainer.EngineContext.Current.TryResolve<IModuleInstallingEvents>(moduleName);
            if (moduleEvent == null)
            {
                moduleEvent = Kooboo.Common.ObjectContainer.EngineContext.Current.TryResolve<IModuleEvents>(moduleName);
            }
            if (moduleEvent != null)
            {
                moduleEvent.OnInstalling(new ModuleContext(moduleName), controllerContext);
            }
        }

        #endregion


        public IPath GetTempInstallationPath(string moduleName)
        {
            return _installationFileManager.GetTempInstallationPath(moduleName);
        }
    }
}
