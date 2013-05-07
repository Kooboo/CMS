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
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Content.Persistence.Default;
using System.IO;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Search.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISearchSettingProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<SearchSetting>))]
    public class SearchSettingProvider : FileSystemProviderBase<SearchSetting>, ISearchSettingProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        public IEnumerable<SearchSetting> All(Content.Models.Repository repository)
        {
            var baseDir = SearchSetting.FolderSearchSettingPath.GetBasePhysicalPath(repository);
            List<SearchSetting> list = new List<SearchSetting>();

            if (Directory.Exists(baseDir))
            {
                foreach (var file in IO.IOUtility.EnumerateFilesExludeHidden(baseDir, "*" + PathHelper.SettingFileExtension))
                {
                    list.Add(new SearchSetting(repository, Path.GetFileNameWithoutExtension(file.Name)));
                }
            }

            return list.AsQueryable();
        }

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
    }
}
