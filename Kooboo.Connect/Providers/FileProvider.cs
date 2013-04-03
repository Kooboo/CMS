using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.Runtime.Serialization;

namespace Kooboo.Connect.Providers
{
    public class FileProvider : IDataProvider
    {
        public string BaseDir = Path.Combine(Kooboo.Settings.BaseDirectory, "Cms_Data", "Connect");

        public FileProvider()
        { }

        public FileProvider(string baseDir)
        {
            this.BaseDir = baseDir;
        }

        public string UserDir
        {
            get
            {
                return Path.Combine(BaseDir, "Users");
            }
        }

        //public string CustomerDir
        //{
        //    get
        //    {
        //        return Path.Combine(BaseDir, "Customers");
        //    }
        //}

        //public string ActivityDir
        //{
        //    get
        //    {
        //        return Path.Combine(BaseDir, "Activities");
        //    }
        //}

        private static System.Threading.ReaderWriterLockSlim userLocker = new System.Threading.ReaderWriterLockSlim();
        private static System.Threading.ReaderWriterLockSlim customerLocker = new System.Threading.ReaderWriterLockSlim();
        private static System.Threading.ReaderWriterLockSlim activityLocker = new System.Threading.ReaderWriterLockSlim();

        public IQueryable<Connect.User> LoadUsers()
        {
            return AllUsers().AsQueryable();
        }
        private IEnumerable<Connect.User> AllUsers()
        {
            if (Directory.Exists(UserDir))
            {
                foreach (var item in Directory.EnumerateFiles(UserDir))
                {
                    string fileName = Path.GetFileNameWithoutExtension(item);
                    yield return LoadUser(fileName);
                }
            }
        }

        public Connect.User LoadUser(string name)
        {
            string userfile = GetUserFilePath(name);
            if (File.Exists(userfile))
            {
                userLocker.EnterReadLock();
                try
                {
                    return DataContractSerializationHelper.Deserialize<Connect.User>(userfile);
                }
                finally
                {
                    userLocker.ExitReadLock();
                }

            }
            return null;
        }

        private string GetUserFilePath(string name)
        {
            return Path.Combine(UserDir, name + ".config");
        }

        public Connect.User LoadUserByMail(string mail)
        {
            return AllUsers().Where(it => string.Compare(it.Email, mail, true) == 0).FirstOrDefault();
        }

        public bool Save(Connect.User account)
        {
            if (string.IsNullOrEmpty(account.Name))
            {
                throw new ArgumentNullException("The user name can not be null");
            }
            string userFile = Path.Combine(UserDir, account.Name + ".config");
            userLocker.EnterWriteLock();
            try
            {
                DataContractSerializationHelper.Serialize(account, userFile);
            }
            finally
            {
                userLocker.ExitWriteLock();
            }

            return true;
        }

        public bool DeleteUser(Connect.User account)
        {
            string userfile = GetUserFilePath(account.Name);
            userLocker.EnterWriteLock();
            try
            {
                if (File.Exists(userfile))
                {
                    File.Delete(userfile);
                }
                return true;
            }
            finally
            {
                userLocker.ExitWriteLock();
            }
        }
    }
}
