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
using System.Web;
using Kooboo.CMS.Account.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Kooboo.CMS.Web2.Areas.Account.Models
{
    public class CheckPermission : Permission
    {
        public CheckPermission()
        {

        }
        public CheckPermission(Permission permission)
            : base(permission)
        {
        }
        public bool Checked { get; set; }
    }

    public class CreateRoleModel
    {
        public CreateRoleModel()
        { }

        public CreateRoleModel(Role role, IEnumerable<Permission> permissions)
        {
            this.Name = role.Name;

            this.AllPermissions = permissions.Select(it => new CheckPermission(it) { Checked = role.HasPermission(it) });
        }

        [Remote("IsNameAvailable", "Roles")]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        public IEnumerable<CheckPermission> AllPermissions { get; set; }

        public IEnumerable<string> GetAreas()
        {
            return AllPermissions.GroupBy(it => it.AreaName, StringComparer.CurrentCultureIgnoreCase).Select(it => it.Key);
        }
        public IEnumerable<string> GetGroups(string areaName)
        {
            return AllPermissions.Where(it => string.Compare(it.AreaName, areaName, true) == 0).GroupBy(it => it.Group, StringComparer.CurrentCultureIgnoreCase).Select(it => it.Key);
        }
        public IEnumerable<CheckPermission> GetPermission(string areaName, string group)
        {
            return AllPermissions.Where(it => string.Compare(it.AreaName, areaName, true) == 0
                && string.Compare(it.Group, group, true) == 0);
        }
        public Role ToRole()
        {
            return new Role()
            {
                Name = this.Name,
                Permissions = AllPermissions.Where(it => it.Checked).Select(it => new Permission(it)).ToList()
            };
        }
    }
}