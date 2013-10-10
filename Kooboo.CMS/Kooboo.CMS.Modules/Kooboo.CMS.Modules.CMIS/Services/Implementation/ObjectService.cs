#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Modules.CMIS.Models;
using Kooboo.CMS.Content.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Services.Implementation
{
    //[Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IObjectService))]
    public partial class Service : IObjectService
    {
        //#region .ctor
        //TextContentManager _textContentManager;
        //public ObjectService(TextContentManager textContentManager)
        //{
        //    _textContentManager = textContentManager;
        //}
        //#endregion

        #region CreateDocument

        public createDocumentResponse CreateDocument(createDocumentRequest request)
        {
            var site = ModelHelper.GetSite(request.repositoryId);
            var textFolder = ModelHelper.GetTextFolder(request.repositoryId, request.folderId);
            var nameValueCollection = request.properties.ToNameValueCollection();
            var inegrateId = _incomeDataManager.AddTextContent(site, textFolder, nameValueCollection, null, null, "", ContextHelper.GetVendor());
            return new createDocumentResponse(inegrateId, null);
        }
        #endregion

        #region CreateDocumentFromSource
        public createDocumentFromSourceResponse CreateDocumentFromSource(createDocumentFromSourceRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CreateFolder
        public createFolderResponse CreateFolder(createFolderRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CreateRelationship
        public createRelationshipResponse CreateRelationship(createRelationshipRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CreatePolicy
        public createPolicyResponse CreatePolicy(createPolicyRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CreateItem
        //public string CreateItem(string repositoryId, cmisPropertiesType properties, string folderId, string[] policies, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

        #region GetAllowableActions
        public getAllowableActionsResponse GetAllowableActions(getAllowableActionsRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GetObject
        public getObjectResponse GetObject(getObjectRequest request)
        {
            var response = new getObjectResponse();
            var integrateId = new ContentIntegrateId(request.objectId);

            var repository = ModelHelper.GetRepository(request.repositoryId);
            var folder = ModelHelper.GetTextFolder(request.repositoryId, integrateId.FolderName);
            var uuid = integrateId.ContentUUID;

            var content = folder.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();


            if (content != null)
            {
                response.@object = ModelHelper.TocmisObjectType(content);
            }
            return response;
        }
        #endregion

        #region GetProperties
        public getPropertiesResponse GetProperties(getPropertiesRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GetObjectByPath
        public getObjectByPathResponse GetObjectByPath(getObjectByPathRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GetContentStream
        public getContentStreamResponse GetContentStream(getContentStreamRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GetRenditions
        public getRenditionsResponse GetRenditions(getRenditionsRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region UpdateProperties

        public updatePropertiesResponse UpdateProperties(updatePropertiesRequest request)
        {
            var site = ModelHelper.GetSite(request.repositoryId);
            var integrateId = new Kooboo.CMS.Content.Models.ContentIntegrateId(request.objectId);

            var textFolder = ModelHelper.GetTextFolder(request.repositoryId, integrateId.FolderName);
            var nameValueCollection = request.properties.ToNameValueCollection();
            var integrateUUID = _incomeDataManager.UpdateTextContent(site, textFolder, integrateId.Id, nameValueCollection, "", ContextHelper.GetVendor());
            return new updatePropertiesResponse(integrateUUID, null, null);
        }
        #endregion

        #region MoveObject
        public moveObjectResponse MoveObject(moveObjectRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DeleteObject
        public deleteObjectResponse DeleteObject(deleteObjectRequest request)
        {
            var integrateId = new Kooboo.CMS.Content.Models.ContentIntegrateId(request.objectId);
            var site = ModelHelper.GetSite(request.repositoryId);
            var textFolder = ModelHelper.GetTextFolder(request.repositoryId, integrateId.FolderName);
            _incomeDataManager.DeleteTextContent(site, textFolder, integrateId.Id, ContextHelper.GetVendor());

            return new deleteObjectResponse();
        }
        #endregion

        #region DeleteTree
        public deleteTreeResponse DeleteTree(deleteTreeRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region SetContentStream
        public setContentStreamResponse SetContentStream(setContentStreamRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region AppendContentStream
        //public DataModel.TokenedDocumentId AppendContentStream(string repositoryId, string objectId, cmisContentStreamType contentStream, bool isLastChunk, string changeToken)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

        #region DeleteContentStream
        public deleteContentStreamResponse DeleteContentStream(deleteContentStreamRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
