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
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IUrlRedirectProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<UrlRedirect>))]
    public class UrlRedirectProvider : ListFileRepository<UrlRedirect>, IUrlRedirectProvider
    {
        #region GetLocker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        } 
        #endregion

        #region IImportable Members

        public void Export(Site site, System.IO.Stream outputStream)
        {
            locker.EnterReadLock();
            try
            {
                ImportHelper.Export(new[] { new UrlRedirectsFile(site) }, outputStream);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            locker.EnterWriteLock();
            try
            {
                ImportHelper.ImportData<UrlRedirect>(site, this, UrlRedirectsFile.UrlRedirectFileName, zipStream, @override);
            }
            finally
            {
                locker.ExitWriteLock();
            }

        }

        #endregion

        #region GetFile
        protected override string GetFile(Site site)
        {
            return new UrlRedirectsFile(site).PhysicalPath;
        } 
        #endregion       
    
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
    }
}
