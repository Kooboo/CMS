#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Member.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Member.Persistence.EntityFramework.Mappings
{
    public class MembershipConnectMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<MembershipConnect>
    {
        public MembershipConnectMapping()
        {
            this.HasKey(it => it.Name);

            this.Property(it => it.Name).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.DisplayName).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.AppId).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.AppSecret).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.Enabled);

            this.Property(it => it.OptionsXml).HasColumnType("nvarchar").HasMaxLength(1024);
            this.Property(it => it.MembershipGroupsXml).HasColumnType("nvarchar").HasMaxLength(1024);

            this.HasRequired(it => it.Membership);

            this.ToTable("Kooboo_CMS_Member_MembershipConnect");
        }
    }
}
