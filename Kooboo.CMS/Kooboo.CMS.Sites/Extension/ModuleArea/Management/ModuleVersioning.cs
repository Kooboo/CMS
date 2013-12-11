using Kooboo.IO;
using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleVersioning))]
    public class ModuleVersioning : IModuleVersioning
    {
        #region .ctor
        IBaseDir _baseDir;
        string _moduleHistoryPath;
        public ModuleVersioning(IBaseDir baseDir)
        {
            _baseDir = baseDir;
            _moduleHistoryPath = Path.Combine(_baseDir.Cms_DataPhysicalPath, "ModuleHistory");
            if (!Directory.Exists(_moduleHistoryPath))
            {
                Directory.CreateDirectory(_moduleHistoryPath);
            }
        }
        #endregion

        #region AllInstallationLogs
        public IEnumerable<InstallationContext> AllInstallationLogs(string moduleName)
        {
            var logFile = GetInstallationLogFile(moduleName);
            return GetInstallationContexts(logFile);
        }
        #endregion

        #region LogInstallation
        private string GetInstallationLogFile(string moduleName)
        {
            var logFile = Path.Combine(_moduleHistoryPath, moduleName, "InstallationLogs.txt");

            return logFile;
        }

        private void SaveInstallationContexts(string logFile, List<InstallationContext> logs)
        {
            var jsonData = JsonConvert.SerializeObject(logs, Formatting.Indented);
            Kooboo.IO.IOUtility.SaveStringToFile(logFile, jsonData);
        }
        private List<InstallationContext> GetInstallationContexts(string logFile)
        {
            if (!File.Exists(logFile))
            {
                return new List<InstallationContext>();
            }
            var jsonData = Kooboo.IO.IOUtility.ReadAsString(logFile);
            if (string.IsNullOrEmpty(jsonData))
            {
                return new List<InstallationContext>();
            }
            var logs = JsonConvert.DeserializeObject<List<InstallationContext>>(jsonData);
            return logs;
        }
        public void LogInstallation(string moduleName, InstallationContext upgradingContext)
        {
            var logFile = GetInstallationLogFile(moduleName);
            var list = GetInstallationContexts(logFile);
            list.Insert(0, upgradingContext);
            SaveInstallationContexts(logFile, list);
        }
        #endregion

        #region GetLastestInstallation
        public InstallationContext GetLastestInstallation(string moduleName)
        {
            var logFile = GetInstallationLogFile(moduleName);
            var list = GetInstallationContexts(logFile);
            return list.FirstOrDefault();
        }
        #endregion

        #region LogModuleFile

        private string GetModuleWithVersionFile(string moduleName, string version)
        {
            var moduleVersionFile = Path.Combine(_moduleHistoryPath, moduleName);

            return Path.Combine(moduleVersionFile, version + ".zip");

        }

        public void LogModuleFile(ModuleStreamEntry moduleStreamEntry)
        {
            var moduleWithVersionFile = GetModuleWithVersionFile(moduleStreamEntry.ModuleName, moduleStreamEntry.ModuleInfo.Version);
            moduleStreamEntry.SaveTo(moduleWithVersionFile);
        }

        #endregion

        #region GetModuleStream
        public Stream GetModuleStream(string moduleName, string version)
        {
            var moduleWithVersionFile = GetModuleWithVersionFile(moduleName, version);
            if (File.Exists(moduleWithVersionFile))
            {
                using (var fs = new FileStream(moduleWithVersionFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return new MemoryStream(fs.ReadData());
                }
            }
            return null;
        }
        #endregion

        #region RemoveVersion
        public void RemoveVersion(string moduleName)
        {
            var moduleVersion = Path.Combine(_moduleHistoryPath, moduleName);
            Kooboo.IO.IOUtility.DeleteDirectory(moduleVersion, true);
        }
        #endregion

        #region IsModuleVersionExists
        public bool IsModuleVersionExists(string moduleName, string version)
        {
            var moduleWithVersionFile = GetModuleWithVersionFile(moduleName, version);
            return File.Exists(moduleWithVersionFile);
        }
        #endregion
    }
}
