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
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Models;
using System.Collections.Specialized;
using System.IO;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.Web.Url;
using Kooboo.IO;
using Kooboo.CMS.Content.Query;
using Ionic.Zip;
namespace Kooboo.CMS.Content.Services
{
    /// <summary>
    /// Media内容管理
    /// </summary>
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(MediaContentManager))]
    public class MediaContentManager
    {
        private IMediaContentProvider Provider
        {
            get;
            set;
        }
        public MediaContentManager(IMediaContentProvider mediaContentProvider)
        {
            this.Provider = mediaContentProvider;
        }


        #region ByFolder
        public virtual MediaContent Add(Repository repository, MediaFolder mediaFolder, string fileName, Stream fileStream, bool @overrided, string userId = "")
        {
            return Add(repository, mediaFolder, fileName, fileStream, overrided, userId, null);
        }
        public virtual MediaContent Add(Repository repository, MediaFolder mediaFolder, string fileName, Stream fileStream, bool @overrided,
            string userId, MediaContentMetadata metadata)
        {
            fileName = UrlUtility.ToUrlString(Path.GetFileNameWithoutExtension(fileName)) + Path.GetExtension(fileName);
            IsAllowExtension(fileName, mediaFolder.AllowedExtensions);

            var mediaContent = new MediaContent(repository.Name, mediaFolder.FullName);

            mediaContent.UserId = userId;
            mediaContent.Published = true;

            mediaContent.FileName = fileName;

            mediaContent.UserKey = fileName;
            mediaContent.UUID = fileName;

            mediaContent.ContentFile = new ContentFile()
            {
                Name = fileName,
                FileName = fileName,
                Stream = fileStream
            };

            mediaContent.UtcLastModificationDate = mediaContent.UtcCreationDate = DateTime.UtcNow;
            mediaContent.Metadata = metadata;
            Provider.Add(mediaContent, @overrided);

            return mediaContent;
        }
        public virtual MediaContent Update(Repository repository, MediaFolder mediaFolder, string uuid, string fileName, Stream fileStream, string userid = "")
        {
            return Update(repository, mediaFolder, uuid, fileName, fileStream, userid, null);
        }
        public virtual MediaContent Update(Repository repository, MediaFolder mediaFolder, string uuid, string fileName, Stream fileStream,
            string userid, MediaContentMetadata metadata)
        {
            IsAllowExtension(fileName, mediaFolder.AllowedExtensions);


            var binaryContent = mediaFolder.CreateQuery().WhereEquals("UUID", uuid).First();
            var old = new MediaContent(binaryContent);
            binaryContent.UserId = userid;
            binaryContent.UtcLastModificationDate = DateTime.UtcNow;

            binaryContent.FileName = fileName;
            binaryContent.UserKey = fileName;
            binaryContent.UUID = fileName;

            if (fileStream != null)
            {
                binaryContent.ContentFile = new ContentFile()
                {
                    Name = fileName,
                    FileName = fileName,
                    Stream = fileStream
                };
                Provider.SaveContentStream(old, fileStream);
            }

            binaryContent.Metadata = metadata;
            Provider.Update(binaryContent, old);

            return binaryContent;
        }

        public virtual void Update(MediaFolder folder, string uuid, IEnumerable<string> fieldNames, IEnumerable<object> fieldValues)
        {
            var mediaFolder = (MediaFolder)folder;
            var content = mediaFolder.CreateQuery().WhereEquals("uuid", uuid).FirstOrDefault();

            if (content != null)
            {
                var newContent = new MediaContent(content);
                var names = fieldNames.ToArray();
                var values = fieldValues.ToArray();
                for (int i = 0; i < names.Length; i++)
                {
                    newContent[names[i]] = values[i];
                }

                Provider.Update(newContent, content);
            }

        }
        private bool IsAllowExtension(string fileName, IEnumerable<string> extensionArr)
        {
            if (extensionArr != null)
            {
                var extension = fileName.Substring(fileName.LastIndexOf('.') + 1);
                if (!extensionArr.Contains(extension))
                {
                    throw new FriendlyException("Current folder doesn't support " + extension);
                }
            }
            return true;
        }

        public virtual void Delete(Repository repository, MediaFolder mediaFolder, string uuid)
        {
            var mediaContent = mediaFolder.CreateQuery().WhereEquals("UUID", uuid).First();
            Provider.Delete(mediaContent);

        }

        public virtual void Move(Repository repository, string sourceFolder, string oldFileName, string targetFolder, string newFileName)
        {
            var source = new MediaFolder(repository, sourceFolder);
            var target = new MediaFolder(repository, targetFolder);
            Provider.Move(source, oldFileName, target, newFileName);
        }
        #endregion

    }
}
