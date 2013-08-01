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
        static IList<Permission> permissions = new List<Permission>();
        #region .ctor
        public IRoleProvider RoleProvider { get; private set; }
        public RoleManager(IRoleProvider roleProvider)
        {
            RoleProvider = roleProvider;
        }
        #endregion

        #region Permissions
        public virtual IEnumerable<Permission> AllPermissions()
        {
            return permissions;
        }
        public virtual void AddPermission(Permission permission)
        {
            permissions.Add(permission);
        }
        #endregion

        #region Roles
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
        #endregion
    }
}
