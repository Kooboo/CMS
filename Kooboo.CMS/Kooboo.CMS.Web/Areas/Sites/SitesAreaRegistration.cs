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
using Kooboo.CMS.Common;
using Kooboo.CMS.Account.Services;
using Kooboo.CMS.Account.Models;

namespace Kooboo.CMS.Web.Areas.Sites
{
    public class SitesAreaRegistration : AreaRegistrationEx
    {
        public static string SiteAreaName = "Sites";
        public override string AreaName
        {
            get
            {
                return SiteAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sites_default",
                "Sites/{controller}/{action}", //{siteName}/{name}  
                new { action = "Index" }//, siteName = UrlParameter.Optional, name = UrlParameter.Optional 
                , null
                , new[] { "Kooboo.CMS.Web.Areas.Sites.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );


            Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(AreaName, Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "Menu.config"));
            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "WebResources.config"));

            #region RegisterPermissions
            var roleManager = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<RoleManager>();


            roleManager.AddPermission(Permission.Sites_Settings_SystemPermission);
            roleManager.AddPermission(Permission.Sites_Settings_UserSettingPermission);
            roleManager.AddPermission(Permission.Sites_Settings_CustomErrorPermission);
            roleManager.AddPermission(Permission.Sites_Settings_UrlRedirectPermission);
            roleManager.AddPermission(Permission.Sites_Settings_Robot_TxtPermission);
            roleManager.AddPermission(Permission.Sites_Settings_VisitRulePermission);

            roleManager.AddPermission(Permission.Sites_Templates_LayoutPermission);
            roleManager.AddPermission(Permission.Sites_Templates_ViewPermission);
            roleManager.AddPermission(Permission.Sites_Templates_LabelPermission);
            roleManager.AddPermission(Permission.Sites_Templates_FilePermission);
            roleManager.AddPermission(Permission.Sites_Development_ActionMappingPermission);
            roleManager.AddPermission(Permission.Sites_Development_SubmissionPermission);

            roleManager.AddPermission(Permission.Sites_Extensions_PluginPermission);
            roleManager.AddPermission(Permission.Sites_Extensions_ModulePermission);

            roleManager.AddPermission(Permission.Sites_Page_EditPermission);
            roleManager.AddPermission(Permission.Sites_Page_StyleEditPermission);
            roleManager.AddPermission(Permission.Sites_Page_PublishPermission);
            #endregion
            base.RegisterArea(context);
        }
    }
}
