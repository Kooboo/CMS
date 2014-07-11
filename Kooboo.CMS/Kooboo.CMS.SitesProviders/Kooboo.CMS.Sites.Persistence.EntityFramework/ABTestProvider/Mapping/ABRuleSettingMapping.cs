extern alias EntityFramework;
using EntityFramework::System.Data.Entity.ModelConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.ABTestProvider.Mapping
{
    public class ABRuleSettingMapping : EntityTypeConfiguration<ABRuleSettingEntity>
    {
        public ABRuleSettingMapping()
        {
            this.HasKey(it => it.Id);
            this.Property(it => it.UUID).HasMaxLength(128).IsRequired();
            this.Property(it => it.SiteName).HasMaxLength(128);
            this.Property(it => it.ObjectXml).IsRequired();
            this.ToTable("Kooboo_CMS_Sites_ABRuleSettings");
        }
    }
}
