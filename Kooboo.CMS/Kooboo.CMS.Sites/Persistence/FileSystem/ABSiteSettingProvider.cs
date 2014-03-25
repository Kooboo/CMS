using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;
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
    public class ABSiteSettingProvider : IABSiteSettingProvider
    {
        #region .ctor
        IBaseDir _baseDir;
        const string DIRNAME = "ABSiteSettings";
        static System.Threading.ReaderWriterLockSlim @lock = new System.Threading.ReaderWriterLockSlim();
        public ABSiteSettingProvider(IBaseDir baseDir)
        {
            _baseDir = baseDir;
        }
        #endregion

        //#region CreateObject
        //protected override ABSiteSetting CreateObject(Models.Site site, System.IO.FileInfo fileInfo)
        //{
        //    return new ABSiteSetting() { MainSite = Path.GetFileNameWithoutExtension(fileInfo.Name) };
        //}
        //#endregion

        //#region GetBasePath
        //protected override string GetBasePath(Models.Site site)
        //{
        //    var basePath = "";
        //    if (site == null)
        //    {
        //        basePath = Path.Combine(_baseDir.Cms_DataPhysicalPath, DIRNAME);
        //    }
        //    else
        //    {
        //        basePath = Path.Combine(site.PhysicalPath, DIRNAME);
        //    }
        //    return basePath;
        //}
        //#endregion

        //#region GetLocker
        //protected override System.Threading.ReaderWriterLockSlim GetLocker()
        //{
        //    return @lock;
        //}
        //#endregion

        #region All
        public virtual IEnumerable<ABSiteSetting> All()
        {
            var fileStorage = GetFileStorage();

            return fileStorage.GetList();
        }
        #endregion

        #region Import
        public virtual void Import(Stream zipStream, bool @override)
        {
            //base.Import(null, zipStream, @override);
        }
        #endregion

        #region Export
        public void Export(IEnumerable<ABSiteSetting> sources, Stream outputStream)
        {
            throw new NotImplementedException();
        }
        #endregion

        private IFileStorage<ABSiteSetting> GetFileStorage()
        {
            var basePath = Path.Combine(_baseDir.Cms_DataPhysicalPath, DIRNAME);

            return new XmlObjectFileStorage<ABSiteSetting>(basePath, @lock);
        }

        #region Get
        public virtual ABSiteSetting Get(ABSiteSetting dummy)
        {
            var fileStorage = GetFileStorage();

            return fileStorage.Get(dummy);
        }
        #endregion

        #region Add
        public virtual void Add(ABSiteSetting item)
        {
            var fileStorage = GetFileStorage();

            fileStorage.Add(item);
        }
        #endregion

        #region Update
        public virtual void Update(ABSiteSetting @new, ABSiteSetting old)
        {
            var fileStorage = GetFileStorage();

            fileStorage.Update(@new, old);
        }
        #endregion

        #region Remove
        public virtual void Remove(ABSiteSetting item)
        {
            var fileStorage = GetFileStorage();

            fileStorage.Remove(item);
        }
        #endregion

    }
}
