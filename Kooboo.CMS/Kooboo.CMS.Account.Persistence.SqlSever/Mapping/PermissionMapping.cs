using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Models;

namespace Kooboo.CMS.Account.Persistence.SqlSever.Mapping
{
    public class PermissionMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Permission>
    {
        public PermissionMapping()
        {
            this.HasKey(it => new { it.Id, it.RoleName });

            this.Property(it => it.Name).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.AreaName).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.Group).HasColumnType("nvarchar").HasMaxLength(256);
            this.Property(it => it.DisplayName).HasColumnType("nvarchar").HasMaxLength(256);

            this.ToTable("CMS_Account_Permissions");
        }
    }
}
