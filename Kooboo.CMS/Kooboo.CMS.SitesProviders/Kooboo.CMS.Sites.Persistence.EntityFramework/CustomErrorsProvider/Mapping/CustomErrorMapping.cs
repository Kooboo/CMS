using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.CustomErrorsProvider.Mapping
{
    public class CustomErrorMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<CustomErrorEntity>
    {
        public CustomErrorMapping()
        {
            this.HasKey(it => new { it.UUID, it.SiteName });
            this.Property(it => it.ObjectXml).IsRequired();
            this.ToTable("Kooboo_CMS_Sites_CustomErrors");
        }
    }
}
