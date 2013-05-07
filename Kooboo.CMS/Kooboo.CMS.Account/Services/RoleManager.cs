#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Account.Persistence;

namespace Kooboo.CMS.Account.Services
{
    public class RoleManager
    {
        public IRoleProvider RoleProvider { get; private set; }
        public RoleManager(IRoleProvider roleProvider)
        {
            RoleProvider = roleProvider;
        }
        public virtual IEnumerable<Permission> AllPermissions()
        {
            //yield return Permission.Account_User_MaangePermission;
            //yield return Permission.Account_Role_ManagePermission;

            yield return Permission.Contents_SettingPermission;

            yield return Permission.Contents_SchemaPermission;
            yield return Permission.Contents_FolderPermission;
            yield return Permission.Contents_ContentPermission;
            yield return Permission.Contents_BroadcastingPermission;
            yield return Permission.Contents_WorkflowPermission;

            yield return Permission.Sites_Settings_SystemPermission;
            yield return Permission.Sites_Settings_UserSettingPermission;
            yield return Permission.Sites_Settings_CustomErrorPermission;
            yield return Permission.Sites_Settings_UrlRedirectPermission;
            yield return Permission.Sites_Settings_Robot_TxtPermission;
            yield return Permission.Sites_Settings_VisitRulePermission;

            yield return Permission.Sites_Templates_LayoutPermission;
            yield return Permission.Sites_Templates_ViewPermission;
            yield return Permission.Sites_Templates_LabelPermission;
            yield return Permission.Sites_Templates_FilePermission;
            yield return Permission.Sites_Development_ActionMappingPermission;
            yield return Permission.Sites_Development_SubmissionPermission;

            yield return Permission.Sites_Extensions_PluginPermission;
            yield return Permission.Sites_Extensions_ModulePermission;

            yield return Permission.Sites_Page_EditPermission;
            yield return Permission.Contents_HtmlBlockPermission;
            yield return Permission.Sites_Page_StyleEditPermission;
            yield return Permission.Sites_Page_PublishPermission;
        }
        public virtual void Add(Role role)
        {
            RoleProvider.Add(role);
        }
        public virtual void Delete(string roleName)
        {
            RoleProvider.Remove(new Role() { Name = roleName });
        }
        public virtual Role Get(string roleName)
        {
            return RoleProvider.Get(new Role() { Name = roleName });
        }
        public virtual IEnumerable<Role> All()
        {
            return RoleProvider.All();
        }
        public virtual void Update(string roleName, Role newRole)
        {
            var old = Get(roleName);
            RoleProvider.Update(newRole, old);
        }
    }
}
