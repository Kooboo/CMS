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

namespace Kooboo.CMS.Account.Persistence.EntityFramework.Mapping
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

            this.Property(it => it.Password).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.PasswordSalt).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.FailedPasswordAttemptCount);
            this.Property(it => it.IsLockedOut);
            this.Property(it => it.UtcLastLockoutDate);
            this.Property(it => it.ActivateCode).HasColumnType("nvarchar").HasMaxLength(256);
            
            this.Ignore(it => it.IsDummy);          
            this.Ignore(it => it.UUID);
            this.ToTable("Kooboo_CMS_Account_Users");
        }
    }
}
