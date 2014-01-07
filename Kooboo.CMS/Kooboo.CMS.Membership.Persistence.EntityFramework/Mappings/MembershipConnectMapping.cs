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
    public class MembershipConnectMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<MembershipConnect>
    {
        public MembershipConnectMapping()
        {
            this.Ignore(it => it.UUID);
            this.Property(it => it.Name).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.DisplayName).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.AppId).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.AppSecret).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.Enabled);
            this.Property(it => it.UsernameFormat).HasColumnType("nvarchar").HasMaxLength(256);

            this.Property(it => it.OptionsXml).HasColumnType("nvarchar").HasMaxLength(1024);
            this.Property(it => it.MembershipGroupsXml).HasColumnType("nvarchar").HasMaxLength(1024);

            this.HasRequired(it => it.Membership)
                .WithMany();

            this.ToTable("Kooboo_CMS_Membership_MembershipConnect");
        }
    }
}
