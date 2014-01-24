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
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework
{
    [Dependency(ComponentLifeStyle.InRequestScope)]
    public class SiteDBContext : DbContext
    {
        #region .ctor
        static System.Data.Entity.Infrastructure.DbCompiledModel dbCompiledModel;
        static SiteDBContext()
        {
            var builder = new DbModelBuilder();

            builder.Configurations.Add(new PageProvider.Mapping.PageEnityMapping());
            builder.Configurations.Add(new PageProvider.Mapping.PageDraftEnityMapping());
            builder.Configurations.Add(new System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<HtmlBlockProvider.HtmlBlockEntity>());
            builder.Configurations.Add(new System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<LabelProvider.CategoryEntity>());
            builder.Configurations.Add(new System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<LabelProvider.LabelEntity>());
            builder.Configurations.Add(new UserProvider.Mapping.SiteUserMapping());
            builder.Configurations.Add(new SiteProvider.Mapping.SiteSettingMapping());
            builder.Configurations.Add(new UrlRedirectsProvider.Mapping.UrlRedirectMapping());
            builder.Configurations.Add(new CustomErrorsProvider.Mapping.CustomErrorMapping());
            builder.Configurations.Add(new ABTestProvider.Mapping.ABRuleSettingMapping());
            builder.Configurations.Add(new ABTestProvider.Mapping.ABPageSettingMapping());
            builder.Configurations.Add(new ABTestProvider.Mapping.ABPageTestResultMapping());
            builder.Configurations.Add(new ABTestProvider.Mapping.ABSiteSettingMapping());
            dbCompiledModel = builder.Build(new System.Data.Entity.Infrastructure.DbProviderInfo(SiteEntitySetting.Instance.ProviderInvariantName, SiteEntitySetting.Instance.ProviderManifestToken)).Compile();
        }
        public SiteDBContext()
            : this(SiteEntitySetting.Instance.ConnectionString)
        {

        }
        public SiteDBContext(string connectionString)
            : base(connectionString, dbCompiledModel)
        {

        }
        #endregion

        public DbSet<PageProvider.PageEntity> Pages { get; set; }
        public DbSet<PageProvider.PageDraftEntity> PageDrafts { get; set; }
        public DbSet<HtmlBlockProvider.HtmlBlockEntity> HtmlBlocks { get; set; }
        public DbSet<LabelProvider.CategoryEntity> LabelCategories { get; set; }
        public DbSet<LabelProvider.LabelEntity> Labels { get; set; }
        public DbSet<UserProvider.SiteUserEntity> SiteUsers { get; set; }
        public DbSet<SiteProvider.SiteEntity> SiteSettings { get; set; }
        public DbSet<UrlRedirectsProvider.UrlRedirectEntity> UrlRedirects { get; set; }
        public DbSet<CustomErrorsProvider.CustomErrorEntity> CustomErrors { get; set; }

        #region --- A/B Test ---
        public DbSet<ABTestProvider.ABRuleSettingEntity> ABRuleSettings { get; set; }
        public DbSet<ABTestProvider.ABPageSettingEntity> ABPageSettings { get; set; }
        public DbSet<ABTestProvider.ABPageTestResultEntity> ABPageTestResults { get; set; }
        public DbSet<ABTestProvider.ABSiteSettingEntity> ABSiteSettings { get; set; }
        #endregion
    }
}
