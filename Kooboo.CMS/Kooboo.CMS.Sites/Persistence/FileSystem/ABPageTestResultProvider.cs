using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.ABTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IABPageTestResultProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<ABPageTestResult>))]
    public class ABPageTestResultProvider : ObjectFileProviderBase<ABPageTestResult>, IABPageTestResultProvider
    {
        #region .ctor
        const string DIRNAME = "HitResults";
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        public ABPageTestResultProvider()
        {
        }
        #endregion

        #region CreateObject
        protected override ABPageTestResult CreateObject(Models.Site site, System.IO.FileInfo fileInfo)
        {
            return new ABPageTestResult() { Site = site, UUID = Path.GetFileNameWithoutExtension(fileInfo.Name) };
        }
        #endregion

        #region GetBasePath
        protected override string GetBasePath(Models.Site site)
        {
            var basePath = Path.Combine(site.PhysicalPath, ABPageSettingProvider.DIRNAME, DIRNAME);

            return basePath;
        }
        #endregion

        #region GetLocker
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion        
    
        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public void InitializeToDB(Site site)
        {
            //not need to implement.
        }

        public void ExportToDisk(Site site)
        {
            //not need to implement.
        }
        #endregion
    }
}
