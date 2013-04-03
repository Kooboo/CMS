using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Account.Persistence.Caching
{
    public class UserRepository : CacheObjectProviderBase<Models.User>, IUserRepository
    {
        IUserRepository _inner;
        public UserRepository(IUserRepository inner)
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

        public IQueryable<Models.User> All()
        {
            return _inner.All();
        }

        public Models.User Get(Models.User dummy)
        {
            return base.Get(dummy);
        }

        public void Add(Models.User item)
        {
            base.Add(item);
        }

        public void Update(Models.User @new, Models.User old)
        {
            base.Update(@new, old);
        }

        public void Remove(Models.User item)
        {
            base.Remove(item);
        }

        protected override string GetCacheKey(Models.User o)
        {
            return "CacheUser-Name:" + o.UserName;
        }
    }
}
