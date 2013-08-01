#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.Persistence.EntityFramework.Mappings
{
    public class MembershipUserMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<MembershipUser>
    {
        public MembershipUserMapping()
        {
            //this.HasKey(it => it.Id);

            //this.Property(it => it.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);          

            this.Ignore(it => it.UUID);

            this.Property(it => it.UserName).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.Email).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.Password).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.PasswordSalt).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.UtcCreationDate).HasColumnType("datetime");
            this.Property(it => it.IsApproved);
            this.Property(it => it.IsLockedOut);
            this.Property(it => it.InvalidPasswordAttempts);
            this.Property(it => it.ActivateCode).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.UtcLastLockoutDate).HasColumnType("datetime");
            this.Property(it => it.UtcLastLoginDate).HasColumnType("datetime");
            this.Property(it => it.UtcLastPasswordChangedDate).HasColumnType("datetime");
            this.Property(it => it.PasswordQuestion).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.PasswordAnswer).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.Culture).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.TimeZoneId).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.Comment).HasColumnType("nvarchar").HasMaxLength(1024);
            this.Property(it => it.ProviderType).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.ProviderUserId).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.ProfileXml).HasColumnType("nvarchar").HasMaxLength(1024);
            this.Property(it => it.ProviderExtraDataXml).HasColumnType("nvarchar").HasMaxLength(1024);
            this.Property(it => it.MembershipGroupsXml).HasColumnType("nvarchar").HasMaxLength(1024);

            this.HasRequired(it => it.Membership)
                .WithMany();

            this.ToTable("Kooboo_CMS_Member_MembershipUser");
        }
    }
}
