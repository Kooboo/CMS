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
using Kooboo.CMS.Sites.Persistence;
using Microsoft.WindowsAzure.StorageClient;

namespace Kooboo.CMS.Sites.Providers.AzureTable.UserProvider
{
    public class UserProvider : IUserProvider
    {
        static string SiteUsersTable = "SiteUsers";
        static UserProvider()
        {
            CloudTableClient tableClient = CloudTableHelper.GetTableClient();

            tableClient.CreateTableIfNotExist<SiteUserEntity>(SiteUsersTable);

        }
        public IEnumerable<Models.User> All(Models.Site site)
        {
            return CloudTableHelper.GetTableServiceContext().CreateQuery<SiteUserEntity>(SiteUsersTable)
                           .Where(it => it.SiteName == site.FullName)
                           .ToArray()
                           .Select(it => SiteUserHelper.ToUser(it))
                           .AsQueryable();
        }

        public Models.User Get(Models.User dummy)
        {
            var entity = CloudTableHelper.GetTableServiceContext().CreateQuery<SiteUserEntity>(SiteUsersTable)
                   .Where(it => it.SiteName == dummy.Site.FullName && it.UserName == dummy.UserName)
                   .FirstOrDefault();
            return entity == null ? null : SiteUserHelper.ToUser(entity);
        }

        public void Add(Models.User item)
        {
            InsertOrUpdate(item, item);
        }
        private void InsertOrUpdate(Models.User @new, Models.User old)
        {
            var entity = SiteUserHelper.ToEntity(@new);
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            if (Get(old) == null)
            {
                serviceContext.AddObject(SiteUsersTable, entity);
            }
            else
            {
                serviceContext.AttachTo(SiteUsersTable, entity, "*");
                serviceContext.UpdateObject(entity);
            }
            serviceContext.SaveChangesWithRetries();
        }

        public void Update(Models.User @new, Models.User old)
        {
            InsertOrUpdate(@new, old);
        }

        public void Remove(Models.User item)
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();

            var entity = serviceContext.CreateQuery<SiteUserEntity>(SiteUsersTable)
                   .Where(it => it.PartitionKey == item.Site.FullName && it.RowKey == item.UserName)
                   .FirstOrDefault();
            if (entity != null)
            {
                serviceContext.DeleteObject(entity);
                serviceContext.SaveChangesWithRetries();
            }
        }

        public IEnumerable<Models.User> All()
        {
            throw new NotSupportedException();
        }
    }
}
