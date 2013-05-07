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
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Account.Persistence.FileSystem
{
    [Dependency(typeof(IUserProvider))]
    [Dependency(typeof(IProvider<User>))]
    public class UserProvider : ObjectFileRepository<User>, IUserProvider
    {
        private static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        private IAccountBaseDir accountBaseDir;
        public UserProvider(IAccountBaseDir accountBaseDir)
        {
            this.accountBaseDir = accountBaseDir;
        }

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }

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

        public override void Add(User item)
        {
            var r = Kooboo.Connect.UserServices.CreateUser(item.UserName, item.Password, item.Email);

            if (r == Connect.UserCreateStatus.Success)
            {
                item.Password = "******";
                base.Add(item);
            }
            else
            {
                throw new KoobooException("Create user failed, message:" + r.ToString());
            }

        }

        public bool ValidateUser(string userName, string password)
        {
            return Kooboo.Connect.UserServices.ValidateUser(userName, password) != null;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return Kooboo.Connect.UserServices.ChangePassword(userName, oldPassword, newPassword);
        }
        public bool ChangePassword(string userName, string newPassword)
        {
            return Kooboo.Connect.UserServices.ChangePassword(userName, newPassword);
        }
        public override void Remove(User item)
        {
            base.Remove(item);
            Kooboo.Connect.UserServices.Delete(new Kooboo.Connect.User() { Name = item.UserName });
        }



    }
}
