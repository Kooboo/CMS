#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Modules.CMIS.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Services.Implementation
{
    //[Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(INavigationService))]
    public partial class Service : INavigationService
    {
        //#region .ctor
        //TextFolderManager _textFolderManager;
        //public NavigationService(TextFolderManager textFolderManager)
        //{
        //    _textFolderManager = textFolderManager;
        //}
        //#endregion

        #region GetChildren

        private static cmisObjectInFolderType ToPathedCmisObject(Kooboo.CMS.Content.Models.TextFolder textFolder)
        {
            cmisObjectInFolderType pathedCmisObject = new cmisObjectInFolderType();
            pathedCmisObject.pathSegment = textFolder.FullName;

            var cmisObject = ToCmisObject(textFolder);
            pathedCmisObject.@object = cmisObject;
            return pathedCmisObject;
        }

        private static cmisObjectType ToCmisObject(Kooboo.CMS.Content.Models.TextFolder textFolder)
        {
            var hasSchema = !string.IsNullOrEmpty(textFolder.SchemaName);
            var cmisObject = new cmisObjectType();
            cmisObject.allowableActions = new cmisAllowableActionsType()
            {
                canAddObjectToFolder = hasSchema,
                canApplyACL = false,
                canApplyPolicy = false,
                canCancelCheckOut = false,
                canCheckIn = false,
                canCheckOut = false,
                canCreateDocument = hasSchema,
                canCreateFolder = true,
                canCreateRelationship = hasSchema,
                canDeleteContentStream = true,
                canDeleteObject = hasSchema,
                canDeleteTree = false,
                canGetACL = false,
                canGetAllVersions = false,
                canGetAppliedPolicies = false,
                canGetChildren = true,
                canGetContentStream = false,
                canGetDescendants = false,
                canGetFolderParent = true,
                canGetFolderTree = true,
                canGetObjectParents = false,
                canGetObjectRelationships = false,
                canGetProperties = false,
                canGetRenditions = false,
                canMoveObject = false,
                canRemoveObjectFromFolder = false,
                canRemovePolicy = false,
                canSetContentStream = false,
                canUpdateProperties = false
            };
            cmisObject.changeEventInfo = null;
            cmisObject.exactACL = false;
            cmisObject.policyIds = null;
            cmisObject.rendition = null;

            cmisObject.properties = new cmisPropertiesType();
            var properties = new List<cmisProperty>();
            properties.Add(new cmisPropertyId()
            {
                displayName = "Id",
                localName = "Id",
                propertyDefinitionId = CmisPropertyDefinitionId.ObjectId,
                queryName = "Id",
                value = new string[] { textFolder.FriendlyName }
            });
            properties.Add(new cmisPropertyString()
            {
                displayName = "Name",
                localName = "Name",
                propertyDefinitionId = CmisPropertyDefinitionId.Name,
                queryName = "Name",
                value = new string[] { textFolder.FriendlyName }
            });
            properties.Add(new cmisPropertyString()
            {
                displayName = "DisplayName",
                localName = "DisplayName",
                queryName = "DisplayName",
                value = new string[] { textFolder.DisplayName }
            });
            properties.Add(new cmisPropertyDateTime()
            {
                displayName = "UtcCreationDate",
                localName = "UtcCreationDate",
                propertyDefinitionId = CmisPropertyDefinitionId.CreationDate,
                queryName = "UtcCreationDate",
                value = new DateTime[] { textFolder.UtcCreationDate }
            });
            properties.Add(new cmisPropertyString()
            {
                displayName = "UserId",
                localName = "UserId",
                propertyDefinitionId = CmisPropertyDefinitionId.CreatedBy,
                queryName = "UserId",
                value = new string[] { textFolder.UserId }
            });
            properties.Add(new cmisPropertyBoolean()
            {
                displayName = "EnablePaging",
                localName = "EnablePaging",
                queryName = "EnablePaging",
                value = new bool[] { textFolder.EnablePaging == null ? true : textFolder.EnablePaging.Value }
            });
            properties.Add(new cmisPropertyBoolean()
            {
                displayName = "Sortable",
                localName = "Sortable",
                queryName = "Sortable",
                value = new bool[] { textFolder.Sortable == null ? true : textFolder.Sortable.Value }
            });
            properties.Add(new cmisPropertyString()
            {
                displayName = "SchemaName",
                localName = "SchemaName",
                queryName = "SchemaName",
                value = new string[] { textFolder.SchemaName }
            });
            properties.Add(new cmisPropertyBoolean()
            {
                displayName = "Hidden",
                localName = "Hidden",
                queryName = "Hidden",
                value = new bool[] { textFolder.Hidden == null ? false : textFolder.Hidden.Value }
            });
            properties.Add(new cmisPropertyInteger()
            {
                displayName = "PageSize",
                localName = "PageSize",
                queryName = "PageSize",
                value = new string[] { textFolder.PageSize.ToString() }
            });
            cmisObject.properties.Items = properties.ToArray();
            return cmisObject;
        }

        public getChildrenResponse GetChildren(getChildrenRequest request)
        {
            var resultList = new cmisObjectInFolderListType();

            var repository = ModelHelper.GetRepository(request.repositoryId);


            IEnumerable<Kooboo.CMS.Content.Models.TextFolder> childFolders = null;
            if (request.folderId == "/")
            {
                childFolders = _textFolderManager.All(repository, null);
            }
            else
            {
                var textFolder = ModelHelper.GetTextFolder(request.repositoryId, request.folderId);
                childFolders = _textFolderManager.ChildFolders(textFolder);
            }

            resultList.numItems = childFolders.Count().ToString();
            var skipCount = 0;
            if (!string.IsNullOrEmpty(request.skipCount))
            {
                skipCount = request.skipCount.As<int>();
            }
            childFolders = childFolders.Skip(skipCount);
            if (string.IsNullOrEmpty(request.maxItems))
            {
                var maxItems = request.maxItems.As<int>();
                childFolders = childFolders.Take(maxItems);
            }
            resultList.objects = childFolders.Select(it => ToPathedCmisObject(it)/*ToPathedCmisObject(it)*/).ToArray();
            return new getChildrenResponse(resultList);
        }
        #endregion

        #region GetDescendants
        public getDescendantsResponse GetDescendants(getDescendantsRequest request)
        {
            throw new FaultException<cmisFaultType>(ModelHelper.CreateFault(enumServiceException.notSupported));
        }
        #endregion

        #region GetFolderTree
        private cmisObjectInFolderContainerType ToPathedCmisObjectContainer(Kooboo.CMS.Content.Models.TextFolder textFolder, int? maxDepth)
        {
            if (maxDepth != null && textFolder.NamePaths.Length > maxDepth.Value)
            {
                return null;
            }
            cmisObjectInFolderContainerType container = new cmisObjectInFolderContainerType();
            container.objectInFolder = ToPathedCmisObject(textFolder);
            container.children = _textFolderManager.ChildFolders(textFolder).Select(it => ToPathedCmisObjectContainer(it, maxDepth)).Where(it => it != null).ToArray();
            return container;
        }
        public getFolderTreeResponse GetFolderTree(getFolderTreeRequest request)
        {
            var resultList = new cmisObjectListType();
            var repository = ModelHelper.GetRepository(request.repositoryId);

            cmisObjectInFolderContainerType root = new cmisObjectInFolderContainerType();
            IEnumerable<Kooboo.CMS.Content.Models.TextFolder> childFolders = null;
            if (request.folderId == "/")
            {
                root.objectInFolder = new cmisObjectInFolderType() { pathSegment = "/" };
                childFolders = _textFolderManager.All(repository, null);
            }
            else
            {
                var textFolder = ModelHelper.GetTextFolder(request.repositoryId, request.folderId);
                root.objectInFolder = ToPathedCmisObject(textFolder);
                childFolders = _textFolderManager.ChildFolders(textFolder);
            }
            int? maxDepth = null;
            if (!string.IsNullOrEmpty(request.depth))
            {
                maxDepth = request.depth.As<int>();
            }
            root.children = childFolders.Select(it => ToPathedCmisObjectContainer(it, maxDepth)).Where(it => it != null).ToArray();

            return new getFolderTreeResponse(new[] { root });
        }
        #endregion

        #region GetFolderParent
        public getFolderParentResponse GetFolderParent(getFolderParentRequest request)
        {
            var textFolder = ModelHelper.GetTextFolder(request.repositoryId, request.folderId);
            if (textFolder.Parent != null)
            {
                return new getFolderParentResponse(ToCmisObject(((Kooboo.CMS.Content.Models.TextFolder)textFolder.Parent).AsActual()));
            }
            else
            {
                var cmisObject = new cmisObjectType();
                cmisObject.properties = new cmisPropertiesType();
                cmisObject.properties.Items = new cmisProperty[]{
                    new cmisPropertyId()
                    {
                        displayName = "Id",
                        localName = "Id",
                        propertyDefinitionId = CmisPropertyDefinitionId.ObjectId,
                        queryName = "Id",
                        value = new string[] { "/" }
                    }
                };
                return new getFolderParentResponse(cmisObject);
            }
        }
        #endregion

        #region GetObjectParents
        public getObjectParentsResponse GetObjectParents(getObjectParentsRequest request)
        {
            throw new FaultException<cmisFaultType>(ModelHelper.CreateFault(enumServiceException.notSupported));
        }
        #endregion

        #region GetCheckedoutDocs
        public getCheckedOutDocsResponse GetCheckedoutDocs(getCheckedOutDocsRequest request)
        {
            throw new FaultException<cmisFaultType>(ModelHelper.CreateFault(enumServiceException.notSupported));
        }
        #endregion
    }
}
