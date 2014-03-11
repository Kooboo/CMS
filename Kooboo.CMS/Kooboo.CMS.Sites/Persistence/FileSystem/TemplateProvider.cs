#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public abstract class TemplateProvider<T> : InheritableProviderBase<T>, ISiteElementProvider<T>
        where T : Template, ISiteObject, IFilePersistable, IPersistable, IIdentifiable, IInheritable<T>
    {
        #region TemplateVersionLogger
        public abstract class TemplateVersionLogger : FileVersionLogger<T>
        {
            protected abstract TemplateProvider<T> GetTemplateProvider();
            public override void LogVersion(T o)
            {
                var locker = GetLocker();
                locker.EnterWriteLock();
                try
                {
                    VersionPath versionPath = new VersionPath(o, NextVersionId(o));
                    IOUtility.EnsureDirectoryExists(versionPath.PhysicalPath);
                    var versionDataFile = Path.Combine(versionPath.PhysicalPath, Path.GetFileName(o.DataFile));
                    var versionTemplateFile = Path.Combine(versionPath.PhysicalPath, Path.GetFileName(o.TemplateFileName));
                    File.Copy(o.DataFile, versionDataFile);
                    File.Copy(o.PhysicalTemplateFileName, versionTemplateFile);
                }
                finally
                {
                    locker.ExitWriteLock();
                }
            }

            public override T GetVersion(T o, int version)
            {
                o = o.AsActual();
                VersionPath versionPath = new VersionPath(o, version);
                var versionDataFile = Path.Combine(versionPath.PhysicalPath, Path.GetFileName(o.DataFile));
                var versionTemplateFile = Path.Combine(versionPath.PhysicalPath, Path.GetFileName(o.TemplateFileName));
                T template = null;
                if (File.Exists(versionDataFile))
                {
                    var provider = GetTemplateProvider();
                    template = provider.Deserialize(o, versionDataFile);
                    template.Body = IOUtility.ReadAsString(versionTemplateFile);
                    template.Init(o);
                }
                return template;
            }

            protected abstract System.Threading.ReaderWriterLockSlim GetLocker();
        }
        #endregion

        #region Get/Save
        public override T Get(T dummyObject)
        {
            if (!dummyObject.Exists())
            {
                return null;
            }
            var template = base.Get(dummyObject);
            template.Init(dummyObject);
            return template;
        }

        protected override void Save(T item)
        {
            base.Save(item);
        }

        #endregion

        #region Import
        public void Export(IEnumerable<T> sources, System.IO.Stream outputStream)
        {
            ImportHelper.Export(sources.OfType<PathResource>(), outputStream);
        }
        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            GetLocker().EnterWriteLock();
            try
            {
                var destDir = ModelActivatorFactory<T>.GetActivator().CreateDummy(site).BasePhysicalPath;
                ImportHelper.Import(site, destDir, zipStream, @override);
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }

        }
        #endregion


        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public void InitializeToDB(Site site)
        {
              //not need to implement
        }

        public void ExportToDisk(Site site)
        {
             //not need to implement
        } 
        #endregion
    }
}
