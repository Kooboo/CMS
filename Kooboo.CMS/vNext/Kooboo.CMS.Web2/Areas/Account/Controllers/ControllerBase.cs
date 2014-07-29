#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Common.ComponentModel;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Web2.Areas.Account.Models;
using Kooboo.CMS.Sites;
//using Kooboo.CMS.Web2.ErrorHandling;

namespace Kooboo.CMS.Web2.Areas.Account.Controllers
{
    //[CmsHandleError(Order = 99)]
    public abstract class ControllerBase : AreaControllerBase
    {
        static ControllerBase()
        {
            //TypeDescriptorHelper.RegisterMetadataType(typeof(Role), typeof(Role_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(User), typeof(User_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(Setting), typeof(Setting_Metadata));
        }
    }
}
