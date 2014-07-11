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
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ISendingSettingProvider))]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<SendingSetting>))]
    public class SendingSettingProvider : FileSystemProviderBase<SendingSetting>, ISendingSettingProvider
    {
        #region All
        public IEnumerable<Models.SendingSetting> All(Models.Repository repository)
        {
            var path = new SendingSettingPath(repository);
            return IOUtility
                     .EnumerateFilesExludeHidden(path.PhysicalPath)
                     .Select(it => new SendingSetting
                     {
                         Repository = repository,
                         Name = Path.GetFileNameWithoutExtension(it.Name)
                     }).ToArray();

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
