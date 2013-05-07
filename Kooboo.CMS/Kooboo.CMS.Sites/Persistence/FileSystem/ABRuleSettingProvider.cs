using Ionic.Zip;
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
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IABRuleSettingProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<ABRuleSetting>))]
    public class ABRuleSettingProvider : ObjectFileProviderBase<ABRuleSetting>, IABRuleSettingProvider
    {
        #region KnownTypes
        protected override IEnumerable<Type> KnownTypes
        {
            get
            {
                return Kooboo.CMS.Common.Runtime.EngineContext.Current.ResolveAll<IVisitRule>().Select(it => it.GetType());
            }
        }
        #endregion

        #region .ctor
        IBaseDir _baseDir;
        const string DIRNAME = "ABRuleSettings";
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        public ABRuleSettingProvider(IBaseDir baseDir)
        {
            _baseDir = baseDir;
        }
        #endregion

        #region CreateObject
        protected override ABRuleSetting CreateObject(Models.Site site, System.IO.FileInfo fileInfo)
        {
            return new ABRuleSetting() { Site = site, Name = Path.GetFileNameWithoutExtension(fileInfo.Name) };
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
        public override IEnumerable<ABRuleSetting> All()
        {
            return base.All(null);
        }
        #endregion
    }
}
