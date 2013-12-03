using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence.EntityFramework.Mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence.EntityFramework
{
    [Dependency(ComponentLifeStyle.InRequestScope)]
    public class BlogDbContext : DbContext
    {

        //public static string DefaultConnectionString = "Server=.\\SQL2012;Database=Kooboo_CMS;User Id=sa;Password=kooboo;";
        public static string DefaultConnectionString = "Data Source=" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cms_Data\\BlogDb.sdf");
        #region .ctor
        static System.Data.Entity.Infrastructure.DbCompiledModel dbCompiledModel;
        static BlogDbContext()
        {
            var builder = new DbModelBuilder();

            builder.Configurations.Add(new Category_Mapping());
            builder.Configurations.Add(new Blog_Mapping());

            //dbCompiledModel = builder.Build(new System.Data.Entity.Infrastructure.DbProviderInfo("System.Data.SqlClient", "2008")).Compile();
            dbCompiledModel = builder.Build(new System.Data.Entity.Infrastructure.DbProviderInfo("System.Data.SqlServerCe.4.0", "4.0")).Compile();
        }

        public BlogDbContext()
            : this(DefaultConnectionString)
        {
        }

        public BlogDbContext(string connectionString)
            : base(connectionString, dbCompiledModel)
        {
        }
        #endregion
    }
}
