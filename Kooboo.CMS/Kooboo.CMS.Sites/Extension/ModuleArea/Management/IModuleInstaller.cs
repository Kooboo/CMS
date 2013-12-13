using Kooboo.CMS.Sites.Extension.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    /// <summary>
    /// Module installation steps:(未来更好的安装流程可能是在上传文件后，显示Module的版本号等信息后再确认一次是否要继续安装。目前不确定是否需要这一步。)
    /// 1. Unzip uploaded stream
    /// 2. Check conflict assembly references
    /// 2. Copy assemblies
    /// 3. Run module installing event
    /// </summary>
    public interface IModuleInstaller
    {
        /// <summary>
        /// Unzips the specified default module name.
        /// </summary>
        /// <param name="defaultModuleName">It will be the upload file name, when the developer assign a different module in the module.config, the value will be changed to it.</param>
        /// <param name="moduleStream">The module stream.</param>
        /// <returns>error message</returns>
        string Unzip(ref string moduleName, Stream moduleStream, string user);

        IEnumerable<ConflictedAssemblyReference> CheckConflictedAssemblyReferences(string moduleName);

        void CopyAssemblies(string moduleName, bool @overrideSystemVersion);

        void RunEvent(string moduleName, ControllerContext controllerContext);

    }
}
