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
    public class UserMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            this.HasKey(it => it.Email);

            this.Property(it => it.UserName).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.IsAdministrator);
            this.Property(it => it.UICulture).HasColumnType("nvarchar").HasMaxLength(50);
            this.Property(it => it.CustomFieldsXml).HasColumnType("text");

            this.Ignore(it => it.Password);
            this.Ignore(it => it.IsDummy);
            this.Ignore(it => it.FailedPasswordAttemptCount);
            this.Ignore(it => it.IsLockedOut);
            this.Ignore(it => it.LastLockoutDate);
            this.Ignore(it => it.UUID);
            this.ToTable("CMS_Account_Users");
        }
    }
}
