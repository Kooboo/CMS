using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;

using System.IO;
using System.ComponentModel.Composition;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Export(typeof(ICustomFileProvider))]
    public class CustomFileProvider : ICustomFileProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        #region ICustomFileRepository Members

        public IQueryable<CustomFile> All(CustomDirectory dir)
        {
            return AllEnumerable(dir).AsQueryable();
        }
        public IEnumerable<CustomFile> AllEnumerable(CustomDirectory dir)
        {
            List<CustomFile> list = new List<CustomFile>();
            var baseDir = dir.PhysicalPath;
            if (Directory.Exists(baseDir))
            {
                //output directory 
                foreach (var folder in IO.IOUtility.EnumerateDirectoriesExludeHidden(baseDir))
                {
                    var customFile = new CustomFile();
                    customFile.Site = dir.Site;
                    customFile.FileType = "Folder";

                    customFile.Name = folder.Name;
                    customFile.Directory = new CustomDirectory(dir, customFile.Name);
                    list.Add(customFile);
                }
                //output files
                foreach (var file in IO.IOUtility.EnumerateFilesExludeHidden(baseDir))
                {
                    var customFile = new CustomFile(file.FullName);
                    customFile.Site = dir.Site;
                    customFile.Directory = new CustomDirectory(dir, customFile.Name);
                    list.Add(customFile);
                }
            }
            return list;
        }
        #endregion

        #region IRepository<ImageFile> Members


        public CustomFile Get(CustomFile dummy)
        {
            throw new NotImplementedException();
        }

        public void Add(CustomFile item)
        {
            throw new NotImplementedException();
        }

        public void Update(CustomFile @new, CustomFile old)
        {
            throw new NotImplementedException();
        }

        public void Remove(CustomFile item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRepository<CustomFile> Members

        public IQueryable<CustomFile> All(Site site)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IImportRepository Members

        public void Export(CustomDirectory dir, Stream outputStream)
        {
            locker.EnterReadLock();
            try
            {
                ImportHelper.Export(new[] { dir }, outputStream);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public void Import(Site site, CustomDirectory dir, Stream zipStream, bool @override)
        {
            locker.EnterWriteLock();
            try
            {
                ImportHelper.Import(site, dir.PhysicalPath, zipStream, @override);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        #endregion
    }
}
