using Kooboo.IO;
using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    public interface IModuleVersioning
    {
        IEnumerable<InstallationContext> AllInstallationLogs(string moduleName);
        void LogInstallation(string moduleName, InstallationContext installationContext);
        InstallationContext GetLastestInstallation(string moduleName);
        /// <summary>
        /// Save the installation file
        /// </summary>
        /// <param name="moduleStreamEntry">The module stream entry.</param>
        /// <returns></returns>
        string SaveInstallationFile(ModuleStreamEntry moduleStreamEntry);
        bool IsInstallationFileExists(InstallationContext installationContext);
        Stream GetInstallationStream(InstallationContext installationContext);
        Stream GetInstallationStream(string moduleName, string installationFile);
        void RemoveHistory(string moduleName);
    }
}
