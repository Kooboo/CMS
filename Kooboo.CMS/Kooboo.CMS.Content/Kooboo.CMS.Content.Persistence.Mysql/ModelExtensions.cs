using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.Mysql
{
    public static class ModelExtensions
    {
        public static string GetTableName(this Schema schema)
        {
            return string.Format("{0}.{1}", schema.Repository.Name, schema.Name);
        }
        public static string GetConnectionString(this Repository repository)
        {
            return MysqlSettings.Instance.ConnectionString;

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
