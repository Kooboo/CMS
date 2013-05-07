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
using NCMIS.Provider;
using NCMIS.Produce;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using NCMIS.ObjectModel.MetaData;
namespace Kooboo.CMS.Content.Interoperability.CMIS
{
    public class ObjectProvider : ObjectProviderBase
    {
        public override NCMIS.ObjectModel.CmisObject CreateDocument(string repositoryId, NCMIS.ObjectModel.CmisProperties properties, string folderId, NCMIS.ObjectModel.ContentStream contentStream, VersioningState versioningState, string[] policies, NCMIS.AccessControl.AccessControlList addACEs, NCMIS.AccessControl.AccessControlList removeACEs)
        {
            DocumentObjectService documentService = (DocumentObjectService)ObjectService.GetService(typeof(ContentBase));
            return documentService.CreateDocument(repositoryId, properties, folderId, contentStream);
        }

        public override string CreateDocumentFromSource(string repositoryId, string sourceId, NCMIS.ObjectModel.CmisProperties properties, string folderId, VersioningState? versioningState, string[] policies, NCMIS.AccessControl.AccessControlList addACEs, NCMIS.AccessControl.AccessControlList removeACEs)
        {
            DocumentObjectService documentService = (DocumentObjectService)ObjectService.GetService(typeof(ContentBase));
            return documentService.CopyDocument(repositoryId, sourceId, properties, folderId);
        }

        public override NCMIS.ObjectModel.CmisObject CreateFolder(string repositoryId, NCMIS.ObjectModel.CmisProperties properties, string folderId, string[] policies, NCMIS.AccessControl.AccessControlList addACEs, NCMIS.AccessControl.AccessControlList removeACEs)
        {
            FolderObjectService folderObjectService = (FolderObjectService)ObjectService.GetService(typeof(Folder));

            return folderObjectService.CreateFolder(repositoryId, properties, folderId);
        }

        public override NCMIS.ObjectModel.CmisObject CreatePolicy(string repositoryId, NCMIS.ObjectModel.CmisProperties properties, string folderId, string[] policies, NCMIS.AccessControl.AccessControlList addACEs, NCMIS.AccessControl.AccessControlList removeACEs)
        {
            throw new NotImplementedException();
        }
        // only for content category relationship
        public override string CreateRelationship(string repositoryId, NCMIS.ObjectModel.CmisProperties properties, string[] policies, NCMIS.AccessControl.AccessControlList addACEs, NCMIS.AccessControl.AccessControlList removeACEs)
        {
            throw new NotImplementedException();
        }

        public override NCMIS.ObjectModel.TokenedDocumentId DeleteContentStream(string repositoryId, string documentId, string changeToken)
        {
            throw new NotSupportedException();
        }

        public override void DeleteObject(string repositoryId, string objectId, bool allVersions)
        {
            IObjectService objectService = ObjectService.GetService(objectId);
            objectService.DeleteObject(repositoryId, objectId);
        }

        public override string[] DeleteTree(string repositoryId, string folderId, bool? allVersions, UnfileObjects? unfileObject, bool? continueOnFailure)
        {
            throw new NotSupportedException();
        }

        public override AllowableActions GetAllowableActions(string repositoryId, string objectId)
        {
            IObjectService objectService = ObjectService.GetService(objectId);
            return objectService.GetAllowableActions(repositoryId, objectId);
        }

        public override NCMIS.ObjectModel.ContentStream GetContentStream(string repositoryId, string objectId, string streamId)
        {
            IObjectService objectService = ObjectService.GetService(objectId);
            return objectService.GetContentStream(repositoryId, objectId, streamId);
        }

        public override NCMIS.ObjectModel.CmisObject GetObject(string repositoryId, string objectId, string filter, bool includeAllowableActions, IncludeRelationships includeRelationships, string renditionFilter, bool includePolicyIds, bool includeAcl)
        {
            IObjectService objectService = ObjectService.GetService(objectId);
            return objectService.GetObject(repositoryId, objectId);
        }

        public override NCMIS.ObjectModel.CmisObject GetObjectByPath(string repositoryId, string path, string filter, bool? includeAllowableActions, IncludeRelationships? includeRelationships, string renditionFilter, bool? includePolicyIds, bool? includeACL)
        {
            throw new NotSupportedException();
        }

        public override NCMIS.ObjectModel.CmisProperties GetProperties(string repositoryId, string objectId, string filter)
        {
            IObjectService objectService = ObjectService.GetService(objectId);
            return objectService.GetProperties(repositoryId, objectId);
        }

        public override NCMIS.ObjectModel.Renditions GetRenditions(string repositoryId, string objectId, string renditionFilter, string maxItems, string skipCount)
        {
            throw new NotSupportedException();
        }

        public override void MoveObject(string repositoryId, string objectId, string targetFolderId, string sourceFolderId)
        {
            throw new NotSupportedException();
        }

        public override NCMIS.ObjectModel.TokenedDocumentId SetContentStream(string repositoryId, string documentId, NCMIS.ObjectModel.ContentStream contentStream, bool? overwriteFlag, string changeToken)
        {
            IObjectService objectService = ObjectService.GetService(documentId);
            objectService.SetContentStream(repositoryId, documentId, contentStream, overwriteFlag);
            return new NCMIS.ObjectModel.TokenedDocumentId() { ChangeToken = changeToken, DocumentId = documentId };
        }

        public override NCMIS.ObjectModel.TokenedCmisObjectId UpdateProperties(string repositoryId, string objectId, NCMIS.ObjectModel.CmisProperties properties, string changeToken)
        {
            IObjectService objectService = ObjectService.GetService(objectId);
            objectService.UpdateProperties(repositoryId, objectId, properties);
            return new NCMIS.ObjectModel.TokenedCmisObjectId() { ChangeToken = changeToken, ObjectId = objectId };
        }
    }
}
