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
using Microsoft.WindowsAzure.StorageClient;
namespace Kooboo.CMS.Content.Persistence.AzureBlobService
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ITextContentFileProvider), Order = 2)]
    public class TextContentFileProvider : ITextContentFileProvider
    {
        public string Save(Models.TextContent content, Models.ContentFile file)
        {
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

            var contentBlob = blobClient.GetBlobReference(content.GetTextContentFilePath(file));

            contentBlob.Properties.ContentType = Kooboo.Common.IO.IOUtility.MimeType(file.FileName);

            contentBlob.UploadFromStream(file.Stream);

            return contentBlob.Uri.ToString();
        }

        public void DeleteFiles(Models.TextContent content)
        {
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

            var contentDir = blobClient.GetBlobDirectoryReference(content.GetTextContentDirectoryPath());
            foreach (var item in contentDir.ListBlobs())
            {
                CloudBlob blob = item as CloudBlob;
                if (item != null)
                {
                    blob.DeleteIfExists();
                }
            }
        }
    }
}
