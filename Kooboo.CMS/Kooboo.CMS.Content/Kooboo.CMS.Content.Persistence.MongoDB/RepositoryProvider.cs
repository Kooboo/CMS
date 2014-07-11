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
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Content.Persistence.MongoDB
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IRepositoryProvider), Order = 2)]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(Kooboo.CMS.Common.Persistence.Non_Relational.IProvider<Repository>), Order = 2)]
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
            var server = DatabaseHelper.GetServer();
            server.Connect();
            return server.State == global::MongoDB.Driver.MongoServerState.Connected;
        }
    }
}
