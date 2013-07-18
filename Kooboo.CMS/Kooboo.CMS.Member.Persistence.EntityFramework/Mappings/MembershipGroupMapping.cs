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
    public class MembershipGroupMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<MembershipGroup>
    {
        public MembershipGroupMapping()
        {
            this.HasKey(it => it.Name);

            this.Property(it => it.Name).HasColumnType("nvarchar").HasMaxLength(256);

            this.HasRequired(it => it.Membership);

            this.ToTable("Kooboo_CMS_Member_MembershipGroup");
        }
    }
}
