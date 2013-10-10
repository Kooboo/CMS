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
    /// The Discovery Services are used to search for query-able objects within the repository. 
    /// </summary>
    [ServiceContract(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/", Name = "DiscoveryService")]
    [XmlSerializerFormat]
    public interface IDiscoveryService
    {
        /// <summary>
        ///  Executes a CMIS query statement against the contents of the repository. 
        /// </summary>
        /// <param name="repositoryId">Required.  The identiﬁer for the repository. </param>
        /// <param name="statement">Required. CMIS query to be executed.Note: The AtomPub and the Browser Binding also use the name q. http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html#x1-10500014</param>
        /// <param name="searchAllVersions">Optional. If TRUE, then the repository MUST include latest and non-latest versions of document objects in the query search scope. If FALSE (default), then the repository MUST only include latest versions of documents in the query search scope. If the repository does not support the optional capabilityAllVersionsSearchable capability, then this parameter value MUST be set to FALSE. </param>
        /// <param name="includeAllowableActions">Optional. For query statements where the SELECT clause contains properties from only one virtual table reference (i.e. referenced object-type), any value for this parameter may be used. If the SELECT clause contains properties from more than one table, then the value of this parameter MUST be "FALSE". Defaults to FALSE. </param>
        /// <param name="includeRelationships">Optional. For query statements where the SELECT clause contains properties from only one virtual table reference (i.e. referenced object-type), any value for this enum may be used. If the SELECT clause contains properties from more than one table, then the value of this parameter MUST be none. Defaults to none. </param>
        /// <param name="renditionFilter">Optional. The rendition filter.</param>
        /// <param name="maxItems">(optional) This is the maximum number of items to return in a response. The repository MUST NOT exceed this maximum. Default is repository-speciﬁc. http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html#x1-1510001 </param>
        /// <param name="skipCount">(optional) This is the number of potential results that the repository MUST skip/page over before returning any results. Defaults to 0.</param>
        /// <returns></returns>
        [OperationContract(Name = "query")]
        queryResponse Query(queryRequest request);

        /// <summary>
        ///Gets a list of content changes. This service is intended to be used by search crawlers or other applications that need to eﬃciently understand what has changed in the repository.
        ///Notes: 
        ///•The content stream is NOT returned for any change event. 
        ///•The deﬁnition of the authority needed to call this service is repository speciﬁc. 
        ///•The latest change log token for a repository can be acquired via the getRepositoryInfo service.
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="changeToken">Optional. If speciﬁed, then the repository MUST return the change event corresponding to the value of the speciﬁed change log token as the ﬁrst result in the output. If not speciﬁed, then the repository MUST return the ﬁrst change event recorded in the change log. </param>
        /// <param name="includeProperties">Optional.  If TRUE, then the repository MUST include the updated property values for "updated" change events if the repository supports returning property values as speciﬁed by capbilityChanges. If FALSE (default), then the repository MUST NOT include the updated property values for "updated" change events. The single exception to this is that the property cmis:objectId MUST always be included. </param>
        /// <param name="includePolicyIds">Optional.  If TRUE, then the repository MUST include the ids of the policies applied to the object referenced in each change event, if the change event modiﬁed the set of policies applied to the object. If FALSE (default), then the repository MUST not include policy information. </param>
        /// <param name="filter">Optional. The filter.</param>
        /// <param name="includeAcl">Optional. if set to <c>true</c> [include acl].</param>
        /// <param name="maxItems">(optional) This is the maximum number of items to return in a response. The repository MUST NOT exceed this maximum. Default is repository-speciﬁc. http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html#x1-1510001 </param>
        /// <returns></returns>
        [OperationContract(Name = "getContentChanges")]
        getContentChangesResponse GetContentChanges(getContentChangesRequest request);
    }
}
