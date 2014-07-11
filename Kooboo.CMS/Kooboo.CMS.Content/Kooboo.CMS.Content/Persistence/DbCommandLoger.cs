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
using System.Data.Common;
using System.IO;


using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.Common.IO;

namespace Kooboo.CMS.Content.Persistence
{
    public interface IDBCommandLogger
    {
        void Log(Repository repository, string providerName, DbCommand command);
    }
    public class DBCommandLogger : IDBCommandLogger
    {
        public void Log(Repository repository, string providerName, DbCommand command)
        {
            var filePath = GetFileName(repository, providerName);
            IOUtility.EnsureDirectoryExists(Path.GetDirectoryName(filePath));
            File.AppendAllText(filePath, ToString(command));
        }
        private static string GetFileName(Repository repository, string providerName)
        {
            RepositoryPath path = new RepositoryPath(repository);
            return System.IO.Path.Combine(path.PhysicalPath, "Logs", string.Format("{0}-{1}.log", providerName, DateTime.Now.ToString("yyyyMMdd")));
        }
        private static string ToString(DbCommand command)
        {
            StringBuilder parameters = new StringBuilder();
            if (command.Parameters != null)
            {
                foreach (DbParameter parameter in command.Parameters)
                {
                    parameters.AppendFormat("{0}={1} ", parameter.ParameterName, parameter.Value);
                }
            }
            return string.Format(@"{0}
--{1}
--{2}
-----------------------------------------------------------------------------------------------------
", command.CommandText, parameters, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ff"));
        }
    }
}
