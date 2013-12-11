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
        void LogInstallation(string moduleName, InstallationContext upgradingContext);
        InstallationContext GetLastestInstallation(string moduleName);
        void LogModuleFile(ModuleStreamEntry moduleStreamEntry);
        bool IsModuleVersionExists(string moduleName, string version);

        Stream GetModuleStream(string moduleName, string verion);

        void RemoveVersion(string moduleName);
    }
}
