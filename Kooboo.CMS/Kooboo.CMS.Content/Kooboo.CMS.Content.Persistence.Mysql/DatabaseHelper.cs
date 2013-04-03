using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;
using System.Data.SqlClient;

namespace Kooboo.CMS.Content.Persistence.Mysql
{
    public static class DatabaseHelper
    {
        private static object locker = new object();

        public static void InitializeDatabase(Repository repository)
        {
            SchemaManager.InitializeRepositoryDatabase(repository);
        }


        public static void DisposeDatabase(Repository repository)
        {
            SchemaManager.DropTable(repository, repository.GetCategoryTableName());
        }
    }
}