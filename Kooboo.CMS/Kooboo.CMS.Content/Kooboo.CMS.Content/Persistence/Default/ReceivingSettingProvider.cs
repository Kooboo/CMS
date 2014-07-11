#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Default
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IReceivingSettingProvider))]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<ReceivingSetting>))]
    public class ReceivingSettingProvider : FileSystemProviderBase<ReceivingSetting>, IReceivingSettingProvider
    {
        #region All
        public IEnumerable<Models.ReceivingSetting> All(Models.Repository repository)
        {
            var path = new ReceivingSettingPath(repository);
            return IOUtility
                     .EnumerateFilesExludeHidden(path.PhysicalPath)
                     .Select(it => new ReceivingSetting
                     {
                         Repository = repository,
                         Name = Path.GetFileNameWithoutExtension(it.Name)
                     })
                     .ToArray().AsQueryable();

        } 
        #endregion

        #region GetLocker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        } 
        #endregion
    }
}
