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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;

using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(UserManager))]
    public class UserManager : ManagerBase<User, IUserProvider>
    {
        #region .ctor
        Kooboo.CMS.Account.Services.UserManager _accountUserManager;
        Kooboo.CMS.Account.Services.RoleManager _accountRoleManager;
        public UserManager(IUserProvider provider, Kooboo.CMS.Account.Services.UserManager accountUserManager, Kooboo.CMS.Account.Services.RoleManager roleManager)
            : base(provider)
        {
            this._accountUserManager = accountUserManager;
            this._accountRoleManager = roleManager;
        }

        #endregion

        #region All/Get/Update
        public override IEnumerable<User> All(Site site, string filterName)
        {
            var result = Provider.All(site).Select(it => it.AsActual());
            if (!string.IsNullOrEmpty(filterName))
            {
                result = result.Where(it => it.UserName.Contains(filterName, StringComparison.CurrentCultureIgnoreCase));
            }
            return result;
        }

        public override User Get(Site site, string name)
        {
            return Provider.Get(new User() { Site = site, UserName = name });
        }

        public override void Update(Site site, User @new, User old)
        {
            @new.Site = site;
            @old.Site = site;
            Provider.Update(@new, @old);
        }
        #endregion

        #region IsInRole
        public virtual bool IsInRole(Site site, string userName, string role)
        {
            if (IsAdministrator(userName))
            {
                return true;
            }
            var siteUser = this.Get(site, userName);

            if (siteUser != null && siteUser.Roles != null)
            {
                return siteUser.Roles.Any(it => it.EqualsOrNullEmpty(role, StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }
        #endregion

        #region Authorize
        public virtual bool Authorize(Site site, string userName, Kooboo.CMS.Account.Models.Permission permission)
        {
            string contextKey = "Permission:" + permission.ToString();
            var allow = ContextVariables.Current.GetObject<bool?>(contextKey);
            if (!allow.HasValue)
            {
                allow = false;

                if (IsAdministrator(userName))
                {
                    allow = true;
                }
                else
                {
                    var roles = GetRoles(site, userName);
                    allow = roles.Any(it => it.HasPermission(permission));
                }
                ContextVariables.Current.SetObject(contextKey, allow);
            }
            return allow.Value;
        }

        public virtual bool IsAdministrator(string userName)
        {
            var accountUser = _accountUserManager.Get(userName);
            if (accountUser == null)
            {
                return false;
            }
            return accountUser.IsAdministrator;
        }

        public virtual bool AllowCreatingSite(string userName)
        {
            return IsAdministrator(userName);
        }

        protected virtual IEnumerable<Kooboo.CMS.Account.Models.Role> GetRoles(Site site, string userName)
        {
            if (site != null)
            {
                var siteUser = this.Get(site, userName);

                if (siteUser != null && siteUser.Roles != null)
                {
                    return siteUser.Roles.Select(it => _accountRoleManager.Get(it)).Where(it => it != null);
                }
            }

            var accountUser = _accountUserManager.Get(userName);
            if (accountUser != null && !string.IsNullOrEmpty(accountUser.GlobalRoles))
            {
                return accountUser.GlobalRoles.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(it => _accountRoleManager.Get(it)).Where(it => it != null);
            }

            return new Kooboo.CMS.Account.Models.Role[0];
        }
        public virtual bool Authorize(Site site, string userName)
        {
            if (IsAdministrator(userName))
            {
                return true;
            }
            return GetRoles(site, userName).Count() > 0;
        }
        #endregion


    }
}
