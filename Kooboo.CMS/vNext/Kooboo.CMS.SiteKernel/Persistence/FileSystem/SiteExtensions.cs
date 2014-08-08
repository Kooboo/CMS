#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.Common.Data.IsolatedStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Persistence.FileSystem
{
    internal static class SiteExtensions
    {
        private static string DiskStoragePath(this Site site, IBaseDir baseDir)
        {
            var basePath = baseDir.Cms_DataPhysicalPath;
            if (site.Parent != null)
            {
                basePath = site.Parent.DiskStoragePath(baseDir);
            }

            return Path.Combine(basePath, "Sites", site.Name);
        }

        public static IIsolatedStorage GetIsolatedStorage(this Site site)
        {
            IBaseDir baseDir = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<IBaseDir>();

            var basePath = site.DiskStoragePath(baseDir);

            var isolatedStorage = new DiskIsolateStorage(site.Name, basePath);

            return isolatedStorage;
        }
    }
}
