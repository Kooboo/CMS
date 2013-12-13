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

        private string GetModuleHistoryPath(string moduleName)
        {
            var path = Path.Combine(_moduleHistoryPath, moduleName);
            Kooboo.IO.IOUtility.EnsureDirectoryExists(path);
            return path;
        }


        private string GetInstallationVersionFilePath(string moduleName, string version)
        {
            var moduleHistoryPath = GetModuleHistoryPath(moduleName);

            return Path.Combine(moduleHistoryPath, version + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");

        }
        public string SaveInstallationFile(ModuleStreamEntry moduleStreamEntry)
        {
            var installationFile = GetInstallationVersionFilePath(moduleStreamEntry.ModuleName, moduleStreamEntry.ModuleInfo.Version);
            moduleStreamEntry.SaveTo(installationFile);
            return Path.GetFileName(installationFile);
        }

        public bool IsInstallationFileExists(InstallationContext installationContext)
        {
            var installationFilePath = GetInstallationFilePath(installationContext.ModuleName, installationContext.InstallationFileName);
            return File.Exists(installationFilePath);
        }

        private string GetInstallationFilePath(string moduleName, string installationFile)
        {
            var moduleHistoryPath = GetModuleHistoryPath(moduleName);
            var installationFilePath = Path.Combine(moduleHistoryPath, installationFile);
            return installationFilePath;
        }

        public Stream GetInstallationStream(InstallationContext installationContext)
        {
            return GetInstallationStream(installationContext.ModuleName, installationContext.InstallationFileName);
        }
        public Stream GetInstallationStream(string moduleName, string installationFile)
        {
            var installationFilePath = GetInstallationFilePath(moduleName, installationFile);
            if (File.Exists(installationFilePath))
            {
                using (var fs = new FileStream(installationFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return new MemoryStream(fs.ReadData());
                }
            }
            return null;
        }
        public void RemoveHistory(string moduleName)
        {
            var moduleHistoryPath = GetModuleHistoryPath(moduleName);
            Kooboo.IO.IOUtility.DeleteDirectory(moduleHistoryPath, true);
        }
    }
}
