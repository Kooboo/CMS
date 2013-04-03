using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;
using System.Data.SqlServerCe;

namespace Kooboo.CMS.Content.Persistence.Sqlce
{
    public static class DatabaseHelper
    {
        public static void CreateDatabase(string dataFile, string connectionString)
        {
            var dir = Path.GetDirectoryName(dataFile);
            IO.IOUtility.EnsureDirectoryExists(dir);
            SqlCeEngine engine = new SqlCeEngine(connectionString);
            engine.CreateDatabase();
        }
    }
}
