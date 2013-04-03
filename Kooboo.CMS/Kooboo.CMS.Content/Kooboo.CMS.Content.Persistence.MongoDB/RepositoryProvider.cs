using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.MongoDB
{
    public class RepositoryProvider : Default.RepositoryProvider
    {
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
