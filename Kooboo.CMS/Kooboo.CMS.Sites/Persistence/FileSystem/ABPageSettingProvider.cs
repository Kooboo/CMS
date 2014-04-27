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
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IABPageSettingProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<ABPageSetting>))]
    public class ABPageSettingProvider : ObjectFileProviderBase<ABPageSetting>, IABPageSettingProvider
    {
        #region .ctor
        public const string DIRNAME = "ABPageSettings";
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        public ABPageSettingProvider()
        {
        }
        #endregion

        #region CreateObject
        protected override ABPageSetting CreateObject(Models.Site site, System.IO.FileInfo fileInfo)
        {
            return new ABPageSetting() { Site = site, MainPage = Path.GetFileNameWithoutExtension(fileInfo.Name) };
        }
        #endregion

        #region GetBasePath
        protected override string GetBasePath(Models.Site site)
        {
            var basePath = Path.Combine(site.PhysicalPath, DIRNAME);

            return basePath;
        }
        #endregion

        #region GetLocker
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion

        #region Import
        void IABPageSettingProvider.Import(Site site, Stream zipStream, bool @override)
        {
            base.Import(site, zipStream, @override);
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
