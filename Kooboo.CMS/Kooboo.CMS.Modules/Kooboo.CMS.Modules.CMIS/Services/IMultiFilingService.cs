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
    /// The Multi-ﬁling services are supported only if the repository supports the multiﬁling or unﬁling optional capabilities (capabilityMultifiling). The Multi-ﬁling Services are used to ﬁle/un-ﬁle objects into/from folders. 
    /// This service is NOT used to create or delete objects in the repository. 
    /// </summary>
    [ServiceContract(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/", Name = "MultiFilingService")]
    [XmlSerializerFormat]
    public interface IMultiFilingService
    {
        /// <summary>
        ///  Adds an existing ﬁleable non-folder object to a folder. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the object. </param>
        /// <param name="folderId">Required. The folder into which the object is to be ﬁled. </param>
        /// <param name="allVersions">Optional. Add all versions of the object to the folder if the repository supports version-speciﬁc ﬁling. Defaults to TRUE. </param>
        [OperationContract(Name = "adObjectToFolder")]
        addObjectToFolderResponse AddObjectToFolder(addObjectToFolderRequest request);

        /// <summary>
        /// Removes an existing ﬁleable non-folder object from a folder. 
        /// </summary>
        /// <param name="repositoryId">Required. The identiﬁer for the repository. </param>
        /// <param name="objectId">Required. The identiﬁer for the object. </param>
        /// <param name="folderId">Optional. The folder from which the object is to be removed. If no value is speciﬁed, then the repository MUST remove the object from all folders in which it is currently ﬁled. </param>
        [OperationContract(Name = "removeObjectFromFolder")]
        removeObjectFromFolderResponse RemoveObjectFromFolder(removeObjectFromFolderRequest request);
    }
}
