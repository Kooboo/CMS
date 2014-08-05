using Kooboo.CMS.Account.Services;
using Kooboo.Common;
using Kooboo.Common.Web;
using System.IO;
using System.Web.Mvc;

namespace Kooboo.CMS.Web2.Areas.Sites
{
    public class SitesAreaRegistration : AreaRegistrationEx
    {
        public override string AreaName 
        {
            get 
            {
                return "Sites";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               "Sites_default",
               "Sites/{controller}/{action}", //{siteName}/{name}  
               new { action = "Index" }//, siteName = UrlParameter.Optional, name = UrlParameter.Optional 
               , null
               , new[] { "Kooboo.CMS.Web2.Areas.Sites.Controllers", "Kooboo.Web.Mvc", "Kooboo.Common.Web.WebResourceLoader" }
           );
            Kooboo.Common.Web.Menu.MenuFactory.RegisterAreaMenu(AreaName, Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "Menu.config"));
            Kooboo.Common.Web.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "WebResources.config"));

            #region RegisterPermissions
            var roleManager = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<RoleManager>();


            roleManager.AddPermission(Permissions.Sites_Settings_SystemPermission);
            roleManager.AddPermission(Permissions.Sites_Settings_UserSettingPermission);
            roleManager.AddPermission(Permissions.Sites_Settings_CustomErrorPermission);
            roleManager.AddPermission(Permissions.Sites_Settings_UrlRedirectPermission);
            roleManager.AddPermission(Permissions.Sites_Settings_Robot_TxtPermission);
            roleManager.AddPermission(Permissions.Sites_Settings_VisitRulePermission);

            roleManager.AddPermission(Permissions.Sites_Templates_LayoutPermission);
            roleManager.AddPermission(Permissions.Sites_Templates_ViewPermission);
            roleManager.AddPermission(Permissions.Sites_Templates_LabelPermission);
            roleManager.AddPermission(Permissions.Sites_Templates_FilePermission);
            roleManager.AddPermission(Permissions.Sites_Templates_PageMappingPermission);
            roleManager.AddPermission(Permissions.Sites_Development_SubmissionPermission);

            roleManager.AddPermission(Permissions.Sites_Extensions_PluginPermission);
            roleManager.AddPermission(Permissions.Sites_Extensions_ModulePermission);

            roleManager.AddPermission(Permissions.Sites_Page_EditPermission);
            //roleManager.AddPermission(Permission.Sites_Page_StyleEditPermission);
            roleManager.AddPermission(Permissions.Sites_Page_PublishPermission);
            #endregion
            base.RegisterArea(context);
        }
    }
}