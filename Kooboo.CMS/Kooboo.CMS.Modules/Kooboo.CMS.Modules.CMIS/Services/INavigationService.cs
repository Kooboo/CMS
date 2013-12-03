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
    /// The Navigation Services are used to traverse the folder hierarchy in a CMIS repository, and to locate documents that are checked out. 
    /// </summary>
    [ServiceContract(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/", Name = "NavigationService")]
    [XmlSerializerFormat]
    public partial interface INavigationService
    {
        /// <summary>
        /// When implemented in a derived class, gets the list of child objects contained in the specified folder.
        /// </summary>
        /// <param name="repositoryId">Required.The id of the repository.</param>
        /// <param name="folderId">Required.The folder id of the folder to retreive descendants for.</param>
        /// <param name="maxItems">Optional. The maximum number of items to return in the response. (Optional)</param>
        /// <param name="skipCount">Optional. The number of potential results to skip before returning any results.</param>
        /// <param name="orderBy">Optional. Specifies sort specification for returned result, must be a comma separated list of one or more column names.</param>
        /// <param name="filter">Optional. Value indicating which properties for Objects that must be returned.</param>
        /// <param name="includeRelationships">Optional. Value indicating what relationships in which the objects returned participate that must be returned, if any.</param>
        /// <param name="renditionFilter">Optional. Renditions whose kind matches this filter must be returned.</param>
        /// <param name="includeAllowableActions">Optional. If true, then the the available actions for each object in the result set must be returned.</param>
        /// <param name="includePathSegment">Optional. If true, then a PathSegment for each child object, for use in constructing that object’s path, must be returned.</param>
        /// <returns>Child objects for specified folder.</returns>
        [OperationContract(Name = "getChildren")]
        getChildrenResponse GetChildren(getChildrenRequest request);

        /// <summary>
        /// Gets the set of descendant objects contained in the specified folder or any of its child-folders.
        /// </summary>
        /// <param name="repositoryId">Required.The id of the repository.</param>
        /// <param name="folderId">Required.The folder id of the folder to retreive descendants for.</param>
        /// <param name="depth">The number of levels of depth in the type hierachy from which to return results. (Optional)</param>
        /// <param name="filter">Value indicating which properties for Objects that must be returned.</param>
        /// <param name="includeRelationships">Value indicating what relationships in which the objects returned participate that must be returned, if any.</param>
        /// <param name="renditionFilter">Renditions whose kind matches this filter must be returned.</param>
        /// <param name="includeAllowableActions">If true, then the the available actions for each object in the result set must be returned.</param>
        /// <param name="includePathSegment">If true, then a PathSegment for each child object, for use in constructing that object’s path, must be returned.</param>
        /// <returns>Descendant objects for specified folder.</returns>
        [OperationContract(Name = "getDescendants")]
        getDescendantsResponse GetDescendants(getDescendantsRequest request);

        /// <summary>
        /// Gets the set of descendant folder objects contained in the specified folder. A hierarchical feed comprising all the folders under a specified folder.
        /// </summary>
        /// <param name="repositoryId">Required. The id of the repository.</param>
        /// <param name="folderId">Required. The folder id of the folder to retreive tree for.</param>
        /// <param name="depth">The number of levels of depth in the type hierachy from which to return results. (Optional)</param>
        /// <param name="filter">Value indicating which properties for Objects that must be returned.</param>
        /// <param name="includeRelationships">Value indicating what relationships in which the objects returned participate that must be returned, if any.</param>
        /// <param name="renditionFilter">Renditions whose kind matches this filter must be returned.</param>
        /// <param name="includeAllowableActions">If true, then the the available actions for each object in the result set must be returned.</param>
        /// <param name="includePathSegment">If true, then a PathSegment for each child object, for use in constructing that object’s path, must be returned.</param>
        /// <returns>Descendant folder objects, for the specified folder.</returns>
        [OperationContract(Name = "getFolderTree")]
        getFolderTreeResponse GetFolderTree(getFolderTreeRequest request);

        /// <summary>
        /// Gets the parent folder object for the specified folder object.
        /// </summary>
        /// <param name="repositoryId">Required.The id of the repository.</param>
        /// <param name="folderId">Required.The folder id to retreive parent folder object for.</param>
        /// <param name="filter">Value indicating which properties for Objects that must be returned.</param>
        /// <returns>Parent folder object for specified folder.</returns>
        [OperationContract(Name = "getFolderParent")]
        getFolderParentResponse GetFolderParent(getFolderParentRequest request);

        /// <summary>
        ///  Gets the parent folder(s) for the speciﬁed ﬁleable object. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the object. </param>
        /// <param name="filter">The filter.</param>
        /// <param name="includeRelationships">Value indicating what relationships in which the objects returned participate that must be returned, if any.</param>
        /// <param name="renditionFilter">Renditions whose kind matches this filter must be returned.</param>
        /// <param name="includeAllowableActions">If true, then the the available actions for each object in the result set must be returned.</param>
        /// <param name="includeRelativePathSegment">http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html#x1-520003</param>
        /// <returns></returns>
        [OperationContract(Name = "getObjectParents")]
        getObjectParentsResponse GetObjectParents(getObjectParentsRequest request);

        /// <summary>
        /// Gets the checked out documents. 
        /// </summary>
        /// <param name="repositoryId">Required. The id of the repository.</param>
        /// <param name="folderId">The folder id of the folder to retreive checkedout documents for. If left out all checked out documents will be returned.</param>
        /// <param name="filter">Value indicating which properties for Objects that must be returned.</param>
        /// <param name="orderBy">Specifies sort specification for returned result, must be a comma separated list of one or more column names.</param>
        /// <param name="includeAllowableActions">If true, then the the available actions for each object in the result set must be returned.</param>
        /// <param name="includeRelationships">Value indicating what relationships in which the objects returned participate that must be returned, if any.</param>
        /// <param name="renditionFilter">Renditions whose kind matches this filter must be returned.</param>
        /// <param name="maxItems">The maximum number of items to return in the response. (optional)</param>
        /// <param name="skipCount">The number of potential results to skip before returning any results.</param>
        /// <returns>Checked out documents.</returns>
        [OperationContract(Name = "getCheckedoutDocs")]
        getCheckedOutDocsResponse GetCheckedoutDocs(getCheckedOutDocsRequest request);
    }
}
