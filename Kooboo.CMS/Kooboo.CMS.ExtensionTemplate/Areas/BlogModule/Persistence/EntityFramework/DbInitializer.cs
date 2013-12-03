using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence.EntityFramework
{
    [Dependency(typeof(IDbInitializer))]
    public class DbInitializer : IDbInitializer
    {
        public void InitializeDb(string connectionString)
        {
            BlogDbContext dbContext = new BlogDbContext(connectionString);
            dbContext.Database.Initialize(true);
        }


        public void DeleteDb(string connectionString)
        {
            BlogDbContext dbContext = new BlogDbContext(connectionString);
            dbContext.Database.Delete();
        }
    }
}
