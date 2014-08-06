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
    public class SiteStorageMeta : IModelStorageMeta<Site>
    {
        private IBaseDir baseDir;
        public SiteStorageMeta(IBaseDir baseDir)
        {
            this.baseDir = baseDir;
        }
        public Kooboo.Common.Data.IsolatedStorage.IIsolatedStorage GetIsolatedStorage(Site site)
        {
            IIsolatedStorage isolatedStorage;
            if (site != null)
            {
                var basePath = site.DiskStoragePath(baseDir);

                isolatedStorage = new DiskIsolateStorage(site.Name, basePath);
            }
            else
            {
                isolatedStorage = new DiskIsolateStorage("Sites", baseDir.Cms_DataPhysicalPath);
            }

            return isolatedStorage;
        }

        public string GetItemPathName(Site item)
        {
            return item.Name;
        }

        public string GetItemSettingFileName(Site item)
        {
            return Path.Combine(item.Name, baseDir.SettingFileName);
        }


        public Site CreateItem(Site site, string pathName)
        {
            //  protected Func<string, T> _initialize = (path) =>
            //{
            //    var o = new T() { UUID = Path.GetFileName(path) };
            //    return o;
            //};
            return null;
        }
    }
}
