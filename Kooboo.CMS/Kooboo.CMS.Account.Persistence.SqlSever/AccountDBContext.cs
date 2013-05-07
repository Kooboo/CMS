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
using System.Data.Entity;

using Kooboo.CMS.Account.Models;
namespace Kooboo.CMS.Account.Persistence.SqlSever
{
    public class AccountDBContext : DbContext
    {
        const string SettingName = "Kooboo_CMS";
        static System.Data.Entity.Infrastructure.DbCompiledModel dbCompiledModel;
        static AccountDBContext()
        {

            var builder = new DbModelBuilder();

            builder.Configurations.Add(new Mapping.UserMapping());
            builder.Configurations.Add(new Mapping.RoleMapping());
            builder.Configurations.Add(new Mapping.PermissionMapping());

            dbCompiledModel = builder.Build(new System.Data.Entity.Infrastructure.DbProviderInfo("System.Data.SqlClient", "2008")).Compile();
        }

        public AccountDBContext()
            : base(SettingName, dbCompiledModel)
        {
        }
        public AccountDBContext(string connectionString)
            : base(connectionString, dbCompiledModel)
        {

        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
    }
}
