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
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Default
{
    public abstract class FolderProvider<T> : FileSystemProviderBase<T>, IFolderProvider<T>
        where T : Folder
    {
        #region KnownTypes
        protected override IEnumerable<Type> KnownTypes
        {
            get
            {
                return new Type[]{
                //typeof(ContainerFolder),
                //typeof(ContentFolder),
                typeof(TextFolder),
                typeof(MediaFolder)
                };
            }
        }
        #endregion

        #region IRepository<Folder> Members

        public IEnumerable<T> All(Repository repository)
        {
            var baseDir = FolderPath.GetBaseDir<T>(repository);
            return GetFolders(repository, baseDir).AsQueryable();
        }
        private IEnumerable<T> GetFolders(Repository repository, string baseDir, Folder parent = null)
        {
            List<T> list = new List<T>();
            if (Directory.Exists(baseDir))
            {
                foreach (var item in IO.IOUtility.EnumerateDirectoriesExludeHidden(baseDir))
                {
                    var folderName = item.Name;
                    // Compatible with the ContentFolderName has been change (_contents=>.contents)
                    if (string.Compare(folderName, TextContentPath.ContentAttachementFolder, true) != 0)
                    {
                        var folder = (T)Activator.CreateInstance(typeof(T), repository, folderName);
                        folder.Parent = parent;
                        folder.UtcCreationDate = item.CreationTimeUtc;
                        if ((folder is MediaFolder) || (folder is TextFolder && File.Exists(Path.Combine(item.FullName, PathHelper.SettingFileName))))
                        {
                            list.Add(folder);
                        }
                    }
                }
            }
            return list;
        }

        #endregion

        #region IFolderProvider Members

        public IQueryable<T> ChildFolders(T parent)
        {
            if (parent == null)
            {
                return null;
            }
            var folderPath = new FolderPath(parent);
            return GetFolders(parent.Repository, folderPath.PhysicalPath, parent).AsQueryable();
        }

        #endregion

        #region IImportProvider Members

        public void Export(Repository repository, IEnumerable<T> models, Stream outputStream)
        {
            var list = models;
            GetLocker().EnterReadLock();
            try
            {
                ImportHelper.Export(list.Select(it => new FolderPath(it).PhysicalPath), outputStream);
            }
            finally
            {
                GetLocker().ExitReadLock();
            }

        }

        public void Import(Repository repository, T folder, Stream zipStream, bool @override)
        {
            GetLocker().EnterWriteLock();
            try
            {
                string basePath = FolderPath.GetBaseDir<T>(repository);
                if (null != folder)
                {
                    basePath = new FolderPath(folder).PhysicalPath;
                }
                ImportHelper.Import(basePath, zipStream, @override);
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }
        }
        #endregion
    }

}
