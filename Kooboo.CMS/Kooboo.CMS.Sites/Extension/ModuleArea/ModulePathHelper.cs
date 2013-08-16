using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModulePathHelper
    {
        #region .ctor
        string _moduleName;
        Site _site;
        public ModulePathHelper(string moduleName)
            : this(moduleName, null)
        {
        }
        public ModulePathHelper(string moduleName, Site site)
        {
            _site = site;
            _moduleName = moduleName;
        }
        #endregion

        #region GetModuleInstallationPath
        public IPath GetModuleInstallationPath()
        {
            return new ModulePath(_moduleName);
        }
        #endregion

        #region GetModuleInstallationFilePath
        public IPath GetModuleInstallationFilePath(params string[] moduleInstallationFilePaths)
        {
            var path = GetModuleInstallationPath();

            if (moduleInstallationFilePaths != null && moduleInstallationFilePaths.Length > 0)
            {
                return new CommonPath()
                {
                    PhysicalPath = Path.Combine(new[] { path.PhysicalPath }.Concat(moduleInstallationFilePaths).ToArray()),
                    VirtualPath = UrlUtility.Combine(new[] { path.VirtualPath }.Concat(moduleInstallationFilePaths).ToArray())
                };
            }
            return path;
        }
        #endregion

        #region GetModuleSharedFilePath/GetModuleLocalFilePath
        /// <summary>
        /// Get the module public shared file path, used by the same module in all websites. 
        /// </summary>
        /// <param name="extraPaths"></param>
        /// <returns></returns>
        public IPath GetModuleSharedFilePath(params string[] extraPaths)
        {
            var baseDir = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IBaseDir>();
            var physicalPaths = new[] { baseDir.Cms_DataPhysicalPath, "Modules", _moduleName };
            var virtualPaths = new[] { baseDir.Cms_DataVirtualPath, "Modules", _moduleName };
            if (extraPaths != null && extraPaths.Length > 0)
            {
                physicalPaths = physicalPaths.Concat(extraPaths).ToArray();
                virtualPaths = virtualPaths.Concat(extraPaths).ToArray();
            }
            var path = new CommonPath()
            {
                PhysicalPath = Path.Combine(physicalPaths),
                VirtualPath = UrlUtility.Combine(virtualPaths)
            };

            return path;

        }

        /// <summary>
        /// Get the site related module file path. Each website has its seperated local file path for every module
        /// </summary>
        /// <param name="extraPaths"></param>
        /// <returns></returns>
        public IPath GetModuleLocalFilePath(params string[] extraPaths)
        {
            if (_site == null)
            {
                throw new ArgumentNullException("site");
            }
            var physicalPaths = new[] { _site.PhysicalPath, "Modules", _moduleName };
            var virtualPaths = new[] { _site.VirtualPath, "Modules", _moduleName };
            if (extraPaths != null && extraPaths.Length > 0)
            {
                physicalPaths = physicalPaths.Concat(extraPaths).ToArray();
                virtualPaths = virtualPaths.Concat(extraPaths).ToArray();
            }
            var path = new CommonPath()
            {
                PhysicalPath = Path.Combine(physicalPaths),
                VirtualPath = UrlUtility.Combine(virtualPaths)
            };

            return path;
        }
        #endregion
    }
}
