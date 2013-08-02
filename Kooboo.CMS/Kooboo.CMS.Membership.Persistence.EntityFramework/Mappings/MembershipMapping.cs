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
    public class MembershipMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Kooboo.CMS.Membership.Models.Membership>
    {
        public MembershipMapping()
        {
            this.Ignore(it => it.UUID);
     
            this.Property(it => it.Name).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.AuthCookieName).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.AuthCookieDomain).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.HashAlgorithmType).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.MaxInvalidPasswordAttempts);
            this.Property(it => it.MinRequiredPasswordLength);
            this.Property(it => it.PasswordStrengthRegularExpression).HasColumnType("nvarchar").HasMaxLength(256);

            this.ToTable("Kooboo_CMS_Membership_Membership");
        }
    }
}
