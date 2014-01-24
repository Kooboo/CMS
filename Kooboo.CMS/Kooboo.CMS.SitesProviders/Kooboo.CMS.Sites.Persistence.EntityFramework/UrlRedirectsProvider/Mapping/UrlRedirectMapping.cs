using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.UrlRedirectsProvider.Mapping
{
    public class UrlRedirectMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<UrlRedirectEntity>
    {
        public UrlRedirectMapping()
        {
            this.HasKey(it => new { it.UUID, it.SiteName });
            this.Property(it => it.ObjectXml).IsRequired();
            this.ToTable("Kooboo_CMS_Sites_UrlRedirects");
        }
    }
}
