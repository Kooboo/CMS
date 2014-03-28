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
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IUrlKeyMapProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<UrlKeyMap>))]
    public class UrlKeyMapProvider : FileProviderBase<UrlKeyMap>, IUrlKeyMapProvider
    {
        #region lock
        static System.Threading.ReaderWriterLockSlim @lock = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);
        //protected override System.Threading.ReaderWriterLockSlim GetLocker()
        //{
        //    return locker;
        //}
        #endregion

        #region GetFile
        private string GetDataFile(Site site)
        {
            return Path.Combine(site.PhysicalPath, "UrlKeyMaps.config");
        }
        #endregion

        protected override IFileStorage<UrlKeyMap> GetFileStorage(Site site)
        {
            return new XmlListFileStorage<UrlKeyMap>(GetDataFile(site), @lock);
        }
    }
}
