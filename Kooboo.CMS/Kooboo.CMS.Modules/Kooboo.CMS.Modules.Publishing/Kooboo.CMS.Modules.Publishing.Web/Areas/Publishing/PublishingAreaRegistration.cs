using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Account.Services;
using Kooboo.CMS.Common;
using Kooboo.Web.Mvc;
using System.IO;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing
{
    public class PublishingAreaRegistration : AreaRegistrationEx
    {
        #region Permissions
        public static Permission Publishing_Local_Queue = new Permission() { AreaName = "Publishing", Group = "Local", Name = "Queue" };
        public static Permission Publishing_Remote_RemoteSites = new Permission() { AreaName = "Publishing", Group = "Remote", Name = "RemoteSites", DisplayName = "Remote sites" };
        public static Permission Publishing_Remote_TextFolderMapping = new Permission() { AreaName = "Publishing", Group = "Remote", Name = "TextFolderMapping", DisplayName = "TextFolder mapping" };
        public static Permission Publishing_Remote_Queue = new Permission() { AreaName = "Publishing", Group = "Remote", Name = "Queue" };
        public static Permission Publishing_Remote_Incoming = new Permission() { AreaName = "Publishing", Group = "Remote", Name = "Incoming" };
        public static Permission Publishing_Remote_Cmis = new Permission() { AreaName = "Publishing", Group = "Remote", Name = "Cmis", DisplayName = "Cmis service" };
        public static Permission Publishing_Local_Logs = new Permission() { AreaName = "Publishing", Group = "Local", Name = "Logs" };

        #endregion
        public const string ModuleName = "Publishing";
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
                "Publishing_default",
                "Publishing/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );


            Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(AreaName, Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "Menu.config"));
            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "WebResources.config"));

            base.RegisterArea(context);

            #region RegisterPermissions

            var roleManager = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<RoleManager>();


            roleManager.AddPermission(Publishing_Local_Queue);
            roleManager.AddPermission(Publishing_Remote_RemoteSites);
            roleManager.AddPermission(Publishing_Remote_TextFolderMapping);
            roleManager.AddPermission(Publishing_Remote_Queue);
            roleManager.AddPermission(Publishing_Remote_Incoming);
            //roleManager.AddPermission(Permission.Publishing_Remote_Cmis);
            roleManager.AddPermission(Publishing_Local_Logs);
            #endregion
        }
    }
}
