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

namespace Kooboo.CMS.Content.Persistence.Mysql
{
    public class RepositoryProvider : Kooboo.CMS.Content.Persistence.Default.RepositoryProvider
    {
        public override void Add(Models.Repository item)
        {
            base.Add(item);
            DatabaseHelper.InitializeDatabase(item);
        }
        public override void Remove(Models.Repository item)
        {
            try
            {
                DatabaseHelper.DisposeDatabase(item);
            }
            catch
            {
            }

            base.Remove(item);
        }
        public override void Initialize(Models.Repository repository)
        {
            DatabaseHelper.InitializeDatabase(repository);
            base.Initialize(repository);
        }
        public override bool TestDbConnection()
        {            
            
            var shareConnectionString = MysqlSettings.Instance.ConnectionString;
            return MysqlHelper.TestConnection(shareConnectionString);
        }

    }
}
