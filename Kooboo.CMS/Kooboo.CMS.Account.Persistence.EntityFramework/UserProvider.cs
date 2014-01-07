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
using Kooboo.CMS.Account.Persistence;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Account.Persistence.EntityFramework
{
    [Dependency(typeof(IUserProvider), ComponentLifeStyle.InRequestScope, Order = 100)]
    [Dependency(typeof(IProvider<User>), ComponentLifeStyle.InRequestScope, Order = 100)]
    public class UserProvider : ProviderBase<User>, IUserProvider
    {
        #region .ctor
        AccountDBContext _dbContext;
        public UserProvider(AccountDBContext dbContext)
            : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        #endregion

        public override IEnumerable<User> All()
        {
            return base.All();
        }

        public override void Update(Models.User @new, Models.User old)
        {
            var update = Get(old);
            update.Password = @new.Password;
            update.IsAdministrator = @new.IsAdministrator;
            update.CustomFields = @new.CustomFields;
            update.Email = @new.Email;
            update.UICulture = @new.UICulture;

            _dbContext.SaveChanges();
        }

        public override User Get(User dummy)
        {
            return _dbContext.Users.Where(o => o.UserName == dummy.UserName).FirstOrDefault();
        }

        public User FindUserByEmail(string email)
        {
            return _dbContext.Users.Where(o => o.Email == email).FirstOrDefault();
        }
    }
}
