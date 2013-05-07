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
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Content.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IMediaFolderProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<MediaFolder>))]
    public class MediaFolderProvider : FolderProvider<MediaFolder>, IMediaFolderProvider
    {
        #region Get
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
        #endregion
        
        #region GetLocker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        } 
        #endregion

        #region Rename
        public void Rename(MediaFolder @new, MediaFolder old)
        {
            var newPath = PathFactory.GetPath(@new);
            var oldPath = PathFactory.GetPath(old);
            oldPath.Rename(newPath.PhysicalPath);
        }
        
        #endregion

        #region Export
        public void Export(Repository repository, string baseFolder, string[] folders, string[] docs, System.IO.Stream outputStream)
        {
            using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
            {
                if (folders != null)
                {
                    foreach (var folder in folders)
                    {
                        var mediaFolder = GetMediaFolder(repository, baseFolder, folder);
                        FolderPath path = new FolderPath(mediaFolder);
                        zipFile.AddDirectory(path.PhysicalPath, mediaFolder.Name);
                    }
                }
                if (baseFolder != null && docs != null)
                {
                    var mediaFolder = new MediaFolder(repository, baseFolder);
                    foreach (var doc in docs)
                    {
                        var content = mediaFolder.CreateQuery().WhereEquals("UUID", doc).FirstOrDefault();
                        if (content != null)
                        {
                            zipFile.AddFile(content.PhysicalPath, "");
                        }
                    }
                }
                zipFile.Save(outputStream);
            }
        } 
        #endregion

        #region GetMediaFolder
        private MediaFolder GetMediaFolder(Repository repository, string baseFolder, string folderName)
        {
            var fullName = folderName;
            if (baseFolder != null)
            {
                fullName = FolderHelper.CombineFullName(new[] { baseFolder, folderName });
            }
            return new MediaFolder(repository, fullName);
        } 
        #endregion
    }
}
