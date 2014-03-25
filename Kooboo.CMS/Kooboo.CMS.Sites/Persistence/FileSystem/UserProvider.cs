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
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IUserProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<User>))]
    public class UserProvider : FileProviderBase<User>, IUserProvider
    {
        #region Locker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        //protected override System.Threading.ReaderWriterLockSlim GetLocker()
        //{
        //    return locker;
        //}
        #endregion

        //#region CreateObject
        //protected override User CreateObject(Site site, FileInfo fileInfo)
        //{
        //    return new User() { Site = site, UserName = Path.GetFileNameWithoutExtension(fileInfo.Name) };
        //}
        //#endregion

        //#region GetBasePath
        //protected override string GetBasePath(Site site)
        //{
        //    return User.DataFilePath.GetBasePath(site);
        //}
        //#endregion


        #region ExportToDisk/InitializeToDB
        public void InitializeToDB(Site site)
        {
            //
        }

        public void ExportToDisk(Site site)
        {
            //
        }
        #endregion

        protected override IFileStorage<User> GetFileStorage(Site site)
        {
            return new XmlObjectFileStorage<User>(User.DataFilePath.GetBasePath(site), locker);
        }
    }
}
