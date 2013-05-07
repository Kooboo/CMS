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
using System.IO;
using System.ServiceModel.Activation;
namespace Kooboo.CMS.Content.FileServer.Web.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TextContentFileService : ITextContentFileService
    {
        ITextContentFileProvider textContentFileProvider = Providers.DefaultProviderFactory.GetProvider<ITextContentFileProvider>();
        public string Add(string repositoryName, string folderName, string contentUUID, string fileName, byte[] binaryData)
        {
            var textContent = new TextContent(repositoryName, null, folderName) { UUID = contentUUID };
            var ms = new MemoryStream(binaryData);
            ms.Position = 0;
            var contentFile = new ContentFile() { FileName = fileName, Stream = ms };
            return FileUrlHelper.ResolveUrl(textContentFileProvider.Save(textContent, contentFile));
        }

        public void DeleteFiles(string repositoryName, string folderName, string contentUUID)
        {
            var textContent = new TextContent(repositoryName, null, folderName) { UUID = contentUUID };
            textContentFileProvider.DeleteFiles(textContent);
        }
    }
}