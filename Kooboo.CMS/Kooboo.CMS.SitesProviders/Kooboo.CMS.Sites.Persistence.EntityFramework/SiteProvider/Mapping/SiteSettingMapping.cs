extern alias EntityFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.SiteProvider.Mapping
{
    public class SiteSettingMapping : EntityFramework::System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<SiteEntity>
    {
        public SiteSettingMapping()
        {
            this.HasKey(it => new { it.FullName });

            this.Property(it => it.ParentName).HasColumnType("nvarchar").HasMaxLength(128);

            this.ToTable("Kooboo_CMS_Sites_Sites");
        }
    }
}
