#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.Persistence.EntityFramework
{
    [Dependency(ComponentLifeStyle.InRequestScope)]
    public class MemberDBContext : DbContext
    {
        #region .ctor
        static System.Data.Entity.Infrastructure.DbCompiledModel dbCompiledModel;
        static MemberDBContext()
        {
            var builder = new DbModelBuilder();

            builder.Configurations.Add(new Mappings.MembershipMapping());
            builder.Configurations.Add(new Mappings.MembershipUserMapping());
            builder.Configurations.Add(new Mappings.MembershipGroupMapping());
            builder.Configurations.Add(new Mappings.MembershipConnectMapping());

            dbCompiledModel = builder.Build(new System.Data.Entity.Infrastructure.DbProviderInfo(MemberEntitySetting.Instance.ProviderInvariantName, MemberEntitySetting.Instance.ProviderManifestToken)).Compile();

            //builder.Conventions.Remove<IncludeMetadataConvention>();

          
        }
        public MemberDBContext()
            : this(MemberEntitySetting.Instance.ConnectionString)
        {

        }
        public MemberDBContext(string connectionString)
            : base(connectionString, dbCompiledModel)
        {
              
       
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        #endregion
  
        //public DbSet<Kooboo.CMS.Membership.Models.Membership> Memberships { get; set; }
        //public DbSet<MembershipUser> MembershipUsers { get; set; }
        //public DbSet<MembershipGroup> MembershipGroups { get; set; }
        //public DbSet<MembershipConnect> MembershipConnects { get; set; }
    }
}
