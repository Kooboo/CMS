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
using Kooboo.Common.IO;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public static class IInheritableHelper
    {
        public static IEnumerable<T> All<T>(Site site)
            where T : PathResource, IInheritable<T>, IPersistable
        {
            List<T> results = new List<T>();

            while (site != null)
            {
                var tempResults = AllInternal<T>(site);
                if (results.Count == 0)
                {
                    results.AddRange(tempResults);
                }
                else
                {
                    foreach (var item in tempResults)
                    {
                        if (!results.Any(it => it.Name.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            results.Add(item);
                        }
                    }
                }
                site = site.Parent;
            }
            return results;
        }

        private static IEnumerable<T> AllInternal<T>(Site site)
             where T : PathResource, IInheritable<T>, IPersistable
        {
            string baseDir = ModelActivatorFactory<T>.GetActivator().CreateDummy(site).BasePhysicalPath;
            if (Directory.Exists(baseDir))
            {
                foreach (var dir in IOUtility.EnumerateDirectoriesExludeHidden(baseDir).Where(it => !it.Name.EqualsOrNullEmpty("~versions", StringComparison.OrdinalIgnoreCase)))
                {
                    var o = ModelActivatorFactory<T>.GetActivator().Create(dir.FullName);
                    if (o is IFilePersistable)
                    {
                        if (!File.Exists(((IFilePersistable)o).DataFile))
                        {
                            continue;
                        }
                    }

                    // The provider used in AsActual is possibly different with the current provider.
                    // the AsActual method is not recommended used in Provider level.
                    //if (o.AsActual() == null)
                    //{
                    //    continue;
                    //}

                    yield return o;
                }
            }
        }
    }
}
