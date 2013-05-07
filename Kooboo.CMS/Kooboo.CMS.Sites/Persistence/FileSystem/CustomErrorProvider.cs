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
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ICustomErrorProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<CustomError>))]
    public class CustomErrorProvider : ListFileRepository<CustomError>, ICustomErrorProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);

        #region IImportRepository Members

        public void Export(Site site, System.IO.Stream outputStream)
        {
            locker.EnterReadLock();
            try
            {
                ImportHelper.Export(new[] { new CustomErrorsFile(site) }, outputStream);
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
                ImportHelper.ImportData<CustomError>(site, this, CustomErrorsFile.CustomErrorFileName, zipStream, @override);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        #endregion

        protected override string GetFile(Site site)
        {
            return new CustomErrorsFile(site).PhysicalPath;
        }

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
    }
}
