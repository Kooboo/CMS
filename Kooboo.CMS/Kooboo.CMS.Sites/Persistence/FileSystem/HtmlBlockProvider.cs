using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.IO;
using System.IO;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
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

            public override void Revert(HtmlBlock o, int version)
            {
                var versionData = GetVersion(o, version);
                if (versionData != null)
                {
                    Providers.HtmlBlockProvider.Update(versionData, o);
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

        protected override void Serialize(HtmlBlock item, string filePath)
        {
            IO.IOUtility.SaveStringToFile(filePath, item.Body);
        }
        protected override HtmlBlock Deserialize(HtmlBlock dummy, string filePath)
        {
            dummy.Body = IO.IOUtility.ReadAsString(filePath);
            return dummy;
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
    }
}
