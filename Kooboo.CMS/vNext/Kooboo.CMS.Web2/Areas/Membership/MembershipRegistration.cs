using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Account.Services;
using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web2.Areas.Membership
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
                , new[] { "Kooboo.CMS.Web2.Areas.Membership.Controllers", "Kooboo.Web.Mvc", "Kooboo.Common.Web.WebResourceLoader" }
            );

            Kooboo.Common.Web.Menu.MenuFactory.RegisterAreaMenu(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "Menu.config"));
            Kooboo.Common.Web.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "WebResources.config"));

            #region RegisterPermissions
            var roleManager = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<RoleManager>();
            roleManager.AddPermission(new Permission() { AreaName = "Membership", Group = "", Name = "Setting" });
            roleManager.AddPermission(new Permission() { AreaName = "Membership", Group = "", Name = "Group" });
            roleManager.AddPermission(new Permission() { AreaName = "Membership", Group = "", Name = "Member" });
            roleManager.AddPermission(new Permission() { AreaName = "Membership", Group = "", Name = "Connect" });
            #endregion

            base.RegisterArea(context);
        }
    }
}
