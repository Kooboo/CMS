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
using System.Web;
using Kooboo.CMS.Content.FileServer.Interfaces;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using System.ServiceModel.Activation;
namespace Kooboo.CMS.Content.FileServer.Web.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MediaContentService : IMediaContentService
    {
        IMediaContentProvider mediaContentProvider = Providers.DefaultProviderFactory.GetProvider<IMediaContentProvider>();

        public IEnumerable<MediaContent> All(string repositoryName, string folderName
            , int skip, int maxResult, string prefix)
        {
            var mediaFolder = new MediaFolder(new Repository(repositoryName), folderName);
            return mediaFolder.CreateQuery().WhereContains("FileName", prefix)
                .Skip(skip)
                .Take(maxResult)
                .ToArray()
                .Select(it => { it.VirtualPath = FileUrlHelper.ResolveUrl(it.VirtualPath); return it; });
        }
        public int Count(string repositoryName, string folderName, string prefix)
        {
            var mediaFolder = new MediaFolder(new Repository(repositoryName), folderName);
            return mediaFolder.CreateQuery().WhereContains("FileName", prefix)
                .Count();
        }
        public MediaContent Get(string repositoryName, string folderName, string fileName)
        {
            var mediaFolder = new MediaFolder(new Repository(repositoryName), folderName);
            var content = mediaFolder.CreateQuery().WhereEquals("FileName", fileName)
                .FirstOrDefault();
            if (content != null)
            {
                content.VirtualPath = FileUrlHelper.ResolveUrl(content.VirtualPath);
            }
            return content;
        }
        public string Add(MediaContentParameter content)
        {
            content.MediaContent.ContentFile = new ContentFile() { FileName = content.MediaContent.FileName, Stream = content.FileDataToStream() };
            mediaContentProvider.Add(content.MediaContent);
            return FileUrlHelper.ResolveUrl(content.MediaContent.VirtualPath);
        }
        public void Move(string repositoryName, string sourceFolder, string fileName, string targetFolder, string newFileName)
        {
            var repository = new Repository(repositoryName);
            mediaContentProvider.Move(new MediaFolder(repository, sourceFolder), fileName, new MediaFolder(repository, targetFolder), newFileName);
        }
        public void Update(MediaContentParameter content)
        {
            content.MediaContent.ContentFile = new ContentFile() { FileName = content.MediaContent.FileName, Stream = content.FileDataToStream() };
            mediaContentProvider.Update(content.MediaContent, content.MediaContent);
            FileUrlHelper.ResolveUrl(content.MediaContent.VirtualPath);
        }

        public void Delete(string repositoryName, string folderName, string fileName)
        {
            var mediaFolder = new MediaFolder(new Repository(repositoryName), folderName);
            var content = mediaFolder.CreateQuery().WhereEquals("FileName", fileName)
                .FirstOrDefault();
            if (content != null)
            {
                mediaContentProvider.Delete(content);
            }
        }
    }
}