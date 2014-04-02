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
using System.IO;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IPagePublishingQueueProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<PagePublishingQueueItem>))]
    public class PagePublishingQueueProvider : SettingFileProviderBase<PagePublishingQueueItem>, IPagePublishingQueueProvider
    {
        #region GetLocker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion

        #region All
        public override IEnumerable<PagePublishingQueueItem> All(Site site)
        {
            return AllEnumerable(site);
        }

        private IEnumerable<PagePublishingQueueItem> AllEnumerable(Site site)
        {
            List<PagePublishingQueueItem> list = new List<PagePublishingQueueItem>();
            string baseDir = PagePublishingQueueItem.GetBasePath(site);
            if (Directory.Exists(baseDir))
            {
                foreach (var file in IO.IOUtility.EnumerateFilesExludeHidden(baseDir))
                {
                    list.Add(new PagePublishingQueueItem(site, Path.GetFileNameWithoutExtension(file.Name)));
                }
            }
            return list;
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
