﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

using Ionic.Zip;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.FileServer.Interfaces;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Content.Persistence.FileServerProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IMediaFolderProvider), Order = 2)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<MediaFolder>), Order = 2)]
    public class MediaFolderProvider : IMediaFolderProvider
    {
        public IQueryable<MediaFolder> ChildFolders(MediaFolder parent)
        {
            return RemoteServiceFactory.CreateService<IMediaFolderService>().ChildFolders(parent.Repository.Name, parent.FullName).AsQueryable();
        }

        public IQueryable<MediaFolder> All(Repository repository)
        {
            return RemoteServiceFactory.CreateService<IMediaFolderService>().RootFolders(repository.Name)
                .Select(it => { it.Repository = repository; return it; }).AsQueryable();
        }

        public MediaFolder Get(MediaFolder dummy)
        {
            var folder = RemoteServiceFactory.CreateService<IMediaFolderService>()
            .Get(dummy.Repository.Name, dummy.FullName);
            if (folder != null)
            {
                ((IPersistable)folder).Init(dummy);
            }
            return folder;
        }

        public void Add(MediaFolder item)
        {
            RemoteServiceFactory.CreateService<IMediaFolderService>().Add(item.Repository.Name, item);
        }

        public void Update(MediaFolder @new, MediaFolder old)
        {
            RemoteServiceFactory.CreateService<IMediaFolderService>().Update(@new.Repository.Name, @new);
        }

        public void Remove(MediaFolder item)
        {
            RemoteServiceFactory.CreateService<IMediaFolderService>().Delete(item.Repository.Name, item.FullName);
            //(new MediaContentProvider()).Delete(item);
        }


        public void Export(Repository repository, IEnumerable<MediaFolder> models, System.IO.Stream outputStream)
        {
            throw new NotImplementedException();
        }

        public void Import(Repository repository, MediaFolder folder, System.IO.Stream zipStream, bool @override)
        {
            using (ZipFile zipFile = ZipFile.Read(zipStream))
            {
                foreach (ZipEntry item in zipFile)
                {
                    if (item.IsDirectory)
                    {

                    }
                    else
                    {
                        var path = Path.GetDirectoryName(item.FileName);
                        var fileName = Path.GetFileName(item.FileName);
                        var currentFolder = CreateMediaFolderByPath(folder, path);
                        Add(currentFolder);
                        var stream = new MemoryStream();
                        item.Extract(stream);
                        stream.Position = 0;
                        ServiceFactory.MediaContentManager.Add(repository, currentFolder,
                            fileName, stream, true);
                    }
                }
            }
        }
        private MediaFolder CreateMediaFolderByPath(MediaFolder folder, string pathName)
        {
            return new MediaFolder(folder.Repository, pathName, folder);
        }

        IEnumerable<MediaFolder> IContentElementProvider<MediaFolder>.All(Repository repository)
        {
            return this.All(repository);
        }

        public IEnumerable<MediaFolder> All()
        {
            throw new NotImplementedException();
        }

        public void Rename(MediaFolder @new, MediaFolder old)
        {
            throw new NotImplementedException(); // not implemented yet
        }


        public void Export(Repository repository, string baseFolder, string[] folders, string[] docs, Stream outputStream)
        {
            throw new NotImplementedException();
        }
    }
}
