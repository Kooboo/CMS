using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence.EntityFramework.Mappings
{
    public class Blog_Mapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Blog>
    {
        public Blog_Mapping()
        {
            this.Property(it => it.Title).HasMaxLength(256);
            this.Property(it => it.SiteName).HasMaxLength(256);
            this.HasMany(it => it.Categories).WithMany(it => it.Blogs);
        }
    }
}
