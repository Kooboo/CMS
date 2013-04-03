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
        public static ModuleContext Create(Site site, string moduleName, ModuleSettings moduleSettings, ModulePosition position)
        {

            var context = new ModuleContext(site, moduleName, moduleSettings, position);

            if (!System.IO.Directory.Exists(context.ModulePath.PhysicalPath))
            {
                throw new Exception(string.Format("The module does not exist.Module name:{0}".Localize(), moduleName));
            }

            return context;
        }
        protected ModuleContext(Site site, string moduleName, ModuleSettings moduleSettings, ModulePosition position)
        {
            this.Site = site;
            ModuleName = moduleName;
            this.ModuleSettings = moduleSettings;
            this.ModulePosition = position;
        }
        public string ModuleName { get; private set; }
        public ModulePath ModulePath
        {
            get
            {
                return new ModulePath(this.ModuleName);
            }
        }
        public Site Site { get; private set; }
        public RouteCollection RouteTable
        {
            get
            {
                return RouteTables.GetRouteTable(this.ModuleName);
            }
        }

        public ModuleSettings ModuleSettings { get; private set; }

        public ModulePosition ModulePosition { get; private set; }
    }
}
