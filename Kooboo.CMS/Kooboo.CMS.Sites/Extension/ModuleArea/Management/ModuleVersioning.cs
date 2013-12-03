using Kooboo.IO;
using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleVersioning))]
    public class ModuleVersioning : IModuleVersioning
    {
        IBaseDir _baseDir;
        public ModuleVersioning(IBaseDir baseDir)
        {
            _baseDir = baseDir;
        }
        private string GetModuleWithVersionFile(string moduleName, string version)
        {
            var moduleHistoryPath = Path.Combine(_baseDir.Cms_DataPhysicalPath, "ModuleHistory", moduleName);
            if (!Directory.Exists(moduleHistoryPath))
            {
                Directory.CreateDirectory(moduleHistoryPath);
            }
            return Path.Combine(moduleHistoryPath, version + ".zip");

        }

        public void SaveModuleVersion(ModuleStreamEntry moduleStreamEntry)
        {
            var moduleWithVersionFile = GetModuleWithVersionFile(moduleStreamEntry.ModuleName, moduleStreamEntry.ModuleInfo.Version);
            moduleStreamEntry.SaveTo(moduleWithVersionFile);
        }

        public Stream GetModuleByVersion(string moduleName, string version)
        {
            var moduleWithVersionFile = GetModuleWithVersionFile(moduleName, version);
            if (File.Exists(moduleWithVersionFile))
            {
                return new FileStream(moduleWithVersionFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            return null;
        }
    }
}
