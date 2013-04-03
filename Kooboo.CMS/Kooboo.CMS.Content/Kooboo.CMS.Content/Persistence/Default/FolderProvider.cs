using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using System.IO;
using Kooboo.Web.Url;
using Kooboo.CMS.Content.Models.Paths;

namespace Kooboo.CMS.Content.Persistence.Default
{
    public abstract class FolderProvider<T> : FileSystemProviderBase<T>, IFolderProvider<T>
        where T : Folder
    {
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

        #region IRepository<Folder> Members

        public IQueryable<T> All(Repository repository)
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
                ImportHelper.Import(new FolderPath(folder).PhysicalPath, zipStream, @override);
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }
        }
        #endregion


    }
    public class TextFolderProvider : FolderProvider<TextFolder>, ITextFolderProvider
    {
        #region IFolderProvider Members

        public IQueryable<TextFolder> BySchema(Schema schema)
        {
            return BySchemaEnumerable(schema).AsQueryable();
        }
        public IEnumerable<TextFolder> BySchemaEnumerable(Schema schema)
        {
            IEnumerable<TextFolder> schemaFolders = new TextFolder[0];
            foreach (var item in All(schema.Repository))
            {
                schemaFolders = schemaFolders.Concat(BySchema(item, schema));
            }
            return schemaFolders;
        }
        private IEnumerable<TextFolder> BySchema(TextFolder folder, Schema schema)
        {
            IEnumerable<TextFolder> schemaFolders = new TextFolder[0];
            folder = folder.AsActual();
            if (folder is TextFolder && string.Compare(((TextFolder)folder).SchemaName, schema.Name, true) == 0)
            {
                schemaFolders = schemaFolders.Concat(new[] { folder });
            }

            foreach (var child in ChildFolders(folder))
            {
                schemaFolders = schemaFolders.Concat(BySchema(child, schema));
            }

            return schemaFolders;
        }

        #endregion
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
    }

    public class MediaFolderProvider : FolderProvider<MediaFolder>, IMediaFolderProvider
    {
        public override MediaFolder Get(MediaFolder dummy)
        {
            FolderPath path = new FolderPath(dummy);
            if (!path.Exists())
            {
                return null;
            }
            dummy.IsDummy = false;
            return dummy;
        }
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
    }
}
