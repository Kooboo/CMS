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
using Kooboo.CMS.Sites.Versioning;
using Kooboo.IO;
using System.IO;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IHtmlBlockProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<HtmlBlock>))]
    public class HtmlBlockProvider : InheritableProviderBase<HtmlBlock>, IHtmlBlockProvider
    {
        #region Versioning
        public class HtmlBlockVersionLogger : FileVersionLogger<HtmlBlock>
        {
            static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
            public override void LogVersion(HtmlBlock o)
            {
                locker.EnterWriteLock();
                try
                {
                    VersionPath versionPath = new VersionPath(o, NextVersionId(o));
                    IOUtility.EnsureDirectoryExists(versionPath.PhysicalPath);
                    var versionDataFile = Path.Combine(versionPath.PhysicalPath, o.DataFileName);
                    HtmlBlockProvider provider = new HtmlBlockProvider();
                    provider.Serialize(o, versionDataFile);
                }
                finally
                {
                    locker.ExitWriteLock();
                }
            }

            public override HtmlBlock GetVersion(HtmlBlock o, int version)
            {
                VersionPath versionPath = new VersionPath(o, version);
                var versionDataFile = Path.Combine(versionPath.PhysicalPath, o.DataFileName);
                HtmlBlock htmlBlock = null;
                if (File.Exists(versionDataFile))
                {
                    HtmlBlockProvider provider = new HtmlBlockProvider();
                    htmlBlock = provider.Deserialize(o, versionDataFile);
                    ((IPersistable)htmlBlock).Init(o);
                }
                return htmlBlock;
            }

            public override void Revert(HtmlBlock o, int version, string userName)
            {
                var versionData = GetVersion(o, version);
                if (versionData != null)
                {
                    versionData.UserName = userName;
                    versionData.LastUpdateDate = DateTime.UtcNow;
                    Providers.HtmlBlockProvider.Update(versionData, o);
                    //log a new version when revert
                    LogVersion(versionData);
                }
            }
        }
        #endregion

        #region IHtmlBlockProvider
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        private string GetSettingFile(HtmlBlock item)
        {
            return Path.Combine(item.PhysicalPath, "setting.config");
        }
        protected override void Serialize(HtmlBlock item, string filePath)
        {
            //以前HtmlBlock没有保存setting.config，导致像Username等信息没有正确保存，

            var basePath = Path.GetDirectoryName(filePath);
            var settingFile = Path.Combine(basePath, "setting.config");

            Serialization.Serialize(item, KnownTypes, settingFile);

            IO.IOUtility.SaveStringToFile(filePath, item.Body);
        }
        protected override HtmlBlock Deserialize(HtmlBlock dummy, string filePath)
        {
            var basePath = Path.GetDirectoryName(filePath);
            var settingFile = Path.Combine(basePath, "setting.config");

            if (File.Exists(settingFile))
            {
                //直接从setting.config中读取所有信息。
                var o = (HtmlBlock)Serialization.Deserialize(dummy.GetType(), KnownTypes, settingFile);
                return o;
            }
            else
            {
                //兼容以前只有Body.html的读取方式
                if (File.Exists(filePath))
                {
                    dummy.Body = IO.IOUtility.ReadAsString(filePath);
                    return dummy;
                }
            }
            return null;
        }

        public void Localize(HtmlBlock o, Site targetSite)
        {
            ILocalizableHelper.Localize(o, targetSite);
        }

        public void InitializeHtmlBlocks(Site site)
        {
            // no need to do anything.
        }

        public void ExportHtmlBlocksToDisk(Site site)
        {
            // no need to do anything.
        }


        public void Clear(Site site)
        {
            // no need to do anything.
        }


        public void Export(IEnumerable<HtmlBlock> sources, Stream outputStream)
        {
            ImportHelper.Export(sources.OfType<HtmlBlock>(), outputStream);
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            GetLocker().EnterWriteLock();
            try
            {
                var destDir = ModelActivatorFactory<HtmlBlock>.GetActivator().CreateDummy(site).BasePhysicalPath;
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
            //not need to implement.
        }

        public void ExportToDisk(Site site)
        {
            //not need to implement.
        }
        #endregion
    }
}
