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
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using System.Web.Routing;
using Kooboo.Globalization;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModuleContext
    {
        #region Create
        public static ModuleContext Create(Site site, string moduleName, ModuleSettings moduleSettings, ModulePosition position)
        {

            var context = new ModuleContext(site, moduleName, moduleSettings, position);

            if (!System.IO.Directory.Exists(context.ModulePath.PhysicalPath))
            {
                throw new Exception(string.Format("The module does not exist.Module name:{0}".Localize(), moduleName));
            }
            return context;
        }
        #endregion

        #region ModuleContext
        protected ModuleContext(Site site, string moduleName, ModuleSettings moduleSettings, ModulePosition position)
        {
            this.Site = site;
            ModuleName = moduleName;
            this.ModuleSettings = moduleSettings;
            this.ModulePosition = position;
            this.ModuleInfo = ModuleInfo.Get(moduleName);
        }
        #endregion

        #region ModuleName
        public string ModuleName { get; private set; }
        #endregion

        #region ModuleInfo
        public ModuleInfo ModuleInfo { get; private set; }
        #endregion

        #region ModulePath
        public ModulePath ModulePath
        {
            get
            {
                return new ModulePath(this.ModuleName);
            }
        }
        #endregion

        #region Site
        public Site Site { get; private set; }
        #endregion

        #region RouteTable
        public RouteCollection RouteTable
        {
            get
            {
                return RouteTables.GetRouteTable(this.ModuleName);
            }
        }
        #endregion

        #region ModuleSettings
        public ModuleSettings ModuleSettings { get; private set; }
        #endregion

        #region ModulePosition
        public ModulePosition ModulePosition { get; private set; }
        #endregion

        #region GetSiteDataFolder
        public IPath GetSiteDataFolder()
        {
            return ModuleHelper.GetModuleDataPath(this.Site, this.ModuleName);
        }
        #endregion

        #region GetSharedDataFolder
        public IPath GetSharedDataFolder()
        {
            return ModuleHelper.GetModulePath(this.ModuleName);
        }
        #endregion
    }
}
