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
using Kooboo.CMS.Content.Models;
using NCMIS.ObjectModel;
using System.IO;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Services;

namespace Kooboo.CMS.Content.Interoperability.CMIS
{
    public class DocumentObjectService : IObjectService
    {
        static string UserId = "CMIS";
        private string prefix = "_content";

        public CmisObject CreateDocument(string repositoryId, NCMIS.ObjectModel.CmisProperties properties, string folderId, NCMIS.ObjectModel.ContentStream contentStream)
        {
            FolderObjectService folderObjectService = (FolderObjectService)ObjectService.GetService(typeof(Folder));
            if (folderObjectService.IsSystemFolder(folderId))
            {
                throw new Exception("Could not create document under system folder.");
            }
            Kooboo.CMS.Content.Models.Repository repository = new Repository(repositoryId);


            string objectId = folderId;
            folderObjectService.TryPraseObjectId(folderId, out folderId);
            var folder = CmisFolderHelper.Parse(repository, folderId);

            if (folder is TextFolder)
            {
                var textFolder = (TextFolder)folder.AsActual();

                var content = Services.ServiceFactory.TextContentManager.Add(repository, textFolder, UserId, properties.ToNameValueCollection(), contentStream.ToFileCollection(), null);

                return ObjectConvertor.ToCmis(content, false);
            }
            else if (folder is MediaFolder)
            {
                var mediaFolder = (MediaFolder)folder.AsActual();
                if (contentStream != null)
                {
                    var content = Services.ServiceFactory.MediaContentManager.Add(repository, mediaFolder, UserId, contentStream.Filename, new MemoryStream(contentStream.Stream));
                    return ObjectConvertor.ToCmis(content, false);
                }
            }
            return ObjectConvertor.EmptyCmisObject();
        }

        public string CopyDocument(string repositoryId, string sourceId, NCMIS.ObjectModel.CmisProperties properties, string folderId)
        {
            string id;
            if (!TryPraseObjectId(sourceId, out id))
                throw new Exception("Invalid docuemnt id. parameter name \"sourceId\"");
            string contentUUID;
            Kooboo.CMS.Content.Models.Repository repository = new Repository(repositoryId);
            var sourceFolder = ParseDocumentId(repository, id, out contentUUID);

            IContentManager contentManager;
            IContentQuery<ContentBase> contentQuery;
            if (sourceFolder is TextFolder)
            {
                contentManager = ServiceFactory.TextContentManager;
                contentQuery = ((TextFolder)sourceFolder).CreateQuery().WhereEquals("UUID", contentUUID);
            }
            else
            {
                contentManager = ServiceFactory.MediaContentManager;
                contentQuery = ((MediaFolder)sourceFolder).CreateQuery().WhereEquals("UUID", contentUUID);
            }
            FolderObjectService folderObjectService = (FolderObjectService)ObjectService.GetService(typeof(Folder));
            string objectId = folderId;
            folderObjectService.TryPraseObjectId(folderId, out folderId);
            var targetFolder = CmisFolderHelper.Parse(repository, folderId);

            var content = contentManager.Copy(contentQuery.First(), targetFolder, null, properties.ToNameValueCollection());

            return GetObjectId(content);
        }

        #region IObjectService Members

        public string GetObjectId(object o)
        {
            var content = (ContentBase)o;
            Folder folder = content.GetFolder();
            return prefix + CmisFolderHelper.CompositeFolderId(folder) + "$$" + content.UUID;
        }

        public bool TryPraseObjectId(string objectId, out string id)
        {
            id = string.Empty;
            if (objectId.StartsWith(prefix))
            {
                id = objectId.Substring(prefix.Length);

                return true;
            }
            return false;
        }
        private Folder ParseDocumentId(Repository repository, string id, out string contentUUID)
        {
            string[] idArr = id.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            contentUUID = idArr[1];
            return CmisFolderHelper.Parse(repository, idArr[0]);
        }

        public NCMIS.ObjectModel.CmisObject GetObject(string objectId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NCMIS.ObjectModel.CmisObject> All(string repositoryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NCMIS.ObjectModel.CmisObject> GetChildren(string repositoryId, string objectId, string filter, NCMIS.Produce.IncludeRelationships includeRelationships)
        {
            throw new NotImplementedException();
        }

        public void DeleteObject(string repositoryId, string objectId)
        {
            string id;
            if (!TryPraseObjectId(objectId, out id))
                throw new Exception("Invalid docuemnt id. parameter name \"objectId\"");
            string contentUUID;
            Kooboo.CMS.Content.Models.Repository repository = new Repository(repositoryId);
            var sourceFolder = ParseDocumentId(repository, id, out contentUUID);

            if (sourceFolder is TextFolder)
            {
                ServiceFactory.TextContentManager.Delete(repository, sourceFolder, contentUUID);
            }
            else
            {
                ServiceFactory.MediaContentManager.Delete(repository, sourceFolder, contentUUID);
            }
        }


        public NCMIS.ObjectModel.CmisObject GetParent(string repositoryId, string objectId)
        {
            throw new NotImplementedException();
        }


        public NCMIS.ObjectModel.MetaData.AllowableActions GetAllowableActions(string repositoryId, string objectId)
        {
            return new NCMIS.ObjectModel.MetaData.AllowableActions()
            {
                CanAddObjectToFolder = false,
                CanApplyACL = false,
                CanApplyPolicy = false,
                CanCancelCheckOut = false,
                CanCheckIn = false,
                CanCheckOut = false,
                CanCreateDocument = false,
                CanCreateFolder = false,
                CanCreateRelationship = true,
                CanDeleteContentStream = false,
                CanDeleteObject = true,
                CanDeleteTree = false,
                CanGetACL = false,
                CanGetAllVersions = false,
                CanGetAppliedPolicies = false,
                CanGetChildren = false,
                CanGetContentStream = true,
                CanGetDescendants = false,
                CanGetFolderParent = false,
                CanGetFolderTree = false,
                CanGetObjectParents = false,
                CanGetObjectRelationships = true,
                CanGetProperties = true,
                CanGetRenditions = false,
                CanMoveObject = false,
                CanRemoveObjectFromFolder = false,
                CanRemovePolicy = false,
                CanSetContentStream = true,
                CanUpdateProperties = true

            };
        }

        public ContentStream GetContentStream(string repositoryId, string objectId, string streamId)
        {
            //Kooboo.CMS.Content.Models.Repository repository = new Repository(repositoryId);
            //string id;
            //if (!TryPraseObjectId(objectId, out id))
            //    throw new Exception("Invalid docuemnt id. parameter name \"objectId\"");
            //string contentUUID;
            //var sourceFolder = ParseDocumentId(repository, id, out contentUUID);

            //if (sourceFolder is TextFolder)
            //{
            //    var textFolder = (TextFolder)sourceFolder;
            //    var content = textFolder.CreateQuery().WhereEquals("UUID", contentUUID).First();
            //    var fileVirtualPath = content[streamId];
            //}
            //else
            //{

            //}
            throw new NotImplementedException();

        }

        public CmisObject GetObject(string repositoryId, string objectId)
        {
            throw new NotImplementedException();
        }

        public CmisProperties GetProperties(string repositoryId, string objectId)
        {
            throw new NotImplementedException();
        }

        public void SetContentStream(string repositoryId, string documentId, ContentStream contentStream, bool? overwriteFlag)
        {
            throw new NotImplementedException();
        }

        public void UpdateProperties(string repositoryId, string objectId, CmisProperties properties)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
