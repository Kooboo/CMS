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
    /// The Relationship Services are used to retrieve the dependent relationship objects associated with an independent object. 
    /// </summary>
    [ServiceContract(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/", Name = "RelationshipService")]
    [XmlSerializerFormat]
    public interface IRelationshipService
    {
        /// <summary>
        /// Gets all or a subset of relationships associated with an independent object. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the object. </param>
        /// <param name="includeSubRelationshipTypes">Optional. If TRUE, then the repository MUST return all relationships whose object-types are descendant-types of the object-type speciﬁed by the typeId parameter value as well as relationships of the speciﬁed type. If FALSE (default), then the repository MUST only return relationships whose object-types is equivalent to the object-type speciﬁed by the typeId parameter value. If the typeId input is not speciﬁed, then this input MUST be ignored. </param>
        /// <param name="relationshipDirection">Optional. An enumeration specifying whether the repository MUST return relationships where the speciﬁed object is the source of the relationship, the target of the relationship, or both. </param>
        /// <param name="typeId">Optional. If speciﬁed, then the repository MUST return only relationships whose object-type is of the type speciﬁed.If not speciﬁed, then the repository MUST return relationship objects of all types. </param>
        /// <param name="filter">Optional. Value indicating which properties for objects MUST be returned. This ﬁlter is a list of property query names and NOT a list of property ids. The query names of secondary type properties MUST follow the pattern <secondaryTypeQueryName>.<propertyQueryName>.Example: cmis:name,amount,worflow.stage.</param>
        /// <param name="includeAllowableActions">Optional. If TRUE, then the Repository MUST return the available actions for each object in the result set. Defaults to FALSE.</param>
        /// <param name="maxItems">(optional) This is the maximum number of items to return in a response. The repository MUST NOT exceed this maximum. Default is repository-speciﬁc. http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html#x1-1510001 </param>
        /// <param name="skipCount">(optional) This is the number of potential results that the repository MUST skip/page over before returning any results. Defaults to 0.</param>
        /// <returns></returns>
        [OperationContract(Name = "getObjectRelationships")]
        getObjectRelationshipsResponse GetObjectRelationships(getObjectRelationshipsRequest request);
    }
}
