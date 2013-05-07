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

namespace Kooboo.CMS.Account.Persistence.SqlSever
{
    public class RoleProvider : SqlServerRepositoryBase<Role>, IRoleProvider
    {
        public RoleProvider()
            : base()
        {

        }
        

        public override void Update(Models.Role @new, Models.Role old)
        {
            var dbContext = GetDBContext();
            var update = Get(dbContext, old);
            update.Name = @new.Name;
            update.Permissions = @new.Permissions;

            dbContext.SaveChanges();
        }

        public override Role Get(AccountDBContext dbContext, Role dummy)
        {
            return dbContext.Roles.Include("Permissions").Where(o => o.Name == dummy.Name).FirstOrDefault();
        }
    }
}
