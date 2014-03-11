using Ionic.Zip;
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
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISubmissionSettingProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<SubmissionSetting>))]
    public class SubmissionSettingProvider : ObjectFileProviderBase<SubmissionSetting>, ISubmissionSettingProvider
    {

        #region .ctor
        IBaseDir _baseDir;
        const string DIRNAME = "SubmissionSettings";
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        public SubmissionSettingProvider(IBaseDir baseDir)
        {
            _baseDir = baseDir;
        }
        #endregion

        #region CreateObject
        protected override SubmissionSetting CreateObject(Models.Site site, System.IO.FileInfo fileInfo)
        {
            return new SubmissionSetting() { Site = site, Name = Path.GetFileNameWithoutExtension(fileInfo.Name) };
        }
        #endregion

        #region GetBasePath
        protected override string GetBasePath(Models.Site site)
        {
            var basePath = "";
            if (site == null)
            {
                basePath = Path.Combine(_baseDir.Cms_DataPhysicalPath, DIRNAME);
            }
            else
            {
                basePath = Path.Combine(site.PhysicalPath, DIRNAME);
            }
            return basePath;
        }
        #endregion

        #region GetLocker
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion

        #region All
        public override IEnumerable<SubmissionSetting> All()
        {
            throw new NotSupportedException();
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
