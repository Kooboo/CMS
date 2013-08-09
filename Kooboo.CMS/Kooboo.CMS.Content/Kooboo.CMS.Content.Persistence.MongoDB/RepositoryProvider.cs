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
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Content.Persistence.MongoDB
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IRepositoryProvider), Order = 2)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(Kooboo.CMS.Common.Persistence.Non_Relational.IProvider<Repository>), Order = 2)]
    public class RepositoryProvider : Default.RepositoryProvider
    {
        public RepositoryProvider(IBaseDir baseDir)
            : base(baseDir) { }

        public override void Initialize(Models.Repository repository)
        {
            base.Initialize(repository);
            repository.CreateCateogryIndex();
        }

        public override void Remove(Models.Repository item)
        {
            base.Remove(item);
            item.DropDatabase();
        }

        public override bool TestDbConnection()
        {
            var server = DatabaseHelper.Connect();
            return server.State == global::MongoDB.Driver.MongoServerState.Connected;
        }
    }
}
