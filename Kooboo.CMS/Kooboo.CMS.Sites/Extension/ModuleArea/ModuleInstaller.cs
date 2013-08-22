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

            moduleInfo = Unzip(moduleName, moduleStream, ref log);
            if (moduleInfo != null)
            {
                CopyAssembies(moduleInfo, ref log);
            }

            return moduleInfo;
        }
        public static ModuleInfo Unzip(string moduleName, Stream moduleStream, ref StringBuilder log)
        {
            //ModulePath modulePath = new ModulePath(moduleName);
            //if (Directory.Exists(modulePath.PhysicalPath))
            //{
            //    log.Append("The module name already exists.".Localize());
            //    return false;
            //}
            ModuleInfo moduleInfo = null;
            using (ZipFile zipFile = ZipFile.Read(moduleStream))
            {
                string dirName;
                var moduleConfigEntry = ParseZipFile(zipFile, out dirName);
                if (moduleConfigEntry == null)
                {
                    log.Append("The module is invalid.".Localize());
                    return null;
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    moduleConfigEntry.Extract(ms);
                    ms.Position = 0;
                    moduleInfo = ModuleInfo.Get(ms);
                }
                if (moduleInfo == null)
                {
                    return null;
                }
                if (string.IsNullOrEmpty(moduleInfo.ModuleName))
                {
                    moduleInfo.ModuleName = moduleName;
                }

                ModulePath modulePath = new ModulePath(moduleName);
                var modulePhysicalPath = modulePath.PhysicalPath;

                if (Directory.Exists(modulePath.PhysicalPath))
                {
                    log.Append("The module name already exists.".Localize());
                    return null;
                }
                var webconfigEntry = zipFile["web.config"];
                if (webconfigEntry != null)
                {
                    zipFile.RemoveEntry(webconfigEntry);
                }
                zipFile.ExtractAll(modulePhysicalPath, ExtractExistingFileAction.OverwriteSilently);

                if (!string.IsNullOrEmpty(dirName))
                {
                    var subDir = Path.Combine(modulePhysicalPath, dirName);
                    Kooboo.IO.IOUtility.CopyDirectory(subDir, modulePhysicalPath);
                    Kooboo.IO.IOUtility.DeleteDirectory(subDir, true);
                }

            }
            return moduleInfo;
        }
        private static ZipEntry ParseZipFile(ZipFile zipFile, out string dirName)
        {
            dirName = null;
            var moduleConfigEntry = zipFile[ModuleInfo.ModuleInfoFileName];
            if (moduleConfigEntry == null)
            {
                if (zipFile.Entries.Count > 0)
                {
                    moduleConfigEntry = zipFile.Entries.Where(it => it.FileName.Contains(ModuleInfo.ModuleInfoFileName, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();
                    if (moduleConfigEntry != null)
                    {
                        var dirs = moduleConfigEntry.FileName.Split('/');
                        if (dirs.Length > 2)
                        {
                            moduleConfigEntry = null;
                        }
                        else
                        {
                            dirName = dirs[0];
                        }
                    }
                }
            }
            return moduleConfigEntry;
        }
        private static void CopyAssembies(ModuleInfo moduleInfo, ref StringBuilder log)
        {
            ModulePath modulePath = new ModulePath(moduleInfo.ModuleName);
            ModuleItemPath moduleBinPath = new ModuleItemPath(moduleInfo.ModuleName, "Bin");
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
