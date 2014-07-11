
using Kooboo.Common.ObjectContainer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

using Ionic.Zip;
using Kooboo.CMS.Common;
using Kooboo.Common.IO;
using Kooboo.Common.Web;
namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IInstallationFileManager))]
    public class InstallationFileManager : IInstallationFileManager
    {
        #region .ctor
        IBaseDir _baseDir;
        IPath _moduleInstallationPath;
        public InstallationFileManager(IBaseDir baseDir)
        {
            _baseDir = baseDir;
            _moduleInstallationPath = new CommonPath() { PhysicalPath = Path.Combine(_baseDir.Cms_DataPhysicalPath, "ModuleInstallations"), VirtualPath = Kooboo.Common.Web.UrlUtility.Combine(_baseDir.Cms_DataVirtualPath, "ModuleInstallations") };
            if (!Directory.Exists(_moduleInstallationPath.PhysicalPath))
            {
                Directory.CreateDirectory(_moduleInstallationPath.PhysicalPath);
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
            var logFile = Path.Combine(_moduleInstallationPath.PhysicalPath, moduleName, "InstallationLogs.txt");

            return logFile;
        }

        private void SaveInstallationContexts(string logFile, List<InstallationContext> logs)
        {
            var jsonData = JsonConvert.SerializeObject(logs, Formatting.Indented);
            IOUtility.SaveStringToFile(logFile, jsonData);
        }
        private List<InstallationContext> GetInstallationContexts(string logFile)
        {
            if (!File.Exists(logFile))
            {
                return new List<InstallationContext>();
            }
            var jsonData = IOUtility.ReadAsString(logFile);
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

        private IPath GetModuleHistoryPath(string moduleName)
        {
            var path = new CommonPath() { PhysicalPath = Path.Combine(_moduleInstallationPath.PhysicalPath, moduleName), VirtualPath = Kooboo.Common.Web.UrlUtility.Combine(_moduleInstallationPath.VirtualPath, moduleName) };
            IOUtility.EnsureDirectoryExists(path.PhysicalPath);
            return path;
        }


        private string GetInstallationVersionFilePath(string moduleName, string version)
        {
            var moduleHistoryPath = GetModuleHistoryPath(moduleName);

            return Path.Combine(moduleHistoryPath.PhysicalPath, version + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");

        }
        public string ArchiveTempInstallationPath(string moduleName, string version)
        {
            var installationFile = GetInstallationVersionFilePath(moduleName, version);
            var tempInstallationPath = GetTempInstallationPath(moduleName);
            using (ZipFile zip = new ZipFile(Encoding.UTF8))
            {
                zip.AddDirectory(tempInstallationPath.PhysicalPath);
                zip.Save(installationFile);
            }
            IOUtility.DeleteDirectory(tempInstallationPath.PhysicalPath, true);
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
            var installationFilePath = Path.Combine(moduleHistoryPath.PhysicalPath, installationFile);
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
            IOUtility.DeleteDirectory(moduleHistoryPath.PhysicalPath, true);
        }
        public IPath GetTempInstallationPath(string moduleName)
        {
            var path = GetModuleHistoryPath(moduleName);
            return new CommonPath()
            {
                PhysicalPath = Path.Combine(path.PhysicalPath, "TEMP"),
                VirtualPath = UrlUtility.Combine(path.VirtualPath, "TEMP")
            };
        }
    }
}
