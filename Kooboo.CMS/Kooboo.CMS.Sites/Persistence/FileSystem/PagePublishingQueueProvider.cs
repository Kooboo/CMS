using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using System.IO;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public class PagePublishingQueueProvider : ObjectFileProvider<PagePublishingQueueItem>, IPagePublishingQueueProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        public override IQueryable<PagePublishingQueueItem> All(Site site)
        {
            return AllEnumerable(site).AsQueryable();
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
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
    }
}
