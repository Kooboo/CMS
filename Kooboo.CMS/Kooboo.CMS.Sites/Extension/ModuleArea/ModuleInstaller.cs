using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;
using Kooboo.Globalization;
namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public static class ModuleInstaller
    {
        public static ModuleInfo Install(string moduleName, Stream moduleStream, ref StringBuilder log)
        {
            ModuleInfo moduleInfo = null;
            if (Unzip(moduleName, moduleStream, ref log))
            {
                moduleInfo = ModuleInfo.Get(moduleName);
                CopyAssembies(moduleInfo, ref log);
            }
            return moduleInfo;
        }
        public static bool Unzip(string moduleName, Stream moduleStream, ref StringBuilder log)
        {
            ModulePath modulePath = new ModulePath(moduleName);
            if (Directory.Exists(modulePath.PhysicalPath))
            {
                log.Append("The module name already exists.".Localize());
                return false;
            }
            using (ZipFile zipFile = ZipFile.Read(moduleStream))
            {
                if (!Verify(zipFile))
                {
                    log.Append("The module is invalid.".Localize());
                    return false;
                }
                var webconfigEntry = zipFile["web.config"];
                if (webconfigEntry != null)
                {
                    zipFile.RemoveEntry(webconfigEntry);
                }

                zipFile.ExtractAll(modulePath.PhysicalPath, ExtractExistingFileAction.OverwriteSilently);
            }
            return true;
        }
        private static bool Verify(ZipFile zipFile)
        {
            var moduleConfigEntry = zipFile[RouteTables.RouteFile];
            var routesConfigEntry = zipFile[ModuleInfo.ModuleInfoFileName];
            if (moduleConfigEntry == null || routesConfigEntry == null)
            {
                return false;
            }
            return true;
        }
        private static void CopyAssembies(ModuleInfo moduleInfo, ref StringBuilder log)
        {
            ModulePath modulePath = new ModulePath(moduleInfo.ModuleName);
            ModuleEntryPath moduleBinPath = new ModuleEntryPath(moduleInfo.ModuleName, "Bin");
            var binPath = Settings.BinDirectory;
            if (Directory.Exists(moduleBinPath.PhysicalPath))
            {
                foreach (var file in Directory.EnumerateFiles(moduleBinPath.PhysicalPath))
                {
                    string fileName = Path.GetFileName(file);
                    if (!Assemblies.Defaults.Any(it => it.EqualsOrNullEmpty(fileName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        File.Copy(file, Path.Combine(binPath, fileName), true);
                    }
                }
            }
        }
    }
}
