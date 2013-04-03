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
    public static class MediaBlobHelper
    {
        public static string MediaDirectoryName = "Media";
        public static string MediaFolderContentType = "MediaFolder";

        public static string GetMediaDirectoryPath(this MediaFolder mediaFolder)
        {
            return UrlUtility.Combine(new string[] { mediaFolder.Repository.Name.ToLower(), MediaDirectoryName }
               .Concat(mediaFolder.NamePaths)
               .ToArray());
        }
        public static string GetMediaFolderItemPath(this MediaFolder mediaFolder, string itemName)
        {
            return UrlUtility.Combine(mediaFolder.GetMediaDirectoryPath(), itemName);
        }

        public static string GetMediaBlobPath(this MediaContent mediaContent)
        {
            return GetMediaFolderItemPath(mediaContent.GetFolder(), mediaContent.FileName);
        }

        public static CloudBlob MediaContentToBlob(this MediaContent mediaContent, CloudBlob blob)
        {
            if (mediaContent.Published.HasValue)
            {
                blob.Metadata["Published"] = mediaContent.Published.Value.ToString();
            }
            if (!string.IsNullOrEmpty(mediaContent.UserId))
            {
                blob.Metadata["UserId"] = mediaContent.UserId;
            }
            if (!string.IsNullOrEmpty(mediaContent.FileName))
            {
                blob.Metadata["FileName"] = mediaContent.FileName;
            }
            if (mediaContent.ContentFile != null)
            {
                blob.Metadata["Size"] = mediaContent.ContentFile.Stream.Length.ToString();
            }


            blob.Properties.ContentType = Kooboo.IO.IOUtility.MimeType(mediaContent.FileName);
            return blob;
        }
        public static MediaContent BlobToMediaContent(this CloudBlob blob, MediaContent mediaContent)
        {
            if (!string.IsNullOrEmpty(blob.Metadata["Published"]))
            {
                mediaContent.Published = bool.Parse(blob.Metadata["Published"]);
            }
            if (!string.IsNullOrEmpty(blob.Metadata["Size"]))
            {
                mediaContent.Size = int.Parse(blob.Metadata["Size"]);
            }
            mediaContent.FileName = blob.Metadata["FileName"];
            mediaContent.UserKey = mediaContent.FileName;
            mediaContent.UUID = mediaContent.FileName;
            mediaContent.UserId = blob.Metadata["UserId"];
            mediaContent.VirtualPath = blob.Uri.ToString();
            return mediaContent;
        }

        public static void SetMediaFolderContentType(this CloudBlob blob)
        {
            blob.Properties.ContentType = MediaFolderContentType;
        }
        public static bool CheckIfMediaFolder(this CloudBlob blob)
        {
            return blob.Properties.ContentType == MediaFolderContentType;
        }
        public static CloudBlobContainer InitializeRepositoryContainer(Repository repository)
        {
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(repository.Name.ToLower());

            var created = container.CreateIfNotExist();
            if (created)
            {
                container.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            return container;
        }
        public static void DeleteRepositoryContainer(Repository repository)
        {
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(repository.Name.ToLower());

            container.Delete();
        }
    }
}
