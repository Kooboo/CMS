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
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    public class InstallationScriptFinder : IInstallationScriptFinder
    {
        public IEnumerable<InstallationScriptFileInfo> GetInstallationScripts(string moduleName, InstallationContext context)
        {
            var scripts = GetInstallationScripts(moduleName);

            return scripts.Where(it => it.VersionRange.In(context.VersionRange)).OrderBy(it => it.VersionRange);

        }
        private IEnumerable<InstallationScriptFileInfo> GetInstallationScripts(string moduleName)
        {
            var installationScriptPath = new ModuleItemPath(moduleName, "InstallationScripts");
            List<InstallationScriptFileInfo> list = new List<InstallationScriptFileInfo>();
            if (Directory.Exists(installationScriptPath.PhysicalPath))
            {
                foreach (var item in Directory.EnumerateFiles(installationScriptPath.PhysicalPath))
                {
                    list.Add(new InstallationScriptFileInfo(item));
                }
            }
            return list;
        }
    }
}
