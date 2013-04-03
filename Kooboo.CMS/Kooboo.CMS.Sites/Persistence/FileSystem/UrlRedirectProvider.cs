using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public class UrlRedirectProvider : ListFileRepository<UrlRedirect>, IUrlRedirectProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);

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
                ImportHelper.ImportData<UrlRedirect>(site, this, UrlRedirectsFile.FileName, zipStream, @override);
            }
            finally
            {
                locker.ExitWriteLock();
            }

        }

        #endregion

        protected override string GetFile(Site site)
        {
            return new UrlRedirectsFile(site).PhysicalPath;
        }

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
    }
}
