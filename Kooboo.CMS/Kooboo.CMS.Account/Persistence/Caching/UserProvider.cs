#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Account.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Account.Persistence.Caching
{
    public class UserProvider : CacheObjectProviderBase<User>, IUserProvider
    {
        IUserProvider _inner;
        public UserProvider(IUserProvider inner)
            : base(inner)
        {
            _inner = inner;
        }
        public bool ValidateUser(string userName, string password)
        {
            return _inner.ValidateUser(userName, password);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return _inner.ChangePassword(userName, oldPassword, newPassword);
        }

        public bool ChangePassword(string userName, string newPassword)
        {
            return _inner.ChangePassword(userName, newPassword);
        }

        public IEnumerable<Models.User> All()
        {
            return _inner.All();
        }

        public  override User Get(User dummy)
        {
            return base.Get(dummy);
        }

        public override void Add(User item)
        {
            base.Add(item);
        }

        public override void Update(User @new, User old)
        {
            base.Update(@new, old);
        }

        public override void Remove(User item)
        {
            base.Remove(item);
        }

        protected override string GetCacheKey(User o)
        {
            return "CacheUser-Name:" + o.UserName;
        }
    }
}
