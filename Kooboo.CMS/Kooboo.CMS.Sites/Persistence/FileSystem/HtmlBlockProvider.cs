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
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IHtmlBlockProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<HtmlBlock>))]
    public class HtmlBlockProvider : InheritableProviderBase<HtmlBlock>, IHtmlBlockProvider
    {
        static string BodyFileName = "Body.html";
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
                    var versionDataFile = Path.Combine(versionPath.PhysicalPath, "setting.config");
                    HtmlBlockProvider provider = new HtmlBlockProvider();
                    Kooboo.Runtime.Serialization.DataContractSerializationHelper.Serialize(o, versionDataFile, null);
                }
                finally
                {
                    locker.ExitWriteLock();
                }
            }

            public override HtmlBlock GetVersion(HtmlBlock o, int version)
            {
                VersionPath versionPath = new VersionPath(o, version);
                var versionDataFile = Path.Combine(versionPath.PhysicalPath, "setting.config");
                HtmlBlock versionItem = null;
                if (File.Exists(versionDataFile))
                {
                    versionItem = (HtmlBlock)Kooboo.Runtime.Serialization.DataContractSerializationHelper.Deserialize(typeof(HtmlBlock), null, versionDataFile);
                    ((IPersistable)versionItem).Init(o);
                }
                return versionItem;
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
        static System.Threading.ReaderWriterLockSlim _lock = new System.Threading.ReaderWriterLockSlim();

        //protected override void Serialize(HtmlBlock item, string filePath)
        //{
        //    IO.IOUtility.SaveStringToFile(filePath, item.Body);
        //}
        //protected override HtmlBlock Deserialize(HtmlBlock dummy, string filePath)
        //{
        //    dummy.Body = IO.IOUtility.ReadAsString(filePath);
        //    return dummy;
        //}

        private static HtmlBlock GetObject(HtmlBlock dummy)
        {
            var filePath = GetBodyFilePath(dummy);
            if (File.Exists(filePath))
            {
                dummy.Body = IO.IOUtility.ReadAsString(filePath);
                ((IPersistable)dummy).Init(dummy);
                return dummy;
            }
            return null;
        }

        private static void SaveBody(HtmlBlock item)
        {
            var filePath = GetBodyFilePath(item);
            IO.IOUtility.SaveStringToFile(filePath, item.Body);
        }

        public override HtmlBlock Get(HtmlBlock dummy)
        {
            var o = base.Get(dummy);
            if (o == null)
            {
                //get the body from Body.html. 
                o = GetObject(dummy);
            }
            return o;
        }
        public override void Add(HtmlBlock item)
        {
            base.Add(item);
            //not need in the future, because the body has been saved in the setting.config. 2014/04/16重构,保存整个对象,而不是只保存Body内容.这样可以保存更多的字段值.
            SaveBody(item);
        }
        public override void Update(HtmlBlock @new, HtmlBlock old)
        {
            base.Update(@new, old);
            //
            SaveBody(@new);
        }
        #region Localize

        public void Localize(HtmlBlock o, Site targetSite)
        {
            ILocalizableHelper.Localize(o, targetSite);
        }
        #endregion

        #region Clear
        public void Clear(Site site)
        {
            // no need to do anything.
        }
        #endregion

        #endregion
        private static string GetBodyFilePath(HtmlBlock dummy)
        {
            return Path.Combine(GetBasePath(dummy.Site), dummy.Name, BodyFileName);
        }
        private static string GetBasePath(Site site)
        {
            return Path.Combine(site.PhysicalPath, "HtmlBlocks");
        }
        protected override IFileStorage<HtmlBlock> GetFileStorage(Site site)
        {
            return new DirectoryObjectFileStorage<HtmlBlock>(GetBasePath(site), _lock, new Type[0], (dir) =>
            {
                return new HtmlBlock(site, dir.Name);
            });
        }
    }
}
