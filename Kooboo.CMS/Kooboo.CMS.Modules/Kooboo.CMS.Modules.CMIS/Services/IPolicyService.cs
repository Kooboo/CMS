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
    /// The Policy Services are used to apply or remove a policy object to a controllablePolicy object. 
    /// </summary>
    [ServiceContract(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/", Name = "PolicyService")]
    [XmlSerializerFormat]
    public interface IPolicyService
    {
        /// <summary>
        /// Applies a speciﬁed policy to an object. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="policyId">Required. The identiﬁer for the policy to be applied. </param>
        /// <param name="objectId">Required. The identiﬁer of the object. </param>
        [OperationContract(Name = "applyPolicy")]
        applyPolicyResponse ApplyPolicy(applyPolicyRequest request);

        /// <summary>
        /// Removes a speciﬁed policy from an object. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="policyId">Required. The identiﬁer for the policy to be applied. </param>
        /// <param name="objectId">Required. The identiﬁer of the object. </param>
        [OperationContract(Name = "removePolicy")]
        removePolicyResponse RemovePolicy(removePolicyRequest request);

        /// <summary>
        /// Gets the list of policies currently applied to the speciﬁed object. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer of the object. </param>
        /// <param name="filter">Optional. Value indicating which properties for objects MUST be returned. This ﬁlter is a list of property query names and NOT a list of property ids. The query names of secondary type properties MUST follow the pattern <secondaryTypeQueryName>.<propertyQueryName>.Example: cmis:name,amount,worflow.stage.</param>
        /// <returns></returns>
        [OperationContract(Name = "getAppliedPolicies")]
        getAppliedPoliciesResponse GetAppliedPolicies(getAppliedPoliciesRequest request);
    }
}
