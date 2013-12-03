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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Kooboo.CMS.Sites.Providers.SqlServer
{
    public class SiteDbContext : DbContext
    {
        static System.Data.Entity.Infrastructure.DbCompiledModel dbCompiledModel;
        static SiteDbContext()
        {

            var builder = new DbModelBuilder();

            builder.Configurations.Add(new PageProvider.Mapping.PageEnityMapping());
            builder.Configurations.Add(new PageProvider.Mapping.PageDraftEnityMapping());
            builder.Configurations.Add(new System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<HtmlBlockProvider.HtmlBlockEntity>());
            builder.Configurations.Add(new System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<LabelProvider.CategoryEntity>());
            builder.Configurations.Add(new System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<LabelProvider.LabelEntity>());
            builder.Configurations.Add(new UserProvider.Mapping.SiteUserMapping());

            dbCompiledModel = builder.Build(new System.Data.Entity.Infrastructure.DbProviderInfo("System.Data.SqlClient", "2008")).Compile();
        }
        public SiteDbContext()
        {

        }
        public static SiteDbContext CreateDbContext()
        {
            return new SiteDbContext(SiteOnSQLServerSettings.Instance.ConnectionString);
        }

        public SiteDbContext(string connectionString)
            : base(connectionString, dbCompiledModel)
        {

        }


        public DbSet<PageProvider.PageEntity> Pages { get; set; }
        public DbSet<PageProvider.PageDraftEntity> PageDrafts { get; set; }
        public DbSet<HtmlBlockProvider.HtmlBlockEntity> HtmlBlocks { get; set; }
        public DbSet<LabelProvider.CategoryEntity> LabelCategories { get; set; }
        public DbSet<LabelProvider.LabelEntity> Labels { get; set; }
        public DbSet<UserProvider.SiteUserEntity> SiteUsers { get; set; }
    }
}
