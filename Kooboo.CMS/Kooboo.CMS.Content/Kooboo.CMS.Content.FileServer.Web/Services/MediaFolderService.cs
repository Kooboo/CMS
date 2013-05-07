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
using Kooboo.CMS.Content.Models;
using System.ServiceModel.Activation;
namespace Kooboo.CMS.Content.FileServer.Web.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MediaFolderService : IMediaFolderService
    {
        private IMediaFolderProvider mediaFolderProvider = Providers.DefaultProviderFactory.GetProvider<IMediaFolderProvider>();
        public IEnumerable<Models.MediaFolder> RootFolders(string repositoryName)
        {
            return mediaFolderProvider.All(new Repository(repositoryName));
        }

        public IEnumerable<Models.MediaFolder> ChildFolders(string repositoryName, string fullName)
        {
            return mediaFolderProvider.ChildFolders(new MediaFolder(new Repository(repositoryName), fullName));
        }

        public Models.MediaFolder Get(string repositoryName, string fullName)
        {
            return mediaFolderProvider.Get(new MediaFolder(new Repository(repositoryName), fullName));
        }

        public void Add(string repositoryName, Models.MediaFolder mediaFolder)
        {
            mediaFolder.Repository = new Repository(repositoryName);
            mediaFolderProvider.Add(mediaFolder);
        }

        public void Update(string repositoryName, Models.MediaFolder mediaFolder)
        {
            mediaFolder.Repository = new Repository(repositoryName);
            mediaFolderProvider.Update(mediaFolder, mediaFolder);
        }

        public void Delete(string repositoryName, string fullName)
        {
            mediaFolderProvider.Remove(new MediaFolder(new Repository(repositoryName), fullName));
        }
    }
}