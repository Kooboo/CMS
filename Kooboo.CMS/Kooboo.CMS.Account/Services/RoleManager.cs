using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Models;

namespace Kooboo.CMS.Account.Services
{
    public class RoleManager
    {
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
            yield return Permission.Sites_Settings_PageUrlSettingPermission;
            yield return Permission.Sites_Settings_Robot_TxtPermission;

            yield return Permission.Sites_Templates_LayoutPermission;
            yield return Permission.Sites_Templates_ViewPermission;
            yield return Permission.Sites_Templates_LabelPermission;
            yield return Permission.Sites_Templates_ScriptPermission;
            yield return Permission.Sites_Templates_ThemePermission;
            yield return Permission.Sites_Templates_FilePermission;

            yield return Permission.Sites_Extensions_PluginPermission;
            yield return Permission.Sites_Extensions_ModulePermission;

            yield return Permission.Sites_Page_EditPermission;
            yield return Permission.Contents_HtmlBlockPermission;            
            yield return Permission.Sites_Page_StyleEditPermission;
            yield return Permission.Sites_Page_PublishPermission;
        }
        public void Add(Role role)
        {
            Persistence.RepositoryFactory.RoleRepository.Add(role);
        }
        public void Delete(string roleName)
        {
            Persistence.RepositoryFactory.RoleRepository.Remove(new Role() { Name = roleName });
        }
        public Role Get(string roleName)
        {
            return Persistence.RepositoryFactory.RoleRepository.Get(new Role() { Name = roleName });
        }
        public IQueryable<Role> All()
        {
            return Persistence.RepositoryFactory.RoleRepository.All();
        }
        public void Update(string roleName, Role newRole)
        {
            var old = Get(roleName);
            Persistence.RepositoryFactory.RoleRepository.Update(newRole, old);
        }
    }
}
