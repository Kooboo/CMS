using Ionic.Zip;
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IABRuleSettingProvider))]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<ABRuleSetting>))]
    public class ABRuleSettingProvider : FileProviderBase<ABRuleSetting>, IABRuleSettingProvider
    {
        #region KnownTypes
        private IEnumerable<Type> KnownTypes
        {
            get
            {
                return Kooboo.Common.ObjectContainer.EngineContext.Current.ResolveAll<IVisitRule>().Select(it => it.GetType());
            }
        }
        #endregion

        #region .ctor
        IBaseDir _baseDir;
        const string DIRNAME = "ABRuleSettings";
        static System.Threading.ReaderWriterLockSlim @lock = new System.Threading.ReaderWriterLockSlim();
        public ABRuleSettingProvider(IBaseDir baseDir)
        {
            _baseDir = baseDir;
        }
        #endregion

        //#region CreateObject
        //protected override ABRuleSetting CreateObject(Models.Site site, System.IO.FileInfo fileInfo)
        //{
        //    return new ABRuleSetting() { Site = site, Name = Path.GetFileNameWithoutExtension(fileInfo.Name) };
        //}
        //#endregion

        #region GetBasePath
        private string GetBasePath(Site site)
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

        //#region GetLocker
        //protected override System.Threading.ReaderWriterLockSlim GetLocker()
        //{
        //    return locker;
        //}
        //#endregion

        #region All
        public override IEnumerable<ABRuleSetting> All()
        {
            return base.All(null);
        }
        #endregion

        protected override IFileStorage<ABRuleSetting> GetFileStorage(Site site)
        {
            return new XmlObjectFileStorage<ABRuleSetting>(GetBasePath(site), @lock, KnownTypes);
        }
    }
}
