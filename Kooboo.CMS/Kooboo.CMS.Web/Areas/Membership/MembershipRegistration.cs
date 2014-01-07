using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Account.Services;
using Kooboo.CMS.Common;
using Kooboo.Web.Mvc;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Membership
{
    public class MembershipRegistration : AreaRegistrationEx
    {
        public override string AreaName
        {
            get
            {
                return "Membership";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Membership_default",
                "Membership/{controller}/{action}",
                new { controller = "Membership", action = "Index" }
                , new[] { "Kooboo.CMS.Web.Areas.Membership.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );

            Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "Menu.config"));
            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "WebResources.config"));

            #region RegisterPermissions
            var roleManager = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<RoleManager>();
            roleManager.AddPermission(new Permission() { AreaName = "Membership", Group = "", Name = "Setting" });
            roleManager.AddPermission(new Permission() { AreaName = "Membership", Group = "", Name = "Group" });
            roleManager.AddPermission(new Permission() { AreaName = "Membership", Group = "", Name = "Member" });
            roleManager.AddPermission(new Permission() { AreaName = "Membership", Group = "", Name = "Connect" });
            #endregion

            base.RegisterArea(context);
        }
    }
}
