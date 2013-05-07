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

namespace Kooboo.CMS.Account.Persistence.SqlSever.Mapping
{
    public class RoleMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Role>
    {
        public RoleMapping()
        {
            this.HasKey(it => it.Name);
            this.Ignore(it => it.IsDummy);
            this.HasMany(it => it.Permissions);
            this.Ignore(it => it.UUID);
            this.ToTable("CMS_Account_Roles");
        }
    }
}
