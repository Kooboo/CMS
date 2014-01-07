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
    public class MembershipGroupMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<MembershipGroup>
    {
        public MembershipGroupMapping()
        {

            this.Ignore(it => it.UUID);

            this.Property(it => it.Name).HasColumnType("nvarchar").HasMaxLength(256);
            
            this.HasRequired(it => it.Membership)
                .WithMany();

            this.ToTable("Kooboo_CMS_Membership_MembershipGroup");
        }
    }
}
