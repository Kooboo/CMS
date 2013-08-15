using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public static class ModuleHelper
    {
        #region GetModulePath
        public static IPath GetModulePath(string moduleName)
        {
            return new ModulePath(moduleName);
        }
        #endregion

        #region GetModuleItemPath
        public static IPath GetModuleItemPath(string moduleName, params string[] moduleItemPaths)
        {
            var path = GetModulePath(moduleName);

            if (moduleItemPaths != null && moduleItemPaths.Length > 0)
            {
                return new CommonPath()
                {
                    PhysicalPath = Path.Combine(new[] { path.PhysicalPath }.Concat(moduleItemPaths).ToArray()),
                    VirtualPath = UrlUtility.Combine(new[] { path.VirtualPath }.Concat(moduleItemPaths).ToArray())
                };
            }
            return path;
        }
        #endregion

        #region GetModuleDataPath
        public static IPath GetModuleDataPath(Site site, string moduleName, params string[] extraPaths)
        {
            var physicalPaths = new[] { site.PhysicalPath, "Modules", moduleName };
            var virtualPaths = new[] { site.VirtualPath, "Modules", moduleName };
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
