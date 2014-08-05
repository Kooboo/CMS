using Kooboo.CMS.Account.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Areas.Sites
{
    public class Permissions
    {
        public static Permission Sites_Settings_SystemPermission = new Permission() { AreaName = "Sites", Group = "System", Name = "Settings" };
        public static Permission Sites_Settings_UserSettingPermission = new Permission() { AreaName = "Sites", Group = "System", Name = "User" };
        public static Permission Sites_Settings_CustomErrorPermission = new Permission() { AreaName = "Sites", Group = "System", Name = "Custom error" };
        public static Permission Sites_Settings_UrlRedirectPermission = new Permission() { AreaName = "Sites", Group = "System", Name = "Url redirect" };
        public static Permission Sites_Settings_Robot_TxtPermission = new Permission() { AreaName = "Sites", Group = "Settings", Name = "Robots.txt" };
        public static Permission Sites_Settings_VisitRulePermission = new Permission() { AreaName = "Sites", Group = "System", Name = "A/B Test" };

        public static Permission Sites_Templates_LayoutPermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "Layout" };
        public static Permission Sites_Templates_ViewPermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "View" };
        public static Permission Sites_Templates_LabelPermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "Label" };
        public static Permission Sites_Templates_FilePermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "File", DisplayName = "Scripts/Themes/Custom files" };
        public static Permission Sites_Templates_PageMappingPermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "PageMapping", DisplayName = "Page mapping" };
        public static Permission Sites_Development_SubmissionPermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "Submission setting" };

        public static Permission Sites_Extensions_PluginPermission = new Permission() { AreaName = "Sites", Group = "Extensions", Name = "Plugin" };
        public static Permission Sites_Extensions_ModulePermission = new Permission() { AreaName = "Sites", Group = "Extensions", Name = "Module" };

        public static Permission Sites_Page_EditPermission = new Permission() { AreaName = "Sites", Group = "Page", Name = "Edit" };
        public static Permission Sites_Page_StyleEditPermission = new Permission() { AreaName = "Sites", Group = "Page", Name = "Style editing" };
        public static Permission Sites_Page_PublishPermission = new Permission() { AreaName = "Sites", Group = "Page", Name = "Publish", DisplayName = "Publishing & Inline editing" };

    }
}