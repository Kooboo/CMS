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
using Kooboo.CMS.Content.Models;
using System.IO;
using Kooboo.CMS.Content.Models.Paths;

namespace Kooboo.CMS.Content.Persistence.Sqlce
{
    public static class ModelExtensions
    {
        public static string GetTableName(this Schema schema)
        {
            return string.Format("{0}_{1}", schema.Repository.Name, schema.Name);
        }
        public static string GetConnectionString(this Repository repository)
        {
            var dataPath = new DataPath(repository.Name);
            var dataFile = Path.Combine(dataPath.PhysicalPath, repository.Name + ".sdf");

            var connectionString = string.Format("Data Source={0};Persist Security Info=False;", dataFile);
            if (!File.Exists(dataFile))
            {
                DatabaseHelper.CreateDatabase(dataFile, connectionString);
                SchemaManager.InitializeDatabase(repository);
            }
            return connectionString;
        }

        public static string GetCategoryTableName(this Repository repository)
        {
            return string.Format("{0}.__ContentCategory", repository.Name);
        }
        //public static string GetMediaContentTableName(this Repository repository)
        //{
        //    return string.Format("{0}__MediaContent", repository.Name);
        //}
    }
}
