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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.UserProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IUserProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<User>), Order = 100)]
    public class UserProvider : IUserProvider
    {
        #region .ctor
        SiteDBContext _dbContext;
        public UserProvider(SiteDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region All
        public IEnumerable<Models.User> All(Models.Site site)
        {
            return _dbContext.SiteUsers
                        .Where(it => it.SiteName == site.FullName)
                        .ToArray()
                        .Select(it => SiteUserHelper.ToUser(it))
                        .AsQueryable();
        }
        #endregion

        #region Get
        public Models.User Get(Models.User dummy)
        {
            var entity = _dbContext.SiteUsers
                   .Where(it => it.SiteName == dummy.Site.FullName && it.UserName == dummy.UserName)
                   .FirstOrDefault();
            return entity == null ? null : SiteUserHelper.ToUser(entity);
        }
        #endregion

        #region Add/Update
        public void Add(Models.User item)
        {
            InsertOrUpdate(item, item);
        }
        private void InsertOrUpdate(Models.User @new, Models.User old)
        {
            var entity = _dbContext.SiteUsers
                  .Where(it => it.SiteName == @new.Site.FullName && it.UserName == @new.UserName)
                  .FirstOrDefault();

            if (entity != null)
            {
                entity = SiteUserHelper.ToEntity(@new, entity);
            }
            else
            {
                entity = entity = SiteUserHelper.ToEntity(@new, entity);
                _dbContext.SiteUsers.Add(entity);
            }

            _dbContext.SaveChanges();
        }

        public void Update(Models.User @new, Models.User old)
        {
            InsertOrUpdate(@new, old);
        }
        #endregion

        #region Remove
        public void Remove(Models.User item)
        {
            var entity = _dbContext.SiteUsers
                   .Where(it => it.SiteName == item.Site.FullName && it.UserName == item.UserName)
                   .FirstOrDefault();
            if (entity != null)
            {
                _dbContext.SiteUsers.Remove(entity);
                _dbContext.SaveChanges();
            }
        }
        #endregion

        #region All
        public IEnumerable<Models.User> All()
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
