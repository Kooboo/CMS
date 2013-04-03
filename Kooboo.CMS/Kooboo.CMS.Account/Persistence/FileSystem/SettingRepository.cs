using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Models;
using System.IO;

namespace Kooboo.CMS.Account.Persistence.FileSystem
{
    public class SettingRepository : ObjectFileRepository<Setting>, ISettingRepository
    {
        public static string AccountBaseDir = Path.Combine(Settings.BaseDirectory, "Cms_Data", "Account");
        private static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }

        protected override string GetFilePath(Setting o)
        {
            return Path.Combine(AccountBaseDir, "setting.config");
        }

        protected override string GetBasePath()
        {
            return AccountBaseDir;
        }

        protected override Setting CreateObject(string filePath)
        {
            throw new NotImplementedException();
        }
        public override void Update(Setting @new, Setting old)
        {
            string filePath = GetFilePath(@old);
            Save(@new, filePath);
        }
        public Setting Get()
        {
            var setting = Get(null);
            if (setting == null)
            {
                setting = new Setting()
                {
                    EnableLockout = true,
                    FailedPasswordAttemptCount = 5,
                    MinutesToUnlock = 15,
                    PasswordInvalidMessage = "The password is invalid.",
                    PasswordStrength = ".{5,}"
                };
            }
            return setting;
        }
    }
}
