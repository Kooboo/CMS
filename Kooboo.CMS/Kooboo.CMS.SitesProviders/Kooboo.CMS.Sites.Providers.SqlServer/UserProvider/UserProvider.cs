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

namespace Kooboo.CMS.Sites.Providers.SqlServer.UserProvider
{
    public class UserProvider : IUserProvider
    {
        #region CRUD
        public IEnumerable<Models.User> All(Models.Site site)
        {
            return SiteDbContext.CreateDbContext().SiteUsers
                        .Where(it => it.SiteName == site.FullName)
                        .ToArray()
                        .Select(it => SiteUserHelper.ToUser(it))
                        .AsQueryable();
        }

        public Models.User Get(Models.User dummy)
        {
            var entity = SiteDbContext.CreateDbContext().SiteUsers
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
            var dbContext = SiteDbContext.CreateDbContext();
            var entity = dbContext.SiteUsers
                  .Where(it => it.SiteName == @new.Site.FullName && it.UserName == @new.UserName)
                  .FirstOrDefault();

            if (entity != null)
            {
                entity = SiteUserHelper.ToEntity(@new, entity);
            }
            else
            {
                entity = entity = SiteUserHelper.ToEntity(@new, entity);
                dbContext.SiteUsers.Add(entity);
            }

            dbContext.SaveChanges();
        }

        public void Update(Models.User @new, Models.User old)
        {
            InsertOrUpdate(@new, old);
        }

        public void Remove(Models.User item)
        {
            var dbContext = SiteDbContext.CreateDbContext();
            var entity = dbContext.SiteUsers
                   .Where(it => it.SiteName == item.Site.FullName && it.UserName == item.UserName)
                   .FirstOrDefault();
            if (entity != null)
            {
                dbContext.SiteUsers.Remove(entity);
                dbContext.SaveChanges();
            }
        }

        public IEnumerable<Models.User> All()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public void InitializeToDB(Models.Site site)
        {
            //
        }

        public void ExportToDisk(Models.Site site)
        {
            //
        } 
        #endregion
    }
}
