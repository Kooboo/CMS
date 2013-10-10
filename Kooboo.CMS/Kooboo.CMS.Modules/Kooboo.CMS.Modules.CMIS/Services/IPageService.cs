#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Services
{
    [ServiceContract(Name = "PageService")]
    [DataContractFormat]
    public interface IPageService
    {
        [OperationContract(Name = "addPage")]
        string AddPage(string repositoryId, string pageId, Page properties);
        [OperationContract(Name = "updatePage")]
        string UpdatePage(string repositoryId, string pageId, Page properties);
        [OperationContract(Name = "deletePage")]
        void DeletePage(string repositoryId, string pageId);
    }
}
