using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Providers.SqlServer.PageProvider.Mapping
{
    public class PageEnityMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PageEntity>
    {
        public PageEnityMapping()
        {
            this.HasKey(it => new { it.SiteName, it.FullName });
            this.Property(it => it.ParentPage).HasColumnType("nvarchar").HasMaxLength(128);

            this.ToTable("Pages");
        }
    }
    public class PageDraftEnityMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PageDraftEntity>
    {
        public PageDraftEnityMapping()
        {
            this.HasKey(it => new { it.SiteName, it.FullName });
            this.Property(it => it.ParentPage).HasColumnType("nvarchar").HasMaxLength(128);

            this.ToTable("PageDrafts");
        }
    }
}
