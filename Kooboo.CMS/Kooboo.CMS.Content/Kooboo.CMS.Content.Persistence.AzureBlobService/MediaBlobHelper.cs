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
    public static class MediaBlobHelper
    {
        #region const
        public static string MediaDirectoryName = "Media";
        public static string MediaFolderContentType = "MediaFolder"; 
        #endregion

        #region GetMediaDirectoryPath
        public static string GetMediaDirectoryPath(this MediaFolder mediaFolder)
        {
            return UrlUtility.Combine(new string[] { StorageNamesEncoder.EncodeContainerName(mediaFolder.Repository.Name), MediaDirectoryName }
               .Concat(mediaFolder.NamePaths.Select(it => StorageNamesEncoder.EncodeBlobName(it)))
               .ToArray());
        } 
        #endregion

        #region GetMediaFolderItemPath
        public static string GetMediaFolderItemPath(this MediaFolder mediaFolder, string itemName)
        {
            return UrlUtility.Combine(mediaFolder.GetMediaDirectoryPath(), StorageNamesEncoder.EncodeBlobName(itemName));
        } 
        #endregion

        #region GetMediaBlobPath
        public static string GetMediaBlobPath(this MediaContent mediaContent)
        {
            return GetMediaFolderItemPath(mediaContent.GetFolder(), mediaContent.FileName);
        } 
        #endregion

        #region MediaContentToBlob
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
                blob.Metadata["FileName"] = StorageNamesEncoder.EncodeBlobName(mediaContent.FileName);
            }
            if (mediaContent.ContentFile != null)
            {
                blob.Metadata["Size"] = mediaContent.ContentFile.Stream.Length.ToString();
            }
            if (mediaContent.Metadata != null)
            {
                if (!string.IsNullOrEmpty(mediaContent.Metadata.AlternateText))
                {
                    blob.Metadata["AlternateText"] = StorageNamesEncoder.EncodeBlobName(mediaContent.Metadata.AlternateText);
                }
                else if(blob.Metadata.AllKeys.Contains("AlternateText"))
                {
                    blob.Metadata.Remove("AlternateText");
                }

                if (!string.IsNullOrEmpty(mediaContent.Metadata.Description))
                {
                    blob.Metadata["Description"] = StorageNamesEncoder.EncodeBlobName(mediaContent.Metadata.Description);
                }
                else if (blob.Metadata.AllKeys.Contains("Description"))
                {
                    blob.Metadata.Remove("Description");
                }

                if (!string.IsNullOrEmpty(mediaContent.Metadata.Title))
                {
                    blob.Metadata["Title"] = StorageNamesEncoder.EncodeBlobName(mediaContent.Metadata.Title);
                }
                else if (blob.Metadata.AllKeys.Contains("Title"))
                {
                    blob.Metadata.Remove("Title");
                }
                
            }

            blob.Properties.ContentType = Kooboo.IO.IOUtility.MimeType(mediaContent.FileName);
            return blob;
        } 
        #endregion

        #region BlobToMediaContent
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
            mediaContent.FileName = StorageNamesEncoder.DecodeBlobName(blob.Metadata["FileName"]);
            mediaContent.UserKey = mediaContent.FileName;
            mediaContent.UUID = mediaContent.FileName;
            mediaContent.UserId = blob.Metadata["UserId"];
            mediaContent.VirtualPath = blob.Uri.ToString();
            if (mediaContent.Metadata == null)
            {
                mediaContent.Metadata = new MediaContentMetadata();
            }

            mediaContent.Metadata.AlternateText = StorageNamesEncoder.DecodeBlobName(blob.Metadata["AlternateText"]);
            mediaContent.Metadata.Description = StorageNamesEncoder.DecodeBlobName(blob.Metadata["Description"]);
            mediaContent.Metadata.Title = StorageNamesEncoder.DecodeBlobName(blob.Metadata["Title"]);
            return mediaContent;
        } 
        #endregion

        #region SetMediaFolderContentType
        public static void SetMediaFolderContentType(this CloudBlob blob)
        {
            blob.Properties.ContentType = MediaFolderContentType;
        } 
        #endregion

        #region CheckIfMediaFolder
        public static bool CheckIfMediaFolder(this CloudBlob blob)
        {
            return blob.Properties.ContentType == MediaFolderContentType;
        } 
        #endregion

        #region InitializeRepositoryContainer
        public static CloudBlobContainer InitializeRepositoryContainer(Repository repository)
        {
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(StorageNamesEncoder.EncodeContainerName(repository.Name));

            var created = container.CreateIfNotExist();
            if (created)
            {
                container.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            return container;
        } 
        #endregion

        #region DeleteRepositoryContainer
        public static void DeleteRepositoryContainer(Repository repository)
        {
            var blobClient = CloudStorageAccountHelper.GetStorageAccount().CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(StorageNamesEncoder.EncodeContainerName(repository.Name));

            container.Delete();
        } 
        #endregion
    }
}
