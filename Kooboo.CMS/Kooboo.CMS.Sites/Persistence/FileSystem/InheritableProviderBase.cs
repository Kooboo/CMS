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
using Kooboo.IO;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public abstract class InheritableProviderBase<T> : SettingFileProviderBase<T>
        where T : PathResource, ISiteObject, IFilePersistable, IPersistable, IIdentifiable, IInheritable<T>
    {
        public override IEnumerable<T> All(Models.Site site)
        {
            return IInheritableHelper.All<T>(site);
        }

        public override void Update(T @new, T old)
        {
            if (!@new.Equals(old) && old.Exists())
            {
                old.Rename(@new.Name);
            }
            Save(@new);
        }
        public override void Remove(T item)
        {
            string dir = item.PhysicalPath;
            GetLocker().EnterWriteLock();
            try
            {

                IOUtility.DeleteDirectory(dir, true);

            }
            finally
            {
                GetLocker().ExitWriteLock();
            }
        }
    }
}
