using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Models;
using System.IO;
using Kooboo.Runtime.Serialization;
namespace Kooboo.CMS.Account.Persistence.FileSystem
{
    public class RoleRepository : ObjectFileRepository<Role>, IRoleRepository
    {
        private static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        private string accountBaseDir = Path.Combine(Settings.BaseDirectory, "Cms_Data", "Account");
        public RoleRepository()
        {

        }
        public RoleRepository(string accountBaseDir)
        {
            if (!string.IsNullOrEmpty(accountBaseDir))
            {
                this.accountBaseDir = accountBaseDir;
            }
        }

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }

        protected override string GetFilePath(Role o)
        {
            return Path.Combine(GetBasePath(), o.Name + ".config");
        }

        protected override string GetBasePath()
        {
            return Path.Combine(accountBaseDir, "Roles");
        }

        protected override Role CreateObject(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            return new Role() { Name = fileName };
        }
    }
}
