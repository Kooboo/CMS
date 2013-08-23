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
using Kooboo.CMS.Account.Services;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence.EntityFramework;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule
{
    public class ModuleAreaRegistration : AreaRegistration
    {
        public const string ModuleName = "BlogModule";
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
               ModuleName + "_default",
                ModuleName + "/{controller}/{action}",
                new { action = "Index" }
                , null
                , new[] { "Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );

            var menuFile = AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "CMSMenu.config");
            if (File.Exists(menuFile))
            {
                Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(AreaName, menuFile);
            }
            var resourceFile = Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "WebResources.config");
            if (File.Exists(resourceFile))
            {
                Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, resourceFile);
            }

            ModuleInfo moduleInfo = ModuleInfo.Get(ModuleAreaRegistration.ModuleName);
            if (moduleInfo.DefaultSettings.CustomSettings.ContainsKey("ConnectionString"))
            {
                BlogDbContext.DefaultConnectionString = moduleInfo.DefaultSettings.CustomSettings["ConnectionString"];
            }

            #region RegisterPermissions
            var roleManager = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<RoleManager>();
            roleManager.AddPermission(new Permission() { AreaName = "Blog", Group = "", Name = "Blog" });
            roleManager.AddPermission(new Permission() { AreaName = "Blog", Group = "", Name = "Category" });
            #endregion
        }
    }
}
