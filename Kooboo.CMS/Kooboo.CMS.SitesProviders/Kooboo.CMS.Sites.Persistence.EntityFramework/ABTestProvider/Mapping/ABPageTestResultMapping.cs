extern alias EntityFramework;
using EntityFramework::System.Data.Entity.ModelConfiguration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.ABTestProvider.Mapping
{
    public class ABPageTestResultMapping : EntityTypeConfiguration<ABPageTestResultEntity>
    {
        public ABPageTestResultMapping()
        {
            this.HasKey(it => new { it.UUID, it.SiteName });
            this.Property(it => it.ObjectXml).IsRequired();
            this.ToTable("Kooboo_CMS_Sites_ABPageTestResults");
        }
    }
}
