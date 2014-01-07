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
    /// The Versioning services are used to navigate or update a document version series. 
    /// </summary>
    [ServiceContract(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/", Name = "VersioningService")]
    [XmlSerializerFormat]
    public interface IVersioningService
    {
        /// <summary>
        /// Create a private working copy (PWC) of the document. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <param name="objectId">Required. The identiﬁer for the document version object that should be checked out. </param>
        /// <returns></returns>
        [OperationContract(Name = "checkOut")]
        checkOutResponse CheckOut(checkOutRequest request);

        /// <summary>
        /// Reverses the eﬀect of a check-out
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <param name="objectId">Required. The identiﬁer of the Private Working Copy.</param>
        [OperationContract(Name = "cancelCheckOut")]
        cancelCheckOutResponse CancelCheckOut(cancelCheckOutRequest request);

        /// <summary>
        /// Checks-in the Private Working Copy document.
        /// Notes: 
        /// •For repositories that do NOT support the optional capabilityPWCUpdatable capability, the properties and contentStream input parameters MUST be provided on the checkIn service for updates to happen as part of checkIn. 
        /// •Each CMIS protocol binding MUST specify whether the checkin service MUST always include all updatable properties, or only those properties whose values are diﬀerent than the original value of the object.
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <param name="objectId">Required. The identiﬁer of the Private Working Copy.</param>
        /// <param name="major">Optional. TRUE (default) if the checked-in document object MUST be a major version. FALSE if the checked-in document object MUST NOT be a major version but a minor version. </param>
        /// <param name="properties">Optional. The property values that MUST be applied to the checked-in document object. </param>
        /// <param name="contentStream">Optional. The content stream that MUST be stored for the checked-in document object. The method of passing the contentStream to the server and the encoding mechanism will be speciﬁed by each speciﬁc binding. MUST be required if the type requires it. </param>
        /// <param name="checkinComment">Optional. The checkin comment.</param>
        /// <param name="policies">Optional. A list of policy ids that MUST be applied to the newly-created document object. </param>
        /// <param name="addACEs">Optional.  A list of ACEs that MUST be added to the newly-created document object. </param>
        /// <param name="removeACEs">Optional. A list of ACEs that MUST be removed from the newly-created document object.</param>
        /// <returns></returns>
        [OperationContract(Name = "checkIn")]
        checkInResponse CheckIn(checkInRequest request);

        /// <summary>
        /// Get the latest document object in the version series.
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the object. </param>
        /// <param name="versionSeriesId">Alternative of objectId. The identiﬁer for the version series. Actually, it is the object id.</param>
        /// <param name="major">Optional. If TRUE, then the repository MUST return the properties for the latest major version object in the version series. If FALSE (default), the repository MUST return the properties for the latest (major or non-major) version object in the version series. </param>
        /// <param name="filter">Optional. Value indicating which properties for objects MUST be returned. This ﬁlter is a list of property query names and NOT a list of property ids. The query names of secondary type properties MUST follow the pattern <secondaryTypeQueryName>.<propertyQueryName>.Example: cmis:name,amount,worflow.stage.</param>
        /// <param name="includeAllowableActions">Optional. If TRUE, then the Repository MUST return the available actions for each object in the result set. Defaults to FALSE.</param>
        /// <param name="includeRelationships">Optional. Value indicating what relationships in which the objects returned participate MUST be returned, if any.</param>
        /// <param name="renditionFilter">Optional. The Repository MUST return the set of renditions whose kind matches this ﬁlter. See section below for the ﬁlter grammar. Defaults to "cmis:none".</param>
        /// <param name="includePolicyIds">Optional. If TRUE, then the Repository MUST return the Ids of the policies applied to the object. Defaults to FALSE.</param>
        /// <param name="includeAcl">Optional. If TRUE, then the repository MUST return the ACLs for each object in the result set. Defaults to FALSE.</param>
        /// <returns></returns>
        [OperationContract(Name = "getObjectOfLatestVersion")]
        getObjectOfLatestVersionResponse GetObjectOfLatestVersion(getObjectOfLatestVersionRequest request);

        /// <summary>
        /// Get a subset of the properties for the latest document object in the version series. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the version series. </param>
        /// <param name="major">Optional. If TRUE, then the repository MUST return the properties for the latest major version object in the version series. If FALSE (default), the repository MUST return the properties for the latest (major or non-major) version object in the version series. </param>
        /// <param name="filter">Optional. Value indicating which properties for objects MUST be returned. This ﬁlter is a list of property query names and NOT a list of property ids. The query names of secondary type properties MUST follow the pattern <secondaryTypeQueryName>.<propertyQueryName>.Example: cmis:name,amount,worflow.stage.</param>
        /// <returns></returns>
        [OperationContract(Name = "getPropertiesOfLatestVersion")]
        getPropertiesOfLatestVersionResponse GetPropertiesOfLatestVersion(getPropertiesOfLatestVersionRequest request);

        /// <summary>
        /// Returns the list of all document objects in the speciﬁed version series, sorted by cmis:creationDate descending.
        /// Notes: If a Private Working Copy exists for the version series and the caller has permissions to access it, then it MUST be returned as the ﬁrst object in the result list.
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the object. </param>
        /// <param name="versionSeriesId">Alternative of objectId. The identiﬁer for the version series. Actually, it is the object id.</param>
        /// <param name="filter">Optional. Value indicating which properties for objects MUST be returned. This ﬁlter is a list of property query names and NOT a list of property ids. The query names of secondary type properties MUST follow the pattern <secondaryTypeQueryName>.<propertyQueryName>.Example: cmis:name,amount,worflow.stage.</param>
        /// <param name="includeAllowableActions">Optional. If TRUE, then the Repository MUST return the available actions for each object in the result set. Defaults to FALSE.</param>
        /// <returns></returns>
        [OperationContract(Name = "getAllVersions")]
        getAllVersionsResponse GetAllVersions(getAllVersionsRequest request);
    }
}
