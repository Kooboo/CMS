#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using System.ComponentModel.Composition;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.IO;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;
using System.IO;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IViewProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Models.View>))]
    public class ViewProvider : TemplateProvider<Kooboo.CMS.Sites.Models.View>, IViewProvider
    {
        #region ViewVersionLogger
        public class ViewVersionLogger : TemplateProvider<Kooboo.CMS.Sites.Models.View>.TemplateVersionLogger
        {
            static System.Threading.ReaderWriterLockSlim _lock = new System.Threading.ReaderWriterLockSlim();
            protected override TemplateProvider<Kooboo.CMS.Sites.Models.View> GetTemplateProvider()
            {
                return new ViewProvider();
            }
            public override void Revert(Models.View o, int version)
            {
                var versionData = GetVersion(o, version);
                if (versionData != null)
                {
                    Providers.ViewProvider.Update(versionData, o);
                }
            }

            protected override System.Threading.ReaderWriterLockSlim GetLock()
            {
                return _lock;
            }
        }
        #endregion

        #region KnownTypes
        private IEnumerable<Type> KnownTypes
        {
            get
            {
                return new Type[]{
                typeof(DataRuleBase),
                typeof(FolderDataRule),
                typeof(SchemaDataRule),
                typeof(CategoryDataRule),
                typeof(HttpDataRule)
                };
            }
        }
        #endregion

        #region Localize
        public void Localize(Models.View o, Site targetSite)
        {
            ILocalizableHelper.Localize<Models.View>(o, targetSite);
        }
        #endregion

        #region GetLocker
        static System.Threading.ReaderWriterLockSlim _lock = new System.Threading.ReaderWriterLockSlim();

        #endregion

        #region Copy
        public Models.View Copy(Site site, string sourceName, string destName)
        {
            _lock.EnterWriteLock();

            try
            {
                var sourceView = new Models.View(site, sourceName).LastVersion();
                var destView = new Models.View(site, destName);

                IOUtility.CopyDirectory(sourceView.PhysicalPath, destView.PhysicalPath);

                return destView;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        #endregion


        protected override string GetBasePath(Site site)
        {
            return Path.Combine(site.PhysicalPath, "Templates", "Views");
        }

        protected override IFileStorage<Models.View> GetFileStorage(Site site)
        {
            return new DirectoryObjectFileStorage<Models.View>(GetBasePath(site), _lock, KnownTypes, (dir) =>
            {
                return new Models.View(site, dir.Name);
            });
        }
    }
}
