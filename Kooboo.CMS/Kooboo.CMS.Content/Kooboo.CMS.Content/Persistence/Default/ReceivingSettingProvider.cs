using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;

namespace Kooboo.CMS.Content.Persistence.Default
{
    public class ReceivingSettingProvider : FileSystemProviderBase<ReceivingSetting>, IReceivingSettingProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        public IQueryable<Models.ReceivingSetting> All(Models.Repository repository)
        {
            var path = new ReceivingSettingPath(repository);
            return IO.IOUtility
                     .EnumerateFilesExludeHidden(path.PhysicalPath)
                     .Select(it => new ReceivingSetting
                     {
                         Repository = repository,
                         Name = Path.GetFileNameWithoutExtension(it.Name)
                     })
                     .ToArray().AsQueryable();

        }

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
    }
}
