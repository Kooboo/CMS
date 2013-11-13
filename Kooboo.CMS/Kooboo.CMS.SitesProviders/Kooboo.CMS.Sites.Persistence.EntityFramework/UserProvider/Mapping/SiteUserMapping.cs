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

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.UserProvider.Mapping
{
    public class SiteUserMapping : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<SiteUserEntity>
    {
        public SiteUserMapping()
        {
            this.HasKey(it => new { it.SiteName, it.UserName });

            this.ToTable("Kooboo_CMS_Sites_SiteUsers");
        }
    }
}
