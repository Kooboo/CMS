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
using Kooboo.CMS.Account.Persistence;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Account.Persistence.EntityFramework
{
    [Dependency(typeof(IRoleProvider), ComponentLifeStyle.InRequestScope, Order = 100)]
    [Dependency(typeof(IProvider<Role>), ComponentLifeStyle.InRequestScope, Order = 100)]
    public class RoleProvider : ProviderBase<Role>, IRoleProvider
    {
        #region .ctor
        AccountDBContext _dbContext;
        public RoleProvider(AccountDBContext dbContext)
            : base(dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion


        public override void Update(Models.Role @new, Models.Role old)
        {
            var update = Get(old);
            update.Name = @new.Name;
            update.Permissions = @new.Permissions;

            _dbContext.SaveChanges();
        }

        public override Role Get(Role dummy)
        {
            return _dbContext.Roles.Include("Permissions").Where(o => o.Name == dummy.Name).FirstOrDefault();
        }
    }
}
