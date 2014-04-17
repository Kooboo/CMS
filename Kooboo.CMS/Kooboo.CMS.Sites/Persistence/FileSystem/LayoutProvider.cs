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
using System.IO;
using Kooboo.Web.Url;
using Kooboo.IO;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ILayoutProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Layout>))]
    public class LayoutProvider : TemplateProvider<Layout>, ILayoutProvider
    {
        public class LayoutVersionLogger : TemplateProvider<Layout>.TemplateVersionLogger
        {
            static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
            protected override TemplateProvider<Layout> GetTemplateProvider()
            {
                return new LayoutProvider();
            }
            public override void Revert(Layout o, int version, string userName)
            {
                var versionData = GetVersion(o, version);
                if (versionData != null)
                {
                    versionData.UserName = userName;
                    versionData.LastUpdateDate = DateTime.UtcNow;
                    Providers.LayoutProvider.Update(versionData, o);
                    //log a new version when revert
                    LogVersion(versionData);
                }
            }

            protected override System.Threading.ReaderWriterLockSlim GetLock()
            {
                return locker;
            }
        }
        static LayoutProvider()
        {

        }
        #region Layout

        public void Localize(Layout o, Site targetSite)
        {
            ILocalizableHelper.Localize<Layout>(o, targetSite);
        }
        static System.Threading.ReaderWriterLockSlim _lock = new System.Threading.ReaderWriterLockSlim();
        //protected override System.Threading.ReaderWriterLockSlim GetLocker()
        //{
        //    return locker;
        //}
        #endregion

        public Layout Copy(Site site, string sourceName, string destName)
        {
            _lock.EnterWriteLock();

            try
            {
                var sourceLayout = new Layout(site, sourceName).LastVersion();
                var destLayout = new Layout(site, destName);

                IOUtility.CopyDirectory(sourceLayout.PhysicalPath, destLayout.PhysicalPath);

                return destLayout;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        protected override string GetBasePath(Site site)
        {
            return Path.Combine(site.PhysicalPath, "Templates", "Layouts");
        }

        protected override IFileStorage<Layout> GetFileStorage(Site site)
        {
            return new DirectoryObjectFileStorage<Layout>(GetBasePath(site), _lock, new Type[0], (dir) =>
            {
                return new Layout(site, dir.Name);
            });
        }
    }
}
