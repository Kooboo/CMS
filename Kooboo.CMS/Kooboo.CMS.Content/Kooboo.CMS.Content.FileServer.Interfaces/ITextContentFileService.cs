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
using System.IO;
using System.ServiceModel.Web;
using System.ServiceModel;

namespace Kooboo.CMS.Content.FileServer.Interfaces
{
    [ServiceContract]
    public interface ITextContentFileService
    {
        [WebInvoke(UriTemplate = "{repositoryName}/{folderName}/{contentUUID}?fileName={fileName}", Method = "POST")]
        string Add(string repositoryName, string folderName, string contentUUID, string fileName, byte[] binaryData);
        [WebInvoke(UriTemplate = "{repositoryName}/{folderName}/{contentUUID}", Method = "DELETE")]
        void DeleteFiles(string repositoryName, string folderName, string contentUUID);
    }
}
