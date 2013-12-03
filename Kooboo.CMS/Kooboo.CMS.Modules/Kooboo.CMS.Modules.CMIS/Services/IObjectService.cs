#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Modules.CMIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Services
{
    /// <summary>
    /// CMIS provides id-based CRUD (Create, Retrieve, Update, Delete) operations on objects in a repository. 
    /// </summary>
    [ServiceContract(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/", Name = "ObjectService")]
    [XmlSerializerFormat]
    public partial interface IObjectService
    {
        /// <summary>
        /// Creates a document object of the speciﬁed type (given by the cmis:objectTypeId property) in the (optionally) speciﬁed location. 
        /// </summary>        
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="properties">Required. The property values that MUST be applied to the newly-created document object. </param>
        /// <param name="folderId">Optional. If speciﬁed, the identiﬁer for the folder that MUST be the parent folder for the newly-created document object. This parameter MUST be speciﬁed if the repository does NOT support the optional "unﬁling" capability. </param>
        /// <param name="contentStream">Optional. The content stream that MUST be stored for the newly-created document object. The method of passing the contentStream to the server and the encoding mechanism will be speciﬁed by each speciﬁc binding. MUST be required if the type requires it. </param>
        /// <param name="versioningState">Optional. An enumeration specifying what the versioning state of the newly-created object MUST be.</param>
        /// <param name="policies">Optional. A list of policy ids that MUST be applied to the newly-created document object. </param>
        /// <param name="addACEs">Optional. A list of ACEs that MUST be added to the newly-created document object, either using the ACL from folderId if speciﬁed, or being applied if no folderId is speciﬁed. </param>
        /// <param name="removeACEs">Optional. A list of ACEs that MUST be removed from the newly-created document object, either using the ACL from folderId if speciﬁed, or being ignored if no folderId is speciﬁed. </param>
        /// <returns>Id objectId: The id of the newly-created document. </returns>
        [OperationContract(Name = "createDocument")]
        createDocumentResponse CreateDocument(createDocumentRequest request);

        /// <summary>
        /// Creates a document object as a copy of the given source document in the (optionally) speciﬁed location. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="sourceId">Required. The identiﬁer for the source document. </param>
        /// <param name="properties">Optional. The property values that MUST be applied to the object. This list of properties SHOULD only contain properties whose values diﬀer from the source document. </param>
        /// <param name="folderId">Optional. If speciﬁed, the identiﬁer for the folder that MUST be the parent folder for the newly-created document object. This parameter MUST be speciﬁed if the repository does NOT support the optional "unﬁling" capability. </param>
        /// <param name="versioningState">Optional. An enumeration specifying what the versioning state of the newly-created object MUST be. </param>
        /// <param name="policies">Optional. A list of policy ids that MUST be applied to the newly-created document object. </param>
        /// <param name="addACEs">Optional. A list of ACEs that MUST be added to the newly-created document object, either using the ACL from folderId if speciﬁed, or being applied if no folderId is speciﬁed. </param>
        /// <param name="removeACEs">Optional. A list of ACEs that MUST be removed from the newly-created document object, either using the ACL from folderId if speciﬁed, or being ignored if no folderId is speciﬁed. </param>
        /// <returns>Id objectId: The id of the newly-created document. </returns>
        [OperationContract(Name = "createDocumentFromSource")]
        createDocumentFromSourceResponse CreateDocumentFromSource(createDocumentFromSourceRequest request);

        /// <summary>
        /// Creates a folder object of the speciﬁed type in the speciﬁed location. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="properties">Required. The property values that MUST be applied to the newly-created folder object. </param>
        /// <param name="folderId">Required. The identiﬁer for the folder that MUST be the parent folder for the newly-created folder object. </param>
        /// <param name="policies">Optional. A list of policy ids that MUST be applied to the newly-created folder object. </param>
        /// <param name="addACEs">Optional. A list of ACEs that MUST be added to the newly-created folder object, either using the ACL from folderId if speciﬁed, or being applied if no folderId is speciﬁed. </param>
        /// <param name="removeACEs">Optional. A list of ACEs that MUST be removed from the newly-created folder object, either using the ACL from folderId if speciﬁed, or being ignored if no folderId is speciﬁed. </param>
        /// <returns>Id objectId: The id of the newly-created folder. </returns>
        [OperationContract(Name = "createFolder")]
        createFolderResponse CreateFolder(createFolderRequest request);

        /// <summary>
        /// Creates a relationship object of the speciﬁed type. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="properties">Required. The property values that MUST be applied to the newly-created relationship object. </param>
        /// <param name="policies">Optional. A list of policy ids that MUST be applied to the newly-created relationship object. </param>
        /// <param name="addACEs">Optional. A list of ACEs that MUST be added to the newly-created relationship object, either using the ACL from folderId if speciﬁed, or being applied if no folderId is speciﬁed. </param>
        /// <param name="removeACEs">Optional. A list of ACEs that MUST be removed from the newly-created relationship object, either using the ACL from folderId if speciﬁed, or being ignored if no folderId is speciﬁed. </param>
        /// <returns>Id objectId: The id of the newly-created relationship. </returns>
        [OperationContract(Name = "createRelationship")]
        createRelationshipResponse CreateRelationship(createRelationshipRequest request);

        /// <summary>
        /// Creates a policy object of the speciﬁed type. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="properties">Required. The property values that MUST be applied to the newly-created policy object. </param>
        /// <param name="folderId">Optional. If speciﬁed, the identiﬁer for the folder that MUST be the parent folder for the newly-created policy object. This parameter MUST be speciﬁed if the repository does NOT support the optional "unﬁling" capability. </param>
        /// <param name="policies">Optional. A list of policy ids that MUST be applied to the newly-created policy object. </param>
        /// <param name="addACEs">Optional. A list of ACEs that MUST be added to the newly-created policy object, either using the ACL from folderId if speciﬁed, or being applied if no folderId is speciﬁed. </param>
        /// <param name="removeACEs">Optional. A list of ACEs that MUST be removed from the newly-created policy object, either using the ACL from folderId if speciﬁed, or being ignored if no folderId is speciﬁed. </param>
        /// <returns>Id objectId: The id of the newly-created policy. </returns>
        [OperationContract(Name = "createPolicy")]
        createPolicyResponse CreatePolicy(createPolicyRequest request);

        /// <summary>
        /// Creates an item object of the speciﬁed type. New in CMIS 1.2
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="properties">Required. The property values that MUST be applied to the newly-created item object. </param>
        /// <param name="folderId">Optional. If speciﬁed, the identiﬁer for the folder that MUST be the parent folder for the newly-created item object. This parameter MUST be speciﬁed if the repository does NOT support the optional "unﬁling" capability. </param>
        /// <param name="policies">Optional. A list of policy ids that MUST be applied to the newly-created item object. </param>
        /// <param name="addACEs">Optional. A list of ACEs that MUST be added to the newly-created item object, either using the ACL from folderId if speciﬁed, or being applied if no folderId is speciﬁed. </param>
        /// <param name="removeACEs">Optional. A list of ACEs that MUST be removed from the newly-created item object, either using the ACL from folderId if speciﬁed, or being ignored if no folderId is speciﬁed. </param>
        /// <returns></returns>
        //[OperationContract(Name = "createItem")]
        //string CreateItem(string repositoryId, cmisPropertiesType properties, string folderId, string[] policies, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs);

        /// <summary>
        /// Gets the list of allowable actions for an object
        /// </summary>
        /// <param name="repositoryId">The identiﬁer for the repository. </param>
        /// <param name="objectId">The identiﬁer for the object. .</param>
        /// <returns><Array> AllowableActions AllowableActions</returns>
        [OperationContract(Name = "getAllowableActions")]
        getAllowableActionsResponse GetAllowableActions(getAllowableActionsRequest request);

        /// <summary>
        /// Gets the speciﬁed information for the object. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the object. </param>
        /// <param name="filter">Optional. Value indicating which properties for objects MUST be returned. This ﬁlter is a list of property query names and NOT a list of property ids. The query names of secondary type properties MUST follow the pattern <secondaryTypeQueryName>.<propertyQueryName>.Example: cmis:name,amount,worflow.stage.</param>
        /// <param name="includeAllowableActions">Optional. If TRUE, then the Repository MUST return the available actions for each object in the result set. Defaults to FALSE.</param>
        /// <param name="includeRelationships">Optional. Value indicating what relationships in which the objects returned participate MUST be returned, if any.</param>
        /// <param name="renditionFilter">Optional. The Repository MUST return the set of renditions whose kind matches this ﬁlter. See section below for the ﬁlter grammar. Defaults to "cmis:none".</param>
        /// <param name="includePolicyIds">Optional. If TRUE, then the Repository MUST return the Ids of the policies applied to the object. Defaults to FALSE.</param>
        /// <param name="includeAcl">Optional. If TRUE, then the repository MUST return the ACLs for each object in the result set. Defaults to FALSE.</param>
        /// <returns></returns>
        [OperationContract(Name = "getObject")]
        getObjectResponse GetObject(getObjectRequest request);

        /// <summary>
        /// Gets the list of properties for the object. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the object. </param>
        /// <param name="filter">Optional. Value indicating which properties for objects MUST be returned. This ﬁlter is a list of property query names and NOT a list of property ids. The query names of secondary type properties MUST follow the pattern <secondaryTypeQueryName>.<propertyQueryName>.Example: cmis:name,amount,worflow.stage.</param>
        /// <returns></returns>
        [OperationContract(Name = "getProperties")]
        getPropertiesResponse GetProperties(getPropertiesRequest request);

        /// <summary>
        /// Gets the speciﬁed information for the object. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="path">Required. The path to the object. if folder A is under the root, and folder B is under A, then the path would be /A/B http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html#x1-520003</param>
        /// <param name="filter">Optional. Value indicating which properties for objects MUST be returned. This ﬁlter is a list of property query names and NOT a list of property ids. The query names of secondary type properties MUST follow the pattern <secondaryTypeQueryName>.<propertyQueryName>.Example: cmis:name,amount,worflow.stage.</param>
        /// <param name="includeAllowableActions">Optional. If TRUE, then the Repository MUST return the available actions for each object in the result set. Defaults to FALSE.</param>
        /// <param name="includeRelationships">Optional. Value indicating what relationships in which the objects returned participate MUST be returned, if any.</param>
        /// <param name="renditionFilter">Optional. The Repository MUST return the set of renditions whose kind matches this ﬁlter. See section below for the ﬁlter grammar. Defaults to "cmis:none".</param>
        /// <param name="includePolicyIds">Optional. If TRUE, then the Repository MUST return the Ids of the policies applied to the object. Defaults to FALSE.</param>
        /// <param name="includeAcl">Optional. If TRUE, then the repository MUST return the ACLs for each object in the result set. Defaults to FALSE.</param>
        /// <returns></returns>
        [OperationContract(Name = "getObjectByPath")]
        getObjectByPathResponse GetObjectByPath(getObjectByPathRequest request);

        /// <summary>
        /// Gets the content stream for the speciﬁed document object, or gets a rendition stream for a speciﬁed rendition of a document or folder object. 
        /// Notes: Each CMIS protocol binding MAY provide a way for fetching a sub-range within a content stream, in a manner appropriate to that protocol. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the object. </param>
        /// <param name="streamId">Optional. The identiﬁer for the rendition stream, when used to get a rendition stream. For documents, if not provided then this method returns the content stream. For folders, it MUST be provided. </param>
        /// <returns>The speciﬁed content stream or rendition stream for the object. </returns>
        [OperationContract(Name = "getContentStream")]
        getContentStreamResponse GetContentStream(getContentStreamRequest request);

        /// <summary>
        /// Gets the list of associated renditions for the speciﬁed object. Only rendition attributes are returned, not rendition stream. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the object. </param>
        /// <param name="renditionFilter">The rendition filter.</param>
        /// <param name="maxItems">(optional) This is the maximum number of items to return in a response. The repository MUST NOT exceed this maximum. Default is repository-speciﬁc. http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html#x1-1510001 </param>
        /// <param name="skipCount">(optional) This is the number of potential results that the repository MUST skip/page over before returning any results. Defaults to 0.</param>
        /// <returns>The set of renditions available on this object. </returns>
        [OperationContract(Name = "getRenditions")]
        getRenditionsResponse GetRenditions(getRenditionsRequest request);

        /// <summary>
        /// Updates properties and secondary types of the speciﬁed object. 
        /// Notes: 
        /// •A repository MAY automatically create new document versions as part of an update properties operation. Therefore, the objectId output NEED NOT be identical to the objectId input. 
        /// •Only properties whose values are diﬀerent than the original value of the object SHOULD be provided.
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the object. </param>
        /// <param name="properties">Required. The updated property values that MUST be applied to the object. </param>
        /// <param name="changeToken">Optional. http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html#x1-1610003</param>
        /// <returns></returns>
        [OperationContract(Name = "updateProperties")]
        updatePropertiesResponse UpdateProperties(updatePropertiesRequest request);

        /// <summary>
        /// Moves the speciﬁed ﬁle-able object from one folder to another.
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <param name="objectId">Required. The identiﬁer for the object.</param>
        /// <param name="targetFolderId">Required. The folder into which the object is to be moved.</param>
        /// <param name="sourceFolderId">Required. The folder from which the object is to be moved.</param>
        /// <returns>The identiﬁer for the object. The identiﬁer SHOULD NOT change. If the repository has to change the id, this is the new identiﬁer for the object. </returns>
        [OperationContract(Name = "moveObject")]
        moveObjectResponse MoveObject(moveObjectRequest request);

        /// <summary>
        /// Deletes the speciﬁed object.
        /// Notes: If the object is a PWC the checkout is discarded. See section http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html#x1-980003
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <param name="objectId">Required. The identiﬁer for the object.</param>
        /// <param name="allVersions">Optional. If TRUE (default), then delete all versions of the document. If FALSE, delete only the document object speciﬁed. The repository MUST ignore the value of this parameter when this service is invoke on a non-document object or non-versionable document object. </param>
        [OperationContract(Name = "deleteObject")]
        deleteObjectResponse DeleteObject(deleteObjectRequest request);

        /// <summary>
        /// Deletes the speciﬁed folder object and all of its child- and descendant-objects. 
        /// Notes: 
        /// •A repository MAY attempt to delete child- and descendant-objects of the speciﬁed folder in any order. 
        /// •Any child- or descendant-object that the repository cannot delete MUST persist in a valid state in the CMIS domain model. 
        /// •This service is not atomic. 
        /// •However, if deletesinglefiled is chosen and some objects fail to delete, then single-ﬁled objects are either deleted or kept, never just unﬁled. This is so that a user can call this command again to recover from the error by using the same tree.
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <param name="folderId">Required. The identiﬁer of the folder to be deleted. </param>
        /// <param name="allVersions">Optional. If TRUE (default), then delete all versions of all documents. If FALSE, delete only the document versions referenced in the tree. The repository MUST ignore the value of this parameter when this service is invoked on any non-document objects or non-versionable document objects. </param>
        /// <param name="unfileObject">Optional.An enumeration specifying how the repository MUST process ﬁle-able child- or descendant-objects.</param>
        /// <param name="continueOnFailure">Optional.  If TRUE, then the repository SHOULD continue attempting to perform this operation even if deletion of a child- or descendant-object in the speciﬁed folder cannot be deleted. If FALSE (default), then the repository SHOULD abort this method when it fails to delete a single child object or descendant object. </param>
        /// <returns> A list of identiﬁers of objects in the folder tree that were not deleted. </returns>
        [OperationContract(Name = "deleteTree")]
        deleteTreeResponse DeleteTree(deleteTreeRequest request);

        /// <summary>
        ///  Sets the content stream for the speciﬁed document object. 
        ///  Notes: A repository MAY automatically create new document versions as part of this service operations. Therefore, the objectId output NEED NOT be identical to the objectId input. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <param name="objectId">Required. The identiﬁer for the document object.</param>
        /// <param name="contentStream">Required. The content stream.</param>
        /// <param name="overwriteFlag">Optional.  If TRUE (default), then the repository MUST replace the existing content stream for the object (if any) with the input contentStream. If FALSE, then the repository MUST only set the input contentStream for the object if the object currently does not have a content stream. </param>
        /// <param name="changeToken">Optional. The change token.</param>
        /// <returns></returns>
        [OperationContract(Name = "setContentStream")]
        setContentStreamResponse SetContentStream(setContentStreamRequest request);

        /// <summary>
        /// Appends to the content stream for the speciﬁed document object. New in CMIS 1.2
        /// Notes: 
        /// •A repository MAY automatically create new document versions as part of this service method. Therefore, the objectId output NEED NOT be identical to the objectId input. 
        /// •The document may or may not have a content stream prior to calling this service. If there is no content stream, this service has the eﬀect of setting the content stream with the value of the input contentStream. 
        /// •This service is intended to be used by a single client. It should support the upload of very huge content streams. The behavior is repository speciﬁc if multiple clients call this service in succession or in parallel for the same document.
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <param name="objectId">Required. The identiﬁer for the document object.</param>
        /// <param name="contentStream">Required. The content stream.</param>
        /// <param name="isLastChunk"> If TRUE, then this is the last chunk of the content and the client does not intend to send another chunk. If FALSE (default), then the repository should except another chunk from the client. 
        /// Clients SHOULD always set this parameter but repositories SHOULD be prepared that clients don’t provide it. 
        /// Repositories may use this ﬂag to trigger some sort of content processing. For example, only if isLastChunk is TRUE the repsoitory could generate renditions of the content. </param>
        /// <param name="changeToken">The change token.</param>
        /// <returns></returns>
        //[OperationContract(Name = "appendContentStream")]
        //TokenedDocumentId AppendContentStream(string repositoryId, string objectId, cmisContentStreamType contentStream, bool isLastChunk, string changeToken);

        /// <summary>
        /// Deletes the content stream for the speciﬁed document object. 
        /// Notes: A repository MAY automatically create new document versions as part of this service method. Therefore, the obejctId output NEED NOT be identical to the objectId input. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <param name="objectId">Required. The identiﬁer for the document object.</param>
        /// <param name="changeToken">The change token.</param>
        /// <returns></returns>
        [OperationContract(Name = "deleteContentStream")]
        deleteContentStreamResponse DeleteContentStream(deleteContentStreamRequest request);
    }
}
