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
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ICustomErrorProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<CustomError>))]
    public class CustomErrorProvider : FileProviderBase<CustomError>, ICustomErrorProvider
    {
        #region GetLocker
        static System.Threading.ReaderWriterLockSlim @lock = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);
        //protected override System.Threading.ReaderWriterLockSlim GetLocker()
        //{
        //    return @lock;
        //}
        #endregion

        #region GetFile
        private string GetFile(Site site)
        {
            return Path.Combine(site.PhysicalPath, "CustomErrors.config");
        }
        #endregion  

        #region GetFileStorage
        protected override Storage.IFileStorage<CustomError> GetFileStorage(Site site)
        {
            return new XmlListFileStorage<CustomError>(GetFile(site), @lock);
        }
        #endregion
    }
}
