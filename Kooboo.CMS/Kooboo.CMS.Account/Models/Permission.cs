using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.CMS.Account.Models
{
    public class Permission
    {
        public Permission()
        {

        }
        public Permission(Permission p)
        {
            this.AreaName = p.AreaName;
            this.Group = p.Group;
            this.Name = p.Name;
            this.DisplayName = p.DisplayName;
        }
        private string id;
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    id = (AreaName ?? "") + ("_" + Group ?? "") + ("_" + Name ?? "");
                }
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string Name { get; set; }

        /// <summary>
        /// Foreign Key
        /// </summary>
        public string RoleName { get; set; }

        public virtual Role Role { get; set; }

        public string AreaName { get; set; }
        public string Group { get; set; }

        private string displayName;
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(displayName))
                {
                    return this.Name;
                }
                return displayName;
            }
            set
            {
                displayName = value;
            }
        }


        #region override object
        public static bool operator ==(Permission obj1, Permission obj2)
        {
            if (object.Equals(obj1, obj2) == true)
            {
                return true;
            }
            if (object.Equals(obj1, null) == true || object.Equals(obj2, null) == true)
            {
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(Permission obj1, Permission obj2)
        {
            return !(obj1 == obj2);
        }
        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                var permission = (Permission)obj;
                if (string.Compare(AreaName, permission.AreaName, true) == 0 &&
                    string.Compare(Group ?? "", permission.Group ?? "", true) == 0 &&
                    string.Compare(Name, permission.Name, true) == 0)
                {
                    return true;
                }
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return this.AreaName + "." + this.Group + "." + this.Name;
        }
        #endregion

        //public static Permission Account_User_MaangePermission = new Permission() { AreaName = "Account", Group = "", Name = "User" };

        //public static Permission Account_Role_ManagePermission = new Permission() { AreaName = "Account", Group = "", Name = "Role" };

        public static Permission Contents_SettingPermission = new Permission() { AreaName = "Contents", Group = "", Name = "Setting" };

        public static Permission Contents_SchemaPermission = new Permission() { AreaName = "Contents", Group = "", Name = "Schema", DisplayName = "Content type" };
        public static Permission Contents_FolderPermission = new Permission() { AreaName = "Contents", Group = "", Name = "Folder" };
        public static Permission Contents_ContentPermission = new Permission() { AreaName = "Contents", Group = "", Name = "Content" };
        public static Permission Contents_BroadcastingPermission = new Permission() { AreaName = "Contents", Group = "", Name = "Broadcasting" };
        public static Permission Contents_WorkflowPermission = new Permission() { AreaName = "Contents", Group = "", Name = "Workflow" };
        public static Permission Contents_SearchSettingPermission = new Permission() { AreaName = "Contents", Group = "", Name = "SearchSetting", DisplayName = "Search setting" };
        public static Permission Contents_HtmlBlockPermission = new Permission() { AreaName = "Contents", Group = "", Name = "HtmlBlock", DisplayName = "Html block" };

        public static Permission Sites_Settings_SystemPermission = new Permission() { AreaName = "Sites", Group = "System", Name = "Settings" };
        public static Permission Sites_Settings_UserSettingPermission = new Permission() { AreaName = "Sites", Group = "System", Name = "User" };
        public static Permission Sites_Settings_CustomErrorPermission = new Permission() { AreaName = "Sites", Group = "System", Name = "Custom error" };
        public static Permission Sites_Settings_UrlRedirectPermission = new Permission() { AreaName = "Sites", Group = "System", Name = "Url redirect" };

        public static Permission Sites_Settings_Robot_TxtPermission = new Permission() { AreaName = "Sites", Group = "Settings", Name = "Robots.txt" };

        public static Permission Sites_Templates_LayoutPermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "Layout" };
        public static Permission Sites_Templates_ViewPermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "View" };
        public static Permission Sites_Templates_LabelPermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "Label" };
        public static Permission Sites_Templates_ScriptPermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "Script" };
        public static Permission Sites_Templates_ThemePermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "Theme" };
        public static Permission Sites_Templates_FilePermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "File" };
        public static Permission Sites_Settings_PageUrlSettingPermission = new Permission() { AreaName = "Sites", Group = "Development", Name = "Action mapping", DisplayName = "Action mapping" };

        public static Permission Sites_Extensions_PluginPermission = new Permission() { AreaName = "Sites", Group = "Extensions", Name = "Plugin" };
        public static Permission Sites_Extensions_ModulePermission = new Permission() { AreaName = "Sites", Group = "Extensions", Name = "Module" };

        public static Permission Sites_Page_EditPermission = new Permission() { AreaName = "Sites", Group = "Page", Name = "Edit" };        
        public static Permission Sites_Page_StyleEditPermission = new Permission() { AreaName = "Sites", Group = "Page", Name = "Style editing" };
        public static Permission Sites_Page_PublishPermission = new Permission() { AreaName = "Sites", Group = "Page", Name = "Publish", DisplayName = "Publishing & Inline editing" };
    }
}
