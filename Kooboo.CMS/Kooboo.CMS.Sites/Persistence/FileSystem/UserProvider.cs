#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IUserProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<User>))]
    public class UserProvider : ObjectFileProviderBase<User>, IUserProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
               
        protected override User CreateObject(Site site, FileInfo fileInfo)
        {
            return new User() { Site = site, UserName = Path.GetFileNameWithoutExtension(fileInfo.Name) };
        }

        protected override string GetBasePath(Site site)
        {
            return User.DataFilePath.GetBasePath(site);
        }
    }
}
