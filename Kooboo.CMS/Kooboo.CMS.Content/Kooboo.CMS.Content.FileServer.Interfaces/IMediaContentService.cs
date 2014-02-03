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
using System.ServiceModel;
using Kooboo.CMS.Content.Models;
using System.IO;
using System.ServiceModel.Web;
using System.Runtime.Serialization;

namespace Kooboo.CMS.Content.FileServer.Interfaces
{
    [DataContract]
    public class MediaContentParameter
    {
        [DataMember]
        public MediaContent MediaContent { get; set; }
        [DataMember]
        public byte[] FileData { get; set; }

        public Stream FileDataToStream()
        {
            var ms = new MemoryStream(FileData);
            ms.Position = 0;
            return ms;
        }
    }
    [ServiceContract]
    public interface IMediaContentService
    {
        [WebGet(UriTemplate = "all/{repositoryName}/{folderName}?skip={skip}&maxResult={maxResult}&prefix={prefix}")]
        IEnumerable<MediaContent> All(string repositoryName, string folderName, int skip, int maxResult, string prefix);
        
        [WebGet(UriTemplate = "count/{repositoryName}/{folderName}?prefix={prefix}")]
        int Count(string repositoryName, string folderName, string prefix);
        
        [WebGet(UriTemplate = "{repositoryName}/{folderName}?fileName={fileName}")]
        MediaContent Get(string repositoryName, string folderName, string fileName);

        [WebGet(UriTemplate = "GetBytes/{repositoryName}/{folderName}?fileName={fileName}")]
        byte[] GetBytes(string repositoryName, string folderName, string fileName);

        [WebInvoke(UriTemplate = "SaveBytes", Method = "POST")]
        void SaveBytes(MediaContentParameter content);
        
        [WebInvoke(UriTemplate = "/", Method = "POST")]
        string Add(MediaContentParameter content);
        
        [WebInvoke(UriTemplate = "Move/{repositoryName}/{sourceFolder}/{targetFolder}?fileName={fileName}&newFileName={newFileName}", Method = "POST")]
        void Move(string repositoryName, string sourceFolder, string fileName, string targetFolder, string newFileName);
        
        [WebInvoke(UriTemplate = "/", Method = "PUT")]
        void Update(MediaContentParameter content);
        
        [WebInvoke(UriTemplate = "{repositoryName}/{folderName}?fileName={fileName}", Method = "DELETE")]
        void Delete(string repositoryName, string folderName, string fileName);
    }
}
