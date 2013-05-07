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
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Account.Persistence.FileSystem
{
    [Dependency(typeof(ISettingProvider))]
    [Dependency(typeof(IProvider<Setting>))]
    public class SettingProvider : ObjectFileRepository<Setting>, ISettingProvider
    {

        private IAccountBaseDir accountBaseDir;
        private static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        public SettingProvider(IAccountBaseDir baseDir)
        {
            accountBaseDir = baseDir;
        }
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }

        protected override string GetFilePath(Setting o)
        {
            return Path.Combine(accountBaseDir.PhysicalPath, "setting.config");
        }

        protected override string GetBasePath()
        {
            return accountBaseDir.PhysicalPath;
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
