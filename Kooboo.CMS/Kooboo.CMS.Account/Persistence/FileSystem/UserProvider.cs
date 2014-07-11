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
using Kooboo.CMS.Account.Models;
using System.IO;
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Account.Persistence.FileSystem
{
    [Dependency(typeof(IUserProvider))]
    [Dependency(typeof(IProvider<User>))]
    public class UserProvider : ObjectFileRepository<User>, IUserProvider
    {
        #region .ctor
        private static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        private IAccountBaseDir accountBaseDir;
        public UserProvider(IAccountBaseDir accountBaseDir)
        {
            this.accountBaseDir = accountBaseDir;
        }
        #endregion

        #region GetLocker
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion

        #region Override methods
        protected override string GetFilePath(User o)
        {
            return Path.Combine(GetBasePath(), o.UserName + ".config");
        }

        protected override string GetBasePath()
        {
            return Path.Combine(accountBaseDir.PhysicalPath, "Users");
        }

        protected override User CreateObject(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            return new User() { UserName = fileName };
        }

        #endregion

        public override User Get(User dummy)
        {
            var user = base.Get(dummy);
            //The old account data. get the password from the old connect provider
            if (user != null && (string.IsNullOrEmpty(user.Password) || user.Password == "******"))
            {
                var connectUser = (new Kooboo.Connect.FileProvider()).LoadUser(user.UserName);
                if (connectUser != null)
                {
                    user.Password = connectUser.Membership.Password;
                    user.PasswordSalt = connectUser.Membership.PasswordSalt;
                }
            }
            return user;
        }


        public User FindUserByEmail(string email)
        {
            return All().Select(it => Get(it)).Where(it => it.Email.EqualsOrNullEmpty(email, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }
    }
}
