using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Models;
using System.IO;

namespace Kooboo.CMS.Content.Persistence.Default
{
    public class SendingSettingProvider : FileSystemProviderBase<SendingSetting>, ISendingSettingProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        public IQueryable<Models.SendingSetting> All(Models.Repository repository)
        {
            var path = new SendingSettingPath(repository);
            return IO.IOUtility
                     .EnumerateFilesExludeHidden(path.PhysicalPath)
                     .Select(it => new SendingSetting
                     {
                         Repository = repository,
                         Name = Path.GetFileNameWithoutExtension(it.Name)
                     }).ToArray().AsQueryable();

        }


        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
    }
}
