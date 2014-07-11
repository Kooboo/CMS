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
using Kooboo.Common.ObjectContainer;
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Account.Persistence.FileSystem
{
    [Dependency(typeof(IRoleProvider))]
    [Dependency(typeof(IProvider<Role>))]
    public class RoleProvider : ObjectFileRepository<Role>, IRoleProvider
    {
        private static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        private IAccountBaseDir accountBaseDir;
        public RoleProvider(IAccountBaseDir baseDir)
        {
            accountBaseDir = baseDir;
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
            return Path.Combine(accountBaseDir.PhysicalPath, "Roles");
        }

        protected override Role CreateObject(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            return new Role() { Name = fileName };
        }
    }
}
