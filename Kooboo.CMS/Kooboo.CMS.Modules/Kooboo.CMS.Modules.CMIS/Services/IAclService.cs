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
    /// The ACL Services are used to discover and manage Access Control Lists. 
    /// </summary>
    [ServiceContract(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/", Name = "AclService")]
    [XmlSerializerFormat]
    public interface IAclService
    {
        /// <summary>
        /// Adds or removes the given ACEs to or from the ACL of an object .
        /// Notes: This service MUST be supported by the repository, if the optional capability capabilityACL is manage.
        /// How ACEs are added or removed to or from the object is repository speciﬁc – with respect to the ACLPropagation parameter.
        /// Some ACEs that make up an object’s ACL may not be set directly on the object, but determined in other ways, such as inheritance. A repository MAY merge the ACEs provided with the ACEs of the ACL already applied to the object (i.e. the ACEs provided MAY not be completely added or removed from the eﬀective ACL for the object).
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <param name="objectId">Required. The identiﬁer for the object.</param>
        /// <param name="addACEs">Optional. The ACEs to be added. </param>
        /// <param name="removeACEs">Optional. The ACEs to be removed. </param>
        /// <param name="aclPropagation">Optional. Speciﬁes how ACEs should be handled.</param>
        /// <returns></returns>
        [OperationContract(Name = "applyAcl")]
        applyACLResponse ApplyAcl(applyACLRequest request);

        /// <summary>
        /// Get the ACL currently applied to the speciﬁed object. 
        /// Notes: This service MUST be supported by the repository, if the optional capability capabilityACL is discover or manage. A client MUST NOT assume that the returned ACEs can be applied via applyACL. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository.</param>
        /// <param name="objectId">Required. The identiﬁer for the object.</param>
        /// <param name="onlyBasicPermissions">Optional. The repository SHOULD make a best eﬀort to fully express the native security applied to the object. 
        /// ◦TRUE (default) indicates that the client requests that the returned ACL be expressed using only the CMIS basic permissions. 
        /// ◦FALSE indicates that the server may respond using either solely CMIS basic permissions, or repository speciﬁc permissions or some combination of both.
        /// </param>
        /// <returns></returns>
        [OperationContract(Name = "getACL")]
        getACLResponse GetAcl(getACLRequest request);
    }
}
