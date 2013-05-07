using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.ABTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IABSiteSettingProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<ABSiteSetting>))]
    public class ABSiteSettingProvider : ObjectFileProviderBase<ABSiteSetting>, IABSiteSettingProvider
    {
        #region .ctor
        IBaseDir _baseDir;
        const string DIRNAME = "ABSiteSettings";
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        public ABSiteSettingProvider(IBaseDir baseDir)
        {
            _baseDir = baseDir;
        }
        #endregion

        #region CreateObject
        protected override ABSiteSetting CreateObject(Models.Site site, System.IO.FileInfo fileInfo)
        {
            return new ABSiteSetting() { MainSite = Path.GetFileNameWithoutExtension(fileInfo.Name) };
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
        public override IEnumerable<ABSiteSetting> All()
        {
            return base.All(null);
        }
        #endregion

        #region Import
        public virtual void Import(Stream zipStream, bool @override)
        {
            base.Import(null, zipStream, @override);
        }
        #endregion
    }
}
