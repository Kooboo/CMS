#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IDataSourceSettingProvider))]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<DataSourceSetting>))]
    public class DataSourceSettingProvider : InheritableProviderBase<DataSourceSetting>, IDataSourceSettingProvider
    {
        #region .ctor
        const string DIRNAME = "DataSources";
        static System.Threading.ReaderWriterLockSlim @lock = new System.Threading.ReaderWriterLockSlim();
        IDataSourceDesigner[] _designers;
        public DataSourceSettingProvider(IDataSourceDesigner[] designers)
        {
            _designers = designers;
        }
        #endregion
        public void InitializeToDB(Models.Site site)
        {
            //throw new NotImplementedException();
        }

        public void ExportToDisk(Models.Site site)
        {
            //throw new NotImplementedException();
        }

        public void Localize(DataSourceSetting o, Site targetSite)
        {

        }

        protected override IFileStorage<DataSourceSetting> GetFileStorage(Site site)
        {
            var basePath = Path.Combine(site.PhysicalPath, DIRNAME);
            var knownTypes = _designers.Select(it => it.CreateDataSource().GetType());
            return new XmlObjectFileStorage<DataSourceSetting>(basePath, @lock, knownTypes);
        }
    }
}
