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

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public static class ModuleUninstaller
    {
        public static void Uninstall(string moduleName)
        {
            RemoveAssemblies(moduleName);
            DeleteModule(moduleName);
        }
        private static void DeleteModule(string moduleName)
        {
            ModulePath modulePath = new ModulePath(moduleName);
            IO.IOUtility.DeleteDirectory(modulePath.PhysicalPath, true);
        }
        private static void RemoveAssemblies(string moduleName)
        {
            ModulePath modulePath = new ModulePath(moduleName);
            ModuleItemPath moduleBinPath = new ModuleItemPath(moduleName, "Bin");
            var binPath = Settings.BinDirectory;
            if (Directory.Exists(moduleBinPath.PhysicalPath))
            {
                foreach (var file in Directory.EnumerateFiles(moduleBinPath.PhysicalPath))
                {
                    string fileName = Path.GetFileName(file);
                    if (!Assemblies.Defaults.Any(it => it.EqualsOrNullEmpty(fileName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        var binFile = Path.Combine(binPath, fileName);
                        if (File.Exists(binFile))
                        {
                            File.Delete(binFile);
                        }
                    }
                }
            }
        }
    }
}
