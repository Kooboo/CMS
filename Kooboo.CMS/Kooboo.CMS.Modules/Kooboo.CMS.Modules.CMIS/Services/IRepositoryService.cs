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
    /// The Repository Services are used to discover information about the repository, including information about the repository and the object-types deﬁned for the repository. Furthermore, it provides operations to create, modify and delete object-type deﬁnitions if that is supported by the repository. 
    /// createType,updateType,deleteType doest implemented.
    /// </summary>
    [ServiceContract(Namespace = CmisNs.Cmism, Name = "RepositoryService")]
    [XmlSerializerFormat(SupportFaults = true)]
    public partial interface IRepositoryService
    {
        ///// <summary>
        /// Returns a list of CMIS repositories available from this CMIS service endpoint. 
        /// </summary>
        /// <returns>Collection of repositories.</returns>
        //[OperationContract(Name = "getRepositories", Action = "")] //http://social.msdn.microsoft.com/Forums/vstudio/en-US/01e26316-32cf-4d82-92ad-fed05a4b82ff/implementing-a-wcf-service-from-wsdl-without-soapaction
        [System.ServiceModel.OperationContractAttribute(Name = "getRepositories", Action = "")]
        getRepositoriesResponse GetRepositories(getRepositoriesRequest request);

        /// <summary>
        /// Returns information about the CMIS repository, the optional capabilities it supports and its access control information if applicable. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <returns>RepositoryInfo object for specified repository.</returns>
        [OperationContract(Name = "getRepositoryInfo")]
        getRepositoryInfoResponse GetRepositoryInfo(getRepositoryInfoRequest request);

        /// <summary>
        /// Returns the list of object-types deﬁned for the repository that are children of the speciﬁed type. 
        /// </summary>
        /// <param name="repositoryId">Required. The repository id.</param>
        /// <param name="typeId">Optional. The type id of an object-type to retreive children for.</param>
        /// <param name="includePropertyDefinitions">Optional. If TRUE, then the repository MUST return the property deﬁnitions for each object-type. If FALSE (default), the repository MUST return only the attributes for each object-type. </param>
        /// <param name="maxItems">Optional. The maximum number of items to return in the response. (optional)</param>
        /// <param name="skipCount">Optional. The number of potential results to skip before returning any results.</param>
        /// <returns>The specified base types.</returns>
        [OperationContract(Name = "getTypeChildren")]
        getTypeChildrenResponse GetTypeChildren(getTypeChildrenRequest request);

        /// <summary>
        /// Returns the set of the descendant object-types deﬁned for the Repository under the speciﬁed type. 
        /// Notes: 
        /// •This method does NOT support paging as deﬁned in the 2.2.1.1 Paging section. 
        /// •The order in which results are returned is respository-speciﬁc.
        /// </summary>
        /// <param name="repositoryId">Required. The id of the repository.</param>
        /// <param name="typeId">Optional. The typeId of an object-type speciﬁed in the repository. 
        /// ◦If speciﬁed, then the repository MUST return all of descendant types of the speciﬁed type. 
        /// ◦If not speciﬁed, then the Repository MUST return all types and MUST ignore the value of the depth parameter.
        /// </param>
        /// <param name="depth">Optional. The number of levels of depth in the type hierarchy from which to return results.
        /// 1 Return only types that are children of the type. See also getTypeChildren. <Integer value greater than 1> Return only types that are children of the type and descendants up to <value> levels deep. -1 Return ALL descendant types at all depth levels in the CMIS hierarchy.
        /// The default value is repository speciﬁc and SHOULD be at least 2 or -1. 
        /// </param>
        /// <param name="includePropertyDefinitions">Optional. If TRUE, then the repository MUST return the property deﬁnitions for each object-type. If FALSE (default), the repository MUST return only the attributes for each object-type. </param>
        /// <returns>The specified type's descendants.</returns>
        [OperationContract(Name = "getTypeDescendants")]
        getTypeDescendantsResponse GetTypeDescendants(getTypeDescendantsRequest request);

        /// <summary>
        /// Gets the deﬁnition of the speciﬁed object-type. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="typeId">Required. The typeId of an object-type speciﬁed in the repository. </param>
        /// <returns>Type definition for specified object-type.</returns>
        [OperationContract(Name = "getTypeDefinition")]
        getTypeDefinitionResponse GetTypeDefinition(getTypeDefinitionRequest request);
    }
}
