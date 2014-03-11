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

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IViewProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Models.View>))]
    public class ViewProvider : TemplateProvider<Kooboo.CMS.Sites.Models.View>, IViewProvider
    {
        #region ViewVersionLogger
        public class ViewVersionLogger : TemplateProvider<Kooboo.CMS.Sites.Models.View>.TemplateVersionLogger
        {
            static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
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

            protected override System.Threading.ReaderWriterLockSlim GetLocker()
            {
                return locker;
            }
        }
        #endregion

        #region KnownTypes
        protected override IEnumerable<Type> KnownTypes
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
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion

        #region Copy
        public Models.View Copy(Site site, string sourceName, string destName)
        {
            GetLocker().EnterWriteLock();

            try
            {
                var sourceView = new Models.View(site, sourceName).LastVersion();
                var destView = new Models.View(site, destName);

                IOUtility.CopyDirectory(sourceView.PhysicalPath, destView.PhysicalPath);

                return destView;
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }

        }
        #endregion

        #region InitializeToDB/ExportToDisk
        public void InitializeToDB(Site site)
        {
            //
        }

        public void ExportToDisk(Site site)
        {
            //
        }
        #endregion

    }
}
