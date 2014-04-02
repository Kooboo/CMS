#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using System.Web.Mvc;
using System.IO;
using Kooboo;
using Kooboo.Web.Mvc;

namespace KoobooModule.Areas.KoobooModule
{
    public class ModuleAreaRegistration : Kooboo.CMS.Common.AreaRegistrationEx
    {
        public const string ModuleName = "KoobooModule";
        public override string AreaName
        {
            get
            {
                return ModuleName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               this.GetType().Namespace + "_" + ModuleName + "_default",
                ModuleName + "/{controller}/{action}",
                new { controller = "Admin", action = "Index" }
                , null
                , new[] { "KoobooModule.Areas.KoobooModule.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );
            var areaPath = AreaHelpers.CombineAreaFilePhysicalPath(AreaName);
            if (Directory.Exists(areaPath))
            {
                var menuFile = AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "CMSMenu.config");
                if (File.Exists(menuFile))
                {
                    Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(AreaName, menuFile);
                }

                var globalMenuFile = AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "GlobalMenu.config");
                if (File.Exists(globalMenuFile))
                {
                    Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(Kooboo.CMS.Sites.Extension.UI.GlobalSidebarMenu.ModuleGlobalSidebarMenuItemProvider.GetGlobalSidebarMenuTemplateName(AreaName), globalMenuFile);
                }
                var resourceFile = Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "WebResources.config");
                if (File.Exists(resourceFile))
                {
                    Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, resourceFile);
                }
            }
        }
    }
}
