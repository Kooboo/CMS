#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Modules.CMIS.WcfExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Services
{
    [ServiceContract(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    [DispatchByBodyElementBehaviorAttribute]
    public interface IService : IRepositoryService, IObjectService, INavigationService//, IPageService
    {
    }
}
