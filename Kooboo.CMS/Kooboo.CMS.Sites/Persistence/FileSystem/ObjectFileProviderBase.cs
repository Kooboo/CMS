#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Ionic.Zip;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public abstract class ObjectFileProviderBase<T> : SettingFileProviderBase<T>
         where T : IPersistable, IIdentifiable
    {
        #region All
        public override IEnumerable<T> All(Site site)
        {
            GetLocker().EnterReadLock();
            try
            {
                return Enumerate(site);
            }
            finally
            {
                GetLocker().ExitReadLock();
            }

        }

        private IEnumerable<T> Enumerate(Site site)
        {
            List<T> list = new List<T>();
            var basePath = GetBasePath(site);
            if (Directory.Exists(basePath))
            {
                var dir = new DirectoryInfo(basePath);
                foreach (var fileInfo in dir.EnumerateFiles())
                {
                    list.Add(CreateObject(site, fileInfo));
                }
            }
            return list;
        }
        #endregion

        #region GetDataFilePath
        protected override string GetDataFilePath(T o)
        {
            if (o is IFilePersistable)
            {
                return ((IFilePersistable)o).DataFile;
            }
            else
            {
                var basePath = "";
                if (o is ISiteObject)
                {
                    basePath = GetBasePath(((ISiteObject)o).Site);
                }
                else
                {
                    basePath =GetBasePath(null);
                }
                return Path.Combine(basePath, o.UUID + ".config");
            }
        }
        #endregion

        #region Abstract methods
        protected abstract T CreateObject(Site site, FileInfo fileInfo);
        protected abstract string GetBasePath(Site site);
        #endregion

        #region Import/Export
        public void Export(IEnumerable<T> sources, Stream outputStream)
        {
            using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
            {
                foreach (var item in sources)
                {
                    zipFile.AddFile(GetDataFilePath(item), "");
                }

                zipFile.Save(outputStream);
            }
        }

        public void Import(Models.Site site, Stream zipStream, bool @override)
        {
            using (ZipFile zipFile = ZipFile.Read(zipStream))
            {
                ExtractExistingFileAction action = ExtractExistingFileAction.DoNotOverwrite;
                if (@override)
                {
                    action = ExtractExistingFileAction.OverwriteSilently;
                }
                zipFile.ExtractAll(GetBasePath(site), action);
            }
        }
        #endregion


    }
}