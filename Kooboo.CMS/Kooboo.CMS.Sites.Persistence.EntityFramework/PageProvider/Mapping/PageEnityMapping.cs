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

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.PageProvider.Mapping
{
    public class PageEnityMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PageEntity>
    {
        public PageEnityMapping()
        {
            this.HasKey(it => new { it.SiteName, it.FullName });
            this.Property(it => it.ParentPage).HasColumnType("nvarchar").HasMaxLength(128);

            this.ToTable("Kooboo_CMS_Sites_Pages");
        }
    }
    public class PageDraftEnityMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PageDraftEntity>
    {
        public PageDraftEnityMapping()
        {
            this.HasKey(it => new { it.SiteName, it.FullName });
            this.Property(it => it.ParentPage).HasColumnType("nvarchar").HasMaxLength(128);

            this.ToTable("Kooboo_CMS_Sites_PageDrafts");
        }
    }
}
