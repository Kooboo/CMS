﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.SqlServer
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IRepositoryProvider), Order = 2)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(Kooboo.CMS.Common.Persistence.Non_Relational.IProvider<Repository>), Order = 2)]
    public class RepositoryProvider : Kooboo.CMS.Content.Persistence.Default.RepositoryProvider
    {
        public RepositoryProvider(IBaseDir baseDir)
            : base(baseDir) { }

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
            //Do not use sharing database, do not test the sharing connection string.
            //if (SqlServerSettings.Instance.SharingDatabase == false)
            //{
            //    return true;
            //}
            var shareConnectionString = SqlServerSettings.Instance.SharingDatabaseConnectionString;
            return SQLServerHelper.TestConnection(shareConnectionString);
        }

    }
}
