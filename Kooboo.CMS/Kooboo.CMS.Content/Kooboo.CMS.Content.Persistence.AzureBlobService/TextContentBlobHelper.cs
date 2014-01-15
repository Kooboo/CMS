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
using Kooboo.CMS.Content.Models.Paths;
using Microsoft.WindowsAzure.StorageClient;
using Kooboo.CMS.Content.Models;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Content.Persistence.AzureBlobService
{
    public static class TextContentBlobHelper
    {
        public static string TextContentFileDirectoryName = "Folders";

        public static string GetTextContentDirectoryPath(this TextContent textContent)
        {
            var textFolder = textContent.GetFolder();
            return UrlUtility.Combine(new string[] { StorageNamesEncoder.EncodeContainerName(textFolder.Repository.Name), TextContentFileDirectoryName }
               .Concat(textFolder.NamePaths.Select(it=>StorageNamesEncoder.EncodeBlobName(it)))
               .Concat(new[] { StorageNamesEncoder.EncodeBlobName(textContent.UUID) })
               .ToArray());
        }
        public static string GetTextContentFilePath(this TextContent textContent, ContentFile contentFile)
        {
            return UrlUtility.Combine(GetTextContentDirectoryPath(textContent), StorageNamesEncoder.EncodeBlobName(contentFile.FileName));
        }
    }
}
