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
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.FileServer.Interfaces
{
    [ServiceContract]
    public interface IMediaFolderService
    {
        [WebGet(UriTemplate = "RootFolders/{repositoryName}")]
        IEnumerable<MediaFolder> RootFolders(string repositoryName);

        [WebGet(UriTemplate = "ChildFolders/{repositoryName}/{fullName}")]
        IEnumerable<MediaFolder> ChildFolders(string repositoryName, string fullName);

        [WebGet(UriTemplate = "{repositoryName}/{fullName}")]
        MediaFolder Get(string repositoryName, string fullName);

        [WebInvoke(UriTemplate = "{repositoryName}/", Method = "POST")]
        void Add(string repositoryName, MediaFolder mediaFolder);

        [WebInvoke(UriTemplate = "{repositoryName}/", Method = "PUT")]
        void Update(string repositoryName, MediaFolder mediaFolder);

        [WebInvoke(UriTemplate = "{repositoryName}/{fullName}", Method = "DELETE")]
        void Delete(string repositoryName, string fullName);
    }
}
