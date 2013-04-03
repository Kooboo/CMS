using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Models;
using System.IO;

namespace Kooboo.CMS.Account.Persistence.FileSystem
{

    public class UserRepository : ObjectFileRepository<User>, IUserRepository
    {
        private static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        private string accountBaseDir = Path.Combine(Settings.BaseDirectory, "Cms_Data", "Account");
        public UserRepository(string accountBaseDir)
        {
            if (!string.IsNullOrEmpty(accountBaseDir))
            {
                this.accountBaseDir = accountBaseDir;
            }
        }
        public UserRepository()
        {
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
            return Path.Combine(accountBaseDir, "Users");
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
